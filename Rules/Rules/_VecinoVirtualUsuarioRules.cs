using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DAO.DAO;
using Model;
using Model.Entities;
using Encriptacion;
using Model.Resultados;
using Model.Consultas;
using Model.Comandos;
using Model.WSVecinoVirtual.Resultados;
using Model.WSVecinoVirtual.Comandos;
using System.Globalization;

namespace Rules.Rules
{

    public class _VecinoVirtualUsuarioRules : BaseRules<_VecinoVirtualUsuario>
    {

        private readonly _VecinoVirtualUsuarioDAO dao;


        private readonly string baseUrl = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"];

        public _VecinoVirtualUsuarioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = _VecinoVirtualUsuarioDAO.Instance;
        }


        public Result<UsuarioLogueado> IniciarSesionEmpleado(string token)
        {
            var resultado = new Result<UsuarioLogueado>();

            try
            {
                //Consulto en Cerrojo
                var resultadoDatos = RestCall.Call<VVResultado_Usuario>(baseUrl + "v1/Usuario?token=" + token, RestSharp.Portable.Method.GET);
                if (!resultadoDatos.Ok)
                {
                    resultado.AddErrorPublico(resultadoDatos.Error);
                    return resultado;
                }

                //Valido empleado
                if (resultadoDatos.Return.Empleado == false)
                {
                    resultado.AddErrorPublico("El usuario indicado no es un empleado en la Municipalidad de Córdoba");
                    return resultado;
                }

                var usuarioLogeado = new UsuarioLogueado();
                usuarioLogeado.Token = resultadoDatos.Return.Token;


                //Usuario
                var resultadoUsuario = GetByIdObligatorio(resultadoDatos.Return.IdCerrojo);
                if (!resultadoUsuario.Ok)
                {
                    resultado.Copy(resultadoUsuario.Errores);
                    return resultado;
                }
                usuarioLogeado.Usuario = new _Resultado_VecinoVirtualUsuario(resultadoUsuario.Return);

                var app = ConfigurationManager.AppSettings["CERROJO_APP_IDENTIFIER"] + "";
                //Ambito
                var resultadoAmbito = RestCall.Call<VVResultado_AmbitoTrabajo>(baseUrl + "v1/Usuario/AmbitoTrabajo?token=" + token + "&app=" + app, RestSharp.Portable.Method.GET);
                if (!resultadoAmbito.Ok)
                {
                    resultado.AddErrorPublico(resultadoAmbito.Error);
                    return resultado;
                }
                usuarioLogeado.Ambito = new Resultado_Ambito(new _CerrojoAmbitoRules(null).GetByIdObligatorio(resultadoAmbito.Return.IdCerrojo).Return);

                //Areas
                var resultadoAreas = RestCall.Call<List<VVResultado_AmbitoTrabajo>>(baseUrl + "v1/Usuario/Areas?token=" + token + "&app=" + app, RestSharp.Portable.Method.GET);
                if (!resultadoAreas.Ok)
                {
                    resultado.AddErrorPublico(resultadoAreas.Error);
                    return resultado;
                }


                var areasCerrojo = resultadoAreas.Return;

                //Valido que tenga al menos un area
                if (areasCerrojo == null || areasCerrojo.Count() == 0)
                {
                    resultado.AddErrorPublico("El usuario no tiene ningun area asociada");
                    return resultado;
                }

                //Tomo el primer area para pedirle el rol
                var idPrimerArea = areasCerrojo[0].IdCerrojo;
                var resultadoRol = RestCall.Call<VVResultado_Permisos>(baseUrl + "v1/Usuario/Permisos?token=" + token + "&app=" + app + "&idArea=" + areasCerrojo[0].IdCerrojo, RestSharp.Portable.Method.GET);
                if (!resultadoRol.Ok)
                {
                    resultado.AddErrorPublico(resultadoRol.Error);
                    return resultado;
                }

                var rol = resultadoRol.Return;
                if (rol == null)
                {
                    resultado.AddErrorPublico("El usuario no tiene asociado ningun rol");
                    return resultado;
                }
                usuarioLogeado.Rol = rol;


                var areaRules = new _CerrojoAreaRules(usuarioLogeado);
                var areas = new List<Resultado_Area>();

                //Si es Administrador le pongo todas las areas
                if (rol.Rol.ToUpper() == "ADMINISTRADOR")
                {
                    var resultadoConsultaAreas = areaRules.GetAll(false);
                    if (!resultadoConsultaAreas.Ok)
                    {
                        resultado.AddErrorPublico(resultadoConsultaAreas.Error);
                        return resultado;
                    }

                    areas = Resultado_Area.ToList(resultadoConsultaAreas.Return);
                }
                else
                {
                    //Sino busco las areas
                    foreach (var areaCerrojo in areasCerrojo)
                    {
                        var resultadoConsultaArea = areaRules.GetByIdObligatorio(areaCerrojo.IdCerrojo);
                        if (!resultadoConsultaArea.Ok)
                        {
                            resultado.AddErrorPublico(resultadoConsultaArea.Error);
                            return resultado;
                        }

                        var area = resultadoConsultaArea.Return;
                        if (area == null)
                        {
                            resultado.AddErrorPublico("No existe un área asociada al usuario");
                            return resultado;
                        }

                        areas.Add(new Resultado_Area(area));
                    }
                }

                //Quito el area municipalidad de Córdoba que viene codigo negativo (-10)
                usuarioLogeado.Areas = areas.Where(x => !String.Equals(x.CodigoMunicipal, "-10")).ToList();

                //Origenes
                var resultadoOrigenes = new OrigenRules(usuarioLogeado).GetAmbitosDeUsuarioLogeado();
                if (!resultadoOrigenes.Ok)
                {
                    resultado.AddErrorPublico(resultadoOrigenes.Error);
                    return resultado;
                }

                if (resultadoOrigenes.Return == null || resultadoOrigenes.Return.Count() == 0)
                {
                    resultado.AddErrorPublico("El ususario no tiene asociado ningun origen");
                    return resultado;
                }
                usuarioLogeado.OrigenesDisponibles = resultadoOrigenes.Return;

                //Si tiene un solo origen se lo seteo, sino mas adeltante pregunto
                if (resultadoOrigenes.Return.Count == 1)
                {
                    usuarioLogeado.IdOrigenElegido = resultadoOrigenes.Return[0].Id;
                }

                //Menu
                var resultadoMenu = RestCall.Call<List<VVResultado_Menu>>(baseUrl + "v1/Usuario/Menu?token=" + token + "&app=" + app, RestSharp.Portable.Method.GET);
                if (!resultadoMenu.Ok)
                {
                    resultado.AddErrorPublico(resultadoMenu.Error);
                    return resultado;
                }
                var menu = resultadoMenu.Return;
                if (menu == null)
                {
                    resultado.AddErrorPublico("Error cargando el menu");
                    return resultado;
                }

                usuarioLogeado.Menu = menu;
                resultado.Return = usuarioLogeado;
            }

            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }


        public Result<List<_VecinoVirtualUsuario>> GetByFilters(Consulta_VecinoVirtualUsuario consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<List<_Resultado_VecinoVirtualUsuario>> GetResultadoByFilters(Consulta_VecinoVirtualUsuario consulta)
        {
            var resultado = new Result<List<_Resultado_VecinoVirtualUsuario>>();

            var resultadoConsulta = dao.GetByFilters(consulta);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            resultado.Return = _Resultado_VecinoVirtualUsuario.ToList(resultadoConsulta.Return);
            return resultado;
        }


        public Result<Resultado_InitData> GetInitData()
        {
            var resultado = new Result<Resultado_InitData>();
            Resultado_InitData data = new Resultado_InitData();

            data.Requerimiento.Permisos = new PermisoEstadoRequerimientoRules(getUsuarioLogueado()).GetPermisos().Return;
            data.Requerimiento.Estados = Resultado_EstadoRequerimiento.ToList(new EstadoRequerimientoRules(getUsuarioLogueado()).GetAll().Return);

            data.OrdenTrabajo.Permisos = new PermisoEstadoOrdenTrabajoRules(getUsuarioLogueado()).GetPermisos().Return;
            data.OrdenTrabajo.Estados = Resultado_EstadoOrdenTrabajo.ToList(new EstadoOrdenTrabajoRules(getUsuarioLogueado()).GetAll().Return);

            resultado.Return = data;
            return resultado;
        }


        public Result<_Resultado_VecinoVirtualUsuario> CrearUsuario(Comando_UsuarioVecinoVirtualNuevo usuario, bool passwordDefault, bool activado, bool empleado, string urlRetorno)
        {
            var result = new Result<_Resultado_VecinoVirtualUsuario>();
            var user = new VVComando_CrearUsuario();
            user.Comando = new VVComando_UsuarioNuevo();

            //Datos personales
            user.Comando.Nombre = usuario.Nombre;
            user.Comando.Apellido = usuario.Apellido;
            user.Comando.FechaNacimiento = usuario.FechaNacimiento;
            user.Comando.Dni = usuario.Dni;
            user.Comando.SexoMasculino = usuario.SexoMasculino;
            user.Comando.IdEstadoCivil = usuario.IdEstadoCivil;
            if (usuario.Domicilio != null)
            {
                user.Comando.Domicilio = new VVComando_UsuarioActualizarDomicilio()
                {
                    Direccion = usuario.Domicilio.Direccion,
                    Altura = usuario.Domicilio.Altura,
                    Torre = usuario.Domicilio.Torre,
                    Piso = usuario.Domicilio.Piso,
                    Depto = usuario.Domicilio.Depto,
                    CodigoPostal = usuario.Domicilio.CodigoPostal,
                    Barrio = usuario.Domicilio.Barrio,
                    IdBarrio = usuario.Domicilio.IdBarrio,
                    Ciudad = usuario.Domicilio.Ciudad,
                    IdCiudad = usuario.Domicilio.IdCiudad,
                    Provincia = usuario.Domicilio.Provincia,
                    IdProvincia = usuario.Domicilio.IdProvincia
                };
            }

            //Aceso
            user.Comando.Username = usuario.Username;
            user.Comando.Password = usuario.Password;
            user.PasswordDefault = passwordDefault;

            if (empleado)
            {
                user.Comando.Cargo = usuario.Cargo;
                user.Comando.Empleado = true;
                user.KeyEmpleado = ConfigurationManager.AppSettings["CERROJO_USUARIO_EMPLEADO"];
            }

            //Contacto
            user.Comando.Email = usuario.Email;
            user.Comando.TelefonoFijo = usuario.TelefonoFijo;
            user.Comando.TelefonoCelular = usuario.TelefonoCelular;
            user.Comando.Facebook = usuario.Facebook;
            user.Comando.Twitter = usuario.Twitter;
            user.Comando.Instagram = usuario.Instagram;
            user.Comando.LinkedIn = usuario.LinkedIn;

            //Consulto en Cerrojo
            if (activado)
            {
                user.KeyActivacion = ConfigurationManager.AppSettings["VECINO_VIRTUAL_KEY_USUARIO_VALIDADO"];
                user.UrlRetorno = "aa";
                user.UrlServidor = "bb";
            }
            else
            {
                user.KeyActivacion = null;
                user.UrlRetorno = urlRetorno;
                user.UrlServidor = ConfigurationManager.AppSettings["URL_ACTIVAR_USUARIO"];
            }


            try
            {
                var resultCrearUsuario = RestCall.Call<VVResultado_Usuario>(baseUrl + "v1/Usuario", RestSharp.Portable.Method.POST, user);
                if (!resultCrearUsuario.Ok)
                {
                    result.AddErrorPublico(resultCrearUsuario.Error);
                    return result;
                }

                return GetResultadoByIdObligatorio(resultCrearUsuario.Return.IdCerrojo);
            }

            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }


        /**
         * Actualiza a un usuario externo. Se indica cual por el ID del comando
         *
         * **/
        public Result<_Resultado_VecinoVirtualUsuario> ActualizarUsuario(Comando_UsuarioVecinoVirtualEditar comando)
        {
            var result = new Result<_Resultado_VecinoVirtualUsuario>();

            var resultadoConsulta = GetByIdObligatorio(comando.Id);
            if (!resultadoConsulta.Ok)
            {
                result.Copy(resultadoConsulta.Errores);
                return result;
            }

            var comandoWS = new VVComando_ActualizarUsuario();
            comandoWS.Comando = new VVComando_UsuarioActualizar();
            comandoWS.Comando.Id = comando.Id;
            if (resultadoConsulta.Return != null && resultadoConsulta.Return.FechaValidacionRenaper == null)
            {
                comandoWS.Comando.Nombre = comando.Nombre;
                comandoWS.Comando.Apellido = comando.Apellido;
                comandoWS.Comando.Dni = comando.Dni;
                comandoWS.Comando.Email = comando.Email;
                comandoWS.Comando.SexoMasculino = comando.SexoMasculino;
                comandoWS.Comando.FechaNacimiento = comando.FechaNacimiento;
            }

            if (resultadoConsulta.Return.Empleado)
            {

                comandoWS.KeyEmpleado = ConfigurationManager.AppSettings["CERROJO_USUARIO_EMPLEADO"];
                comandoWS.Comando.Empleado = true;
                comandoWS.Comando.Cargo = comando.Cargo;
                if (resultadoConsulta.Return.FechaJubilacion.HasValue)
                {
                    comandoWS.Comando.FechaJubilacion = ((DateTime)resultadoConsulta.Return.FechaJubilacion).ToString("dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                }
                
                comandoWS.Comando.Funcion = resultadoConsulta.Return.Funcion;
            }

            comandoWS.Comando.EstadoCivilId = comando.IdEstadoCivil;
            comandoWS.Comando.Email = comando.Email;
            comandoWS.Comando.TelefonoFijo = comando.TelefonoFijo;
            comandoWS.Comando.TelefonoCelular = comando.TelefonoCelular;
            comandoWS.Comando.Facebook = comando.Facebook;
            comandoWS.Comando.Twitter = comando.Twitter;
            comandoWS.Comando.Instagram = comando.Instagram;
            comandoWS.Comando.LinkedIn = comando.LinkedIn;


            try
            {
                //Consulto en Cerrojo
                string keyUsuarioEditar = ConfigurationManager.AppSettings["VECINO_VIRTUAL_KEY_EDITAR_USUARIOS"];
                comandoWS.KeyEdicion = keyUsuarioEditar;
                var resultadoEditar = RestCall.Call<VVResultado_Usuario>(baseUrl + "v1/Usuario/Usuario?token=" + getUsuarioLogueado().Token, RestSharp.Portable.Method.PUT, comandoWS);
                if (!resultadoEditar.Ok)
                {
                    result.AddErrorPublico(resultadoEditar.Error);
                    return result;
                }

                return GetResultadoByIdObligatorio(resultadoEditar.Return.IdCerrojo);
            }

            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<string> ActualizarFotoPersonal(Comando_UsuarioVecinoVirtualEditarFotoPersonal comando)
        {
            return CambiarFoto(getUsuarioLogueado().Usuario.Id, comando.Content);
        }

        public Result<_Resultado_VecinoVirtualUsuario> ActualizarDatosPersonales(Comando_UsuarioVecinoVirtualEditarDatosPersonales comando)
        {
            var result = new Result<_Resultado_VecinoVirtualUsuario>();

            var comandoWS = new VVComando_UsuarioActualizarDatosPersonales();
            comandoWS.Nombre = comando.Nombre;
            comandoWS.Apellido = comando.Apellido;
            comandoWS.Dni = comando.Dni;
            comandoWS.SexoMasculino = comando.SexoMasculino;
            comandoWS.FechaNacimiento = comando.FechaNacimiento;

            try
            {
                //Consulto en Cerrojo
                var resultadoEditar = RestCall.Call<VVResultado_Usuario>(baseUrl + "v1/Usuario/ActualizarDatosPersonales?token=" + getUsuarioLogueado().Token, RestSharp.Portable.Method.PUT, comandoWS);
                if (!resultadoEditar.Ok)
                {
                    result.AddErrorPublico(resultadoEditar.Error);
                    return result;
                }

                return GetResultadoByIdObligatorio(resultadoEditar.Return.IdCerrojo);
            }

            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<_Resultado_VecinoVirtualUsuario> ActualizarDatosContacto(Comando_UsuarioVecinoVirtualEditarDatosContacto comando)
        {
            var result = new Result<_Resultado_VecinoVirtualUsuario>();

            var comandoWS = new VVComando_UsuarioActualizarDatosContacto();
            comandoWS.TelefonoFijo = comando.TelefonoFijo;
            comandoWS.TelefonoCelular = comando.TelefonoCelular;
            comandoWS.Email = comando.Email;
            comandoWS.Facebook = comando.Facebook;
            comandoWS.Twitter = comando.Twitter;
            comandoWS.Instagram = comando.Instagram;
            comandoWS.LinkedIn = comando.LinkedIn;

            try
            {
                //Consulto en Cerrojo
                var resultadoEditar = RestCall.Call<VVResultado_Usuario>(baseUrl + "v1/Usuario/ActualizarDatosContacto?token=" + getUsuarioLogueado().Token, RestSharp.Portable.Method.PUT, comandoWS);
                if (!resultadoEditar.Ok)
                {
                    result.AddErrorPublico(resultadoEditar.Error);
                    return result;
                }

                return GetResultadoByIdObligatorio(resultadoEditar.Return.IdCerrojo);
            }

            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<_Resultado_VecinoVirtualUsuario> ActualizarDomicilio(Comando_UsuarioVecinoVirtualActualizarDomicilio comando)
        {
            var result = new Result<_Resultado_VecinoVirtualUsuario>();

            var comandoWS = new VVComando_UsuarioActualizarDomicilio();
            comandoWS.Direccion = comando.Direccion;
            comandoWS.Altura = comando.Altura;
            comandoWS.Torre = comando.Torre;
            comandoWS.Piso = comando.Piso;
            comandoWS.Depto = comando.Depto;
            comandoWS.CodigoPostal = comando.CodigoPostal;
            comandoWS.Ciudad = comando.Ciudad;
            comandoWS.IdCiudad = comando.IdCiudad;
            comandoWS.Barrio = comando.Barrio;
            comandoWS.IdBarrio = comando.IdBarrio;
            comandoWS.Provincia = comando.Provincia;
            comandoWS.IdProvincia = comando.IdProvincia;

            try
            {
                //Consulto en Cerrojo
                var url = baseUrl + "v1/Usuario/ActualizarDomicilio?token=" + getUsuarioLogueado().Token;
                var resultadoEditar = RestCall.Call<VVResultado_Usuario>(url, RestSharp.Portable.Method.PUT, comandoWS);
                if (!resultadoEditar.Ok)
                {
                    result.AddErrorPublico(resultadoEditar.Error);
                    return result;
                }

                return GetResultadoByIdObligatorio(resultadoEditar.Return.IdCerrojo);
            }

            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<_Resultado_VecinoVirtualUsuario> ActualizarEstadoCivil(int idEstadoCivil)
        {
            var result = new Result<_Resultado_VecinoVirtualUsuario>();

            try
            {
                //Consulto en Cerrojo
                var resultadoEditar = RestCall.Call<VVResultado_Usuario>(baseUrl + "v1/Usuario/ActualizarEstadoCivil?token=" + getUsuarioLogueado().Token + "&idEstadoCivil=" + idEstadoCivil, RestSharp.Portable.Method.PUT, null);
                if (!resultadoEditar.Ok)
                {
                    result.AddErrorPublico(resultadoEditar.Error);
                    return result;
                }

                return GetResultadoByIdObligatorio(resultadoEditar.Return.IdCerrojo);
            }

            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<bool?> EsValidadoRenaper()
        {
            var result = new Result<bool?>();

            try
            {
                //Consulto en Cerrojo
                var resultadoConsulta = RestCall.Call<bool?>(baseUrl + "/v1/Usuario/ValidadoRenaper?token=" + getUsuarioLogueado().Token, RestSharp.Portable.Method.GET);
                if (!resultadoConsulta.Ok)
                {
                    result.AddErrorPublico(resultadoConsulta.Error);
                    return result;
                }

                result.Return = resultadoConsulta.Return;
            }

            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }


        public Result<string> CambiarFoto(int id, string content)
        {
            var result = new Result<string>();
            try
            {
                var comando = new VVComando_UsuarioCambiarFoto();
                comando.Base64 = content;
                comando.Id = id;

                string keyUsuarioEditar = ConfigurationManager.AppSettings["VECINO_VIRTUAL_KEY_EDITAR_USUARIOS"];
                comando.Key = keyUsuarioEditar;

                var resultadoFoto = RestCall.Call<string>(baseUrl + "v1/Usuario/CambiarFotoPerfil?token=" + getUsuarioLogueado().Token, RestSharp.Portable.Method.PUT, comando);
                if (!resultadoFoto.Ok)
                {
                    result.AddErrorPublico(resultadoFoto.Error);
                    return result;
                }
                result.Return = resultadoFoto.Return;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<bool> CambiarPassword(string passwordAnterior, string passwordNueva)
        {
            var result = new Result<bool>();
            try
            {
                var comando = new VVComando_CambiarPassword();
                comando.PasswordAnterior = passwordAnterior;
                comando.PasswordNueva = passwordNueva;

                var resultClave = RestCall.Call<bool>(baseUrl + "v1/Usuario/CambiarPassword?token=" + getUsuarioLogueado().Token, RestSharp.Portable.Method.PUT, comando);
                if (!resultClave.Ok)
                {
                    result.AddErrorPublico(resultClave.Error);
                    return result;
                }
                result.Return = true;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<bool> CambiarUsername(string username)
        {
            var result = new Result<bool>();
            try
            {
                var resultUsername = RestCall.Call<bool>(baseUrl + "v1/Usuario/CambiarUsername?token=" + getUsuarioLogueado().Token + "&username=" + username, RestSharp.Portable.Method.PUT);

                if (!resultUsername.Ok)
                {
                    result.AddErrorPublico(resultUsername.Error);
                    return result;
                }
                result.Return = true;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<bool> ValidarToken(string token)
        {
            var result = new Result<bool>();
            try
            {
                var resultUsername = RestCall.Call<bool>(baseUrl + "v1/Usuario/ValidarToken?token=" + token, RestSharp.Portable.Method.GET);

                if (!resultUsername.Ok)
                {
                    result.AddErrorPublico(resultUsername.Error);
                    return result;
                }
                result.Return = resultUsername.Return;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<int> GetIdByToken(string token)
        {
            var result = new Result<int>();
            try
            {
                var resultUsername = RestCall.Call<int>(baseUrl + "v1/Usuario/GetId?token=" + token, RestSharp.Portable.Method.GET);

                if (!resultUsername.Ok)
                {
                    result.AddErrorPublico(resultUsername.Error);
                    return result;
                }
                result.Return = resultUsername.Return;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<_Resultado_VecinoVirtualUsuario> GetResultadoByIdObligatorio(int id)
        {
            var resultado = new Result<_Resultado_VecinoVirtualUsuario>();
            var resultUsuario = dao.GetByIdObligatorio(id);
            if (!resultUsuario.Ok)
            {
                resultado.Copy(resultUsuario.Errores);
                return resultado;
            }

            resultado.Return = new _Resultado_VecinoVirtualUsuario(resultUsuario.Return);
            if (resultado.Return != null)
            {
                if (resultado.Return.IdentificadorFotoPersonal == null)
                {
                    if (resultado.Return.SexoMasculino)
                    {
                        resultado.Return.IdentificadorFotoPersonal = ConfigurationManager.AppSettings["IDENTIFICADOR_FOTO_USER_MALE"];
                    }
                    else
                    {
                        resultado.Return.IdentificadorFotoPersonal = ConfigurationManager.AppSettings["IDENTIFICADOR_FOTO_USER_FEMALE"];
                    }
                }
            }
            return resultado;
        }

        public Result<bool> EsAplicacionBloqueada()
        {
            return dao.EsAplicacionBloqueada();
        }

        public Result<List<int>> GetIdsAreasByIdUsuario(int idUsuario)
        {
            return dao.GetIdsAreasByIdUsuario(idUsuario);
        }

        public string AppIdentifier
        {
            get { return ConfigurationManager.AppSettings["CERROJO_APP_IDENTIFIER"]; }
        }

    }


}

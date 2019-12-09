using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Model;
using Model.Entities;
using Rules.Rules.Reportes;
using Model.Resultados;
using Intranet_Servicios2;
using Rules.Rules;
using Rules;
using Intranet_Servicios2.MisRules;
using Model.Comandos;
using Intranet_Servicios2.Utils.Entities.Comando;
using System.Configuration;

namespace Intranet_Servicios2.Utils.MisRules
{
    public class _WSRules_BaseUsuario : _WSRules_Base<_VecinoVirtualUsuario>
    {
        private readonly _VecinoVirtualUsuarioRules rules;

        public _WSRules_BaseUsuario(UsuarioLogueado data)
            : base(data)
        {
            rules = new _VecinoVirtualUsuarioRules(data);
        }



        #region Acceder

        public ResultadoServicio<string> IniciarSesion(string user, string pass, string keyVencimiento)
        {
            var resultado = new ResultadoServicio<string>();

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/IniciarSesion";
            var resultadoIniciarSesion = RestCall.CallPut<Model.WSVecinoVirtual.Resultados.VVResultado_Usuario>(url, new Model.WSVecinoVirtual.Comandos.VVComando_IniciarSesion()
            {
                Username = user,
                Password = pass,
                KeyTokenSinVencimiento = keyVencimiento
            });

            if (!resultadoIniciarSesion.Ok)
            {
                resultado.Error = resultadoIniciarSesion.Error;
                return resultado;
            }

            resultado.Return = resultadoIniciarSesion.Return.Token;
            return resultado;
        }

        public ResultadoServicio<int> GetIdUsuario()
        {
            var resultado = new ResultadoServicio<int>();

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/GetId?token=" + getUsuarioLogueado().Token;
            var resultadoId = RestCall.CallGet<int?>(url);
            if (!resultadoId.Ok)
            {
                resultado.Error = resultadoId.Error;
                return resultado;
            }

            resultado.Return = resultadoId.Return.Value;
            return resultado;
        }

        public ResultadoServicio<bool?> AgregarFCMToken(string fcmToken = null)
        {
            var resultado = new ResultadoServicio<bool?>();

            var resultadoToken = new FCMTokenRules(getUsuarioLogueado()).Insertar(fcmToken);
            if (!resultadoToken.Ok)
            {
                resultado.Error = resultadoToken.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = resultadoToken.Return;
            return resultado;
        }

        public ResultadoServicio<bool?> CerrarSesion(string fcmToken = null)
        {
            var resultado = new ResultadoServicio<bool?>();

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/CerrarSesion?token=" + getUsuarioLogueado().Token;
            var resultadoIniciarSesion = RestCall.CallPut<bool?>(url);
            if (!resultadoIniciarSesion.Ok)
            {
                resultado.Error = resultadoIniciarSesion.Error;
                return resultado;
            }


            //Cancelo el FCMToken
            if (!string.IsNullOrEmpty(fcmToken))
            {
                var resultadoToken = new FCMTokenRules(getUsuarioLogueado()).Borrar(fcmToken);
            }

            resultado.Return = true;
            return resultado;
        }

        public ResultadoServicio<bool?> ValidarToken(string token)
        {
            var resultado = new ResultadoServicio<bool?>();

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/ValidarToken?token=" + token;
            var resultadoValidarToken = RestCall.CallGet<bool?>(url);
            if (!resultadoValidarToken.Ok)
            {
                resultado.Return = false;
                return resultado;
            }

            resultado.Return = resultadoValidarToken.Return.Value;
            return resultado;
        }

        public ResultadoServicio<bool?> EsValidadoRenaper()
        {
            var resultado = new ResultadoServicio<bool?>();

            var resultadoValidar = rules.EsValidadoRenaper();
            if (!resultadoValidar.Ok)
            {
                resultado.Error = resultadoValidar.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = resultadoValidar.Return;
            return resultado;
        }

        #endregion

        #region Insertar

        public ResultadoServicio<_VecinoVirtualUsuario> Insertar(ComandoAppBase_UsuarioNuevo comando, bool passwordDefault, string urlRetorno)
        {
            var resultado = new ResultadoServicio<_VecinoVirtualUsuario>();

            var resultadoInsertar = rules.CrearUsuario(new Model.Comandos.Comando_UsuarioVecinoVirtualNuevo()
            {
                Nombre = comando.Nombre,
                Apellido = comando.Apellido,
                Dni = comando.Dni,
                FechaNacimiento = comando.FechaNacimiento,
                SexoMasculino = comando.SexoMasculino,
                IdEstadoCivil = comando.IdEstadoCivil,
                Domicilio = comando.Domicilio == null ? null : new Model.Comandos.Comando_UsuarioVecinoVirtualActualizarDomicilio()
                {
                    Direccion = comando.Domicilio.Direccion,
                    Altura = comando.Domicilio.Altura,
                    Torre = comando.Domicilio.Torre,
                    Piso = comando.Domicilio.Piso,
                    Depto = comando.Domicilio.Depto,
                    Barrio = comando.Domicilio.Barrio,
                    IdBarrio = comando.Domicilio.IdBarrio,
                    Ciudad = comando.Domicilio.Ciudad,
                    IdCiudad = comando.Domicilio.IdCiudad,
                    Provincia = comando.Domicilio.Provincia,
                    IdProvincia = comando.Domicilio.IdProvincia
                },
                Username = comando.Username,
                Password = comando.Password,

                Email = comando.Email,
                TelefonoFijo = comando.TelefonoFijo,
                TelefonoCelular = comando.TelefonoCelular,
                Facebook = comando.Facebook,
                Twitter = comando.Twitter,
                Instagram = comando.Instagram,
                LinkedIn = comando.LinkedIn
            }, passwordDefault, false, false, urlRetorno);

            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Error;
                return resultado;
            }

            resultado.Return = GetById(resultadoInsertar.Return.Id).Return;
            return resultado;
        }


        #endregion

        #region Actualizar

        public ResultadoServicio<_VecinoVirtualUsuario> Actualizar(ComandoAppBase_UsuarioEditar comando)
        {
            var resultado = new ResultadoServicio<_VecinoVirtualUsuario>();

            var comandoInterno = new Comando_UsuarioVecinoVirtualEditar()
            {
                Id = comando.Id,
                Nombre = comando.Nombre,
                Apellido = comando.Apellido,
                Dni = comando.Dni,
                FechaNacimiento = comando.FechaNacimiento,
                SexoMasculino = comando.SexoMasculino,
                IdEstadoCivil = comando.IdEstadoCivil,

                Email = comando.Email,
                TelefonoFijo = comando.TelefonoFijo,
                TelefonoCelular = comando.TelefonoCelular,
                Facebook = comando.Facebook,
                Twitter = comando.Twitter,
                Instagram = comando.Instagram,
                LinkedIn = comando.LinkedIn
            };

            var resultadoInsertar = rules.ActualizarUsuario(comandoInterno);
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = GetById(resultadoInsertar.Return.Id).Return;
            return resultado;
        }

        public ResultadoServicio<string> ActualizarFotoPersonal(ComandoAppBase_UsuarioEditarFotoPersonal comando)
        {
            var resultado = new ResultadoServicio<string>();

            var comandoInterno = new Comando_UsuarioVecinoVirtualEditarFotoPersonal()
            {
                Content = comando.Content
            };

            var resultadoInsertar = rules.ActualizarFotoPersonal(comandoInterno);
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = resultadoInsertar.Return;

            return resultado;
        }

        public ResultadoServicio<_VecinoVirtualUsuario> ActualizarDatosPersonales(ComandoAppBase_UsuarioEditarDatosPersonales comando)
        {
            var resultado = new ResultadoServicio<_VecinoVirtualUsuario>();

            var comandoInterno = new Comando_UsuarioVecinoVirtualEditarDatosPersonales()
            {
                Nombre = comando.Nombre,
                Apellido = comando.Apellido,
                Dni = comando.Dni,
                FechaNacimiento = comando.FechaNacimiento,
                SexoMasculino = comando.SexoMasculino,
            };

            var resultadoInsertar = rules.ActualizarDatosPersonales(comandoInterno);
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = GetById(resultadoInsertar.Return.Id).Return;
            return resultado;
        }

        public ResultadoServicio<_VecinoVirtualUsuario> ActualizarDatosContacto(ComandoAppBase_UsuarioEditarDatosContacto comando)
        {
            var resultado = new ResultadoServicio<_VecinoVirtualUsuario>();

            var comandoInterno = new Comando_UsuarioVecinoVirtualEditarDatosContacto()
            {
                TelefonoCelular = comando.TelefonoCelular,
                TelefonoFijo = comando.TelefonoFijo,
                Email = comando.Email,
                Facebook = comando.Facebook,
                Twitter = comando.Twitter,
                Instagram = comando.Instagram,
                LinkedIn = comando.LinkedIn
            };

            var resultadoInsertar = rules.ActualizarDatosContacto(comandoInterno);
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = GetById(resultadoInsertar.Return.Id).Return;
            return resultado;
        }

        public ResultadoServicio<_VecinoVirtualUsuario> ActualizarDomicilio(ComandoAppBase_UsuarioDomicilio comando)
        {
            var resultado = new ResultadoServicio<_VecinoVirtualUsuario>();

            var comandoInterno = new Comando_UsuarioVecinoVirtualActualizarDomicilio()
            {
                Direccion = comando.Direccion,
                CodigoPostal = comando.CodigoPostal,
                Altura = comando.Altura,
                Torre = comando.Torre,
                Piso = comando.Piso,
                Depto = comando.Depto,
                Barrio = comando.Barrio,
                IdBarrio = comando.IdBarrio,
                Ciudad = comando.Ciudad,
                IdCiudad = comando.IdCiudad,
                Provincia = comando.Provincia,
                IdProvincia = comando.IdProvincia
            };

            var resultadoInsertar = rules.ActualizarDomicilio(comandoInterno);
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = GetById(resultadoInsertar.Return.Id).Return;
            return resultado;
        }

        public ResultadoServicio<_VecinoVirtualUsuario> ActualizarEstadoCivil(int idEstadoCivil)
        {
            var resultado = new ResultadoServicio<_VecinoVirtualUsuario>();
            var resultadoInsertar = rules.ActualizarEstadoCivil(idEstadoCivil);
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = GetById(resultadoInsertar.Return.Id).Return;
            return resultado;
        }

        public ResultadoServicio<bool?> CambiarUsername(string username)
        {
            var resultado = new ResultadoServicio<bool?>();

            if (getUsuarioLogueado() == null || getUsuarioLogueado().Usuario == null)
            {
                resultado.Error = "Debe iniciar sesion";
                return resultado;
            }

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/CambiarUsername?token=" + getUsuarioLogueado().Token + "&username=" + username;
            var resultadoIniciarSesion = RestCall.CallPut<bool?>(url);
            if (!resultadoIniciarSesion.Ok)
            {
                resultado.Error = resultadoIniciarSesion.Error;
                return resultado;
            }

            resultado.Return = resultadoIniciarSesion.Return;
            return resultado;
        }

        public ResultadoServicio<bool?> CambiarPassword(string passwordAnterior, string passwordNueva)
        {
            var resultado = new ResultadoServicio<bool?>();

            if (getUsuarioLogueado() == null || getUsuarioLogueado().Usuario == null)
            {
                resultado.Error = "Debe iniciar sesion";
                return resultado;
            }

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/CambiarPassword?token=" + getUsuarioLogueado().Token;
            var resultadoCambiarPassword = RestCall.CallPut<bool?>(url, new Model.WSVecinoVirtual.Comandos.VVComando_CambiarPassword()
            {
                PasswordAnterior = passwordAnterior,
                PasswordNueva = passwordNueva
            });
            if (!resultadoCambiarPassword.Ok)
            {
                resultado.Error = resultadoCambiarPassword.Error;
                return resultado;
            }

            resultado.Return = resultadoCambiarPassword.Return;
            return resultado;
        }

        #endregion

        #region Recuperar Cuenta

        public ResultadoServicio<bool?> IniciarRecuperacionCuenta(string username, string urlServidor, string urlRetorno)
        {
            var resultado = new ResultadoServicio<bool?>();

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/RecuperacionCuenta/Iniciar";
            var resultadoIniciarSesion = RestCall.CallPut<bool?>(url, new Model.WSVecinoVirtual.Comandos.VVComando_IniciarRecuperacionCuenta()
            {
                Username = username,
                UrlServidor = urlServidor,
                UrlRetorno = urlRetorno
            });
            if (!resultadoIniciarSesion.Ok)
            {
                resultado.Error = resultadoIniciarSesion.Error;
                return resultado;
            }

            resultado.Return = resultadoIniciarSesion.Return.Value;
            return resultado;
        }

        #endregion

        #region Activar Cuenta

        public ResultadoServicio<bool?> ValidarUsuarioActivadoByUserPass(string user, string pass)
        {
            var resultado = new ResultadoServicio<bool?>();

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/ActivacionCuenta/Validar";
            var resultadoIniciarSesion = RestCall.CallPut<bool?>(url, new Model.WSVecinoVirtual.Comandos.VVComando_ValidarUsuarioActivadorByUserPass()
            {
                Username = user,
                Password = pass
            });
            if (!resultadoIniciarSesion.Ok)
            {
                resultado.Error = resultadoIniciarSesion.Error;
                return resultado;
            }

            resultado.Return = resultadoIniciarSesion.Return.Value;
            return resultado;
        }

        public ResultadoServicio<string> IniciarActivacionUsuario(ComandoAppBase_UsuarioIniciarActivacion comando)
        {
            var resultado = new ResultadoServicio<string>();

            var url = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"] + "/v1/Usuario/ActivacionCuenta/Iniciar";
            var resultadoIniciarSesion = RestCall.CallPut<string>(url, new Model.WSVecinoVirtual.Comandos.VVComando_IniciarActivacionCuenta()
            {
                Username = comando.Username,
                Password = comando.Password,
                UrlServidor = ConfigurationManager.AppSettings["URL_ACTIVAR_USUARIO"],
                UrlRetorno = comando.UrlRetorno
            });


            if (!resultadoIniciarSesion.Ok)
            {
                resultado.Error = resultadoIniciarSesion.Error;
                return resultado;
            }

            resultado.Return = resultadoIniciarSesion.Return;
            return resultado;
        }

        #endregion

        #region Utils

        public ResultadoServicio<_VecinoVirtualUsuario> GetById(int id)
        {
            var resultado = new ResultadoServicio<_VecinoVirtualUsuario>();
            var resultadoConsulta = new _VecinoVirtualUsuarioRules(null).GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Error = resultadoConsulta.ToStringPublico();
                return resultado;
            }

            if (resultadoConsulta.Return == null)
            {
                resultado.Error = "El usuario indicado no existe";
                return resultado;
            }

            resultado.Return = resultadoConsulta.Return;
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


        #endregion
    }
}

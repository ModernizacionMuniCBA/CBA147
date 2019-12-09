using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Model;
using Model.Entities;
using Rules.Rules;
using Rules;
using Intranet_Servicios2.MisRules;
using Intranet_Servicios2.Utils.MisRules;
using Intranet_Servicios2.Utils.Entities.Comando;
using System.Configuration;
using Intranet_Servicios2.v1.Entities.Comandos;

namespace Intranet_Servicios2.v1.MisRules
{
    public class WSRules_Usuario : _WSRules_Base<_VecinoVirtualUsuario>
    {

        private readonly _VecinoVirtualUsuarioRules rules;
        private readonly _WSRules_BaseUsuario rulesBase;

        public WSRules_Usuario(UsuarioLogueado data)
            : base(data)
        {
            rules = new _VecinoVirtualUsuarioRules(data);
            rulesBase = new _WSRules_BaseUsuario(data);
        }


        #region Acceder

        public ResultadoServicio<string> IniciarSesion(string user, string pass, string keyVencimiento)
        {
            //Devuelvo lo mismo que el base
            return rulesBase.IniciarSesion(user, pass, keyVencimiento);
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> GetDatosUsuario()
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();

            //Llamo al base
            var resultadoId = rulesBase.GetIdUsuario();
            if (!resultadoId.Ok)
            {
                resultado.Error = resultadoId.Error;
                return resultado;
            }

            //Devuelvo todos los datos del usuario
            return GetById(resultadoId.Return);
        }

        public ResultadoServicio<bool?> AgregarFCMToken(string fcmToken = null)
        {
            return rulesBase.AgregarFCMToken(fcmToken);
        }

        public ResultadoServicio<bool?> CerrarSesion(string fcmToken = null)
        {
            return rulesBase.CerrarSesion(fcmToken);
        }

        public ResultadoServicio<bool?> ValidarToken(string token)
        {
            return rulesBase.ValidarToken(token);
        }

        public ResultadoServicio<bool?> EsValidadoRenaper()
        {
            return rulesBase.EsValidadoRenaper();
        }

        #endregion

        #region Insertar

        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> Insertar(v1.Entities.Comandos.ComandoApp_UsuarioNuevo comando, bool passwordDefault, string urlRetorno)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();

            var comandoInterno = new ComandoAppBase_UsuarioNuevo()
            {
                Nombre = comando.Nombre,
                Apellido = comando.Apellido,
                Dni = comando.Dni,
                FechaNacimiento = comando.FechaNacimiento,
                SexoMasculino = comando.SexoMasculino,
                IdEstadoCivil = comando.IdEstadoCivil,
                Domicilio = comando.Domicilio == null ? null : new ComandoAppBase_UsuarioDomicilio()
                {
                    Direccion = comando.Domicilio.Direccion,
                    Altura = comando.Domicilio.Altura,
                    Torre = comando.Domicilio.Torre,
                    Piso = comando.Domicilio.Piso,
                    Depto = comando.Domicilio.Depto,
                    CodigoPostal = comando.Domicilio.CodigoPostal,
                    IdBarrio = comando.Domicilio.IdBarrio,
                    Barrio = comando.Domicilio.Barrio,
                    IdCiudad = comando.Domicilio.IdCiudad,
                    Ciudad = comando.Domicilio.Ciudad,
                    IdProvincia = comando.Domicilio.IdProvincia,
                    Provincia = comando.Domicilio.Provincia
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
            };

            var resultadoInsertar = rulesBase.Insertar(comandoInterno, passwordDefault, urlRetorno);
            if (!resultadoInsertar.Ok)
            {
                resultado.Error = resultadoInsertar.Error;
                return resultado;
            }

            resultado.Return = new Entities.Resultados.ResultadoApp_Usuario(resultadoInsertar.Return);
            return resultado;
        }

        #endregion

        #region Actualizar

        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> Actualizar(v1.Entities.Comandos.ComandoApp_UsuarioEditar comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();

            var comandoInterno = new ComandoAppBase_UsuarioEditar()
            {
                Id = getUsuarioLogueado().Usuario.Id,
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
                Instagram = comando.Instagram,
                Twitter = comando.Twitter,
                LinkedIn = comando.LinkedIn
            };

            var resultadoActualizar = rulesBase.Actualizar(comandoInterno);
            if (!resultadoActualizar.Ok)
            {
                resultado.Error = resultadoActualizar.Error;
                return resultado;
            }

            resultado.Return = new Entities.Resultados.ResultadoApp_Usuario(resultadoActualizar.Return);
            return resultado;
        }

        public ResultadoServicio<string> ActualizarFotoPersonal(v1.Entities.Comandos.ComandoApp_UsuarioEditarFotoPersonal comando)
        {
            var comandoInterno = new ComandoAppBase_UsuarioEditarFotoPersonal()
            {
                Content = comando.Content,
            };

            return rulesBase.ActualizarFotoPersonal(comandoInterno);
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> ActualizarDatosPersonales(v1.Entities.Comandos.ComandoApp_UsuarioEditarDatosPersonales comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();

            var comandoInterno = new ComandoAppBase_UsuarioEditarDatosPersonales()
            {
                Nombre = comando.Nombre,
                Apellido = comando.Apellido,
                Dni = comando.Dni,
                FechaNacimiento = comando.FechaNacimiento,
                SexoMasculino = comando.SexoMasculino
            };

            var resultadoActualizar = rulesBase.ActualizarDatosPersonales(comandoInterno);
            if (!resultadoActualizar.Ok)
            {
                resultado.Error = resultadoActualizar.Error;
                return resultado;
            }

            resultado.Return = new Entities.Resultados.ResultadoApp_Usuario(resultadoActualizar.Return);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> ActualizarDatosContacto(v1.Entities.Comandos.ComandoApp_UsuarioEditarDatosContacto comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();

            var comandoInterno = new ComandoAppBase_UsuarioEditarDatosContacto()
            {
                Email = comando.Email,
                TelefonoFijo = comando.TelefonoFijo,
                TelefonoCelular = comando.TelefonoCelular,
                Facebook = comando.Facebook,
                Twitter = comando.Twitter,
                Instagram = comando.Instagram,
                LinkedIn = comando.LinkedIn
            };

            var resultadoActualizar = rulesBase.ActualizarDatosContacto(comandoInterno);
            if (!resultadoActualizar.Ok)
            {
                resultado.Error = resultadoActualizar.Error;
                return resultado;
            }

            resultado.Return = new Entities.Resultados.ResultadoApp_Usuario(resultadoActualizar.Return);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> ActualizarDomicilio(v1.Entities.Comandos.ComandoApp_UsuarioDomicilio comando)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();

            var comandoInterno = new ComandoAppBase_UsuarioDomicilio()
            {
                Direccion = comando.Direccion,
                Altura = comando.Altura,
                Torre = comando.Torre,
                Piso = comando.Piso,
                Depto = comando.Depto,
                CodigoPostal = comando.CodigoPostal,
                IdBarrio = comando.IdBarrio,
                Barrio = comando.Barrio,
                IdCiudad = comando.IdCiudad,
                Ciudad = comando.Ciudad,
                IdProvincia = comando.IdProvincia,
                Provincia = comando.Provincia
            };

            var resultadoActualizar = rulesBase.ActualizarDomicilio(comandoInterno);
            if (!resultadoActualizar.Ok)
            {
                resultado.Error = resultadoActualizar.Error;
                return resultado;
            }

            resultado.Return = new Entities.Resultados.ResultadoApp_Usuario(resultadoActualizar.Return);
            return resultado;
        }

        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> ActualizarEstadoCivil(int idEstadoCivil)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();

            var resultadoActualizar = rulesBase.ActualizarEstadoCivil(idEstadoCivil);
            if (!resultadoActualizar.Ok)
            {
                resultado.Error = resultadoActualizar.Error;
                return resultado;
            }

            resultado.Return = new Entities.Resultados.ResultadoApp_Usuario(resultadoActualizar.Return);
            return resultado;
        }

        public ResultadoServicio<bool?> CambiarUsername(string username)
        {
            return rulesBase.CambiarUsername(username);
        }
      
        public ResultadoServicio<bool?> CambiarPassword(string passwordAnterior, string passwordNueva)
        {
            return rulesBase.CambiarPassword(passwordAnterior, passwordNueva);
        }

        #endregion

        #region Recuperar cuenta

        public ResultadoServicio<bool?> IniciarRecuperacionCuenta(string username, string urlRetorno)
        {
            string urlServidor = ConfigurationManager.AppSettings["URL_RECUPERAR_PASSWORD"];
            return rulesBase.IniciarRecuperacionCuenta(username, urlServidor, urlRetorno);
        }

        #endregion

        #region Activar Usuario

        public ResultadoServicio<bool?> ValidarUsuarioActivadoByUserPass(string user, string pass)
        {
            return rulesBase.ValidarUsuarioActivadoByUserPass(user, pass);
        }

        public ResultadoServicio<string> IniciarActivacionUsuario(ComandoApp_UsuarioIniciarActivacion comando)
        {
            return rulesBase.IniciarActivacionUsuario(new ComandoAppBase_UsuarioIniciarActivacion()
            {
                Username = comando.Username,
                Password = comando.Password,
                UrlRetorno = comando.UrlRetorno
            });
        }

        #endregion

        #region Utils

        private ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> GetById(int id)
        {
            var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();

            //Invoco al base
            var resultadoConsulta = rulesBase.GetById(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Error = resultadoConsulta.Error;
                return resultado;
            }

            //Transformo segun version actual
            resultado.Return = new v1.Entities.Resultados.ResultadoApp_Usuario(resultadoConsulta.Return);
            return resultado;
        }


        #endregion
    }
}

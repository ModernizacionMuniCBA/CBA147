using Intranet_Servicios2.Utils.Controllers;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Intranet_Servicios2.v1.Controllers
{
    /// <summary>
    /// Controlador para Usuarios
    /// </summary>
    public class Usuario_v1Controller : Control
    {
        const string routeBase = "v1/Usuario";

        /// <summary>
        /// Crea un usuario. Lo valida con RENAPER.
        /// </summary>
        /// <param name="comando">Información del usuario</param>
        /// <param name="passwordDefault">Si desea usar una password autogenerada</param>
        /// <param name="urlServidor">Url que se encargara de la activacion del usuario</param>
        /// <param name="urlRetorno">Url a la que se debe retornar al terminar la activacion del usuario</param>
        /// <returns></returns>
        [HttpPost]
        [Route(routeBase)]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> Insertar(v1.Entities.Comandos.ComandoApp_UsuarioNuevo comando, bool passwordDefault, string urlRetorno)
        {
            return new v1.MisRules.WSRules_Usuario(null).Insertar(comando, passwordDefault, urlRetorno);
        }

        /// <summary>
        /// Actualiza los datos del usuario logeado. Si el usuario no esta validado por RENAPER lo valida con los datos enviados en el comando. Si si se encuentra validado ignora los campos de datos personales y solo edita los datos de contacto.
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="comando">Datos del usuario</param>
        /// <returns></returns>
        [HttpPut]
        [Route(routeBase)]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> Actualizar(string token, v1.Entities.Comandos.ComandoApp_UsuarioEditar comando)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).Actualizar(comando);
        }

        /// <summary>
        /// Actualiza la foto personal del usuario
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="comando">Datos personales del usuario</param>
        /// <returns></returns>
        [HttpPut]
        [Route(routeBase + "/ActualizarFotoPersonal")]
        public ResultadoServicio<string> ActualizarFotoPersonal(string token, v1.Entities.Comandos.ComandoApp_UsuarioEditarFotoPersonal comando)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<string>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).ActualizarFotoPersonal(comando);
        }

        /// <summary>
        /// Actualiza los datos personales del usuario. Solo si no esta validado por RENAPER
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="comando">Datos personales del usuario</param>
        /// <returns></returns>
        [HttpPut]
        [Route(routeBase + "/ActualizarDatosPersonales")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> ActualizarDatosPersonales(string token, v1.Entities.Comandos.ComandoApp_UsuarioEditarDatosPersonales comando)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).ActualizarDatosPersonales(comando);
        }

        /// <summary>
        /// Actualiza los datos del contacto del usuario
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="comando">Datos de contacto del usuario</param>
        /// <returns></returns>
        [HttpPut]
        [Route(routeBase + "/ActualizarDatosContacto")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> ActualizarDatosContacto(string token, v1.Entities.Comandos.ComandoApp_UsuarioEditarDatosContacto comando)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).ActualizarDatosContacto(comando);
        }

        [HttpPut]
        [Route(routeBase + "/ActualizarDomicilio")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> ActualizarDomicilio(string token, v1.Entities.Comandos.ComandoApp_UsuarioDomicilio comando)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).ActualizarDomicilio(comando);
        }

        [HttpPut]
        [Route(routeBase + "/ActualizarEstadoCivil")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> ActualizarEstadoCivil(string token, int idEstadoCivil)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).ActualizarEstadoCivil(idEstadoCivil);
        }

        /// <summary>
        /// Verifica si es validado por ReNaPer
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <returns></returns>
        [HttpGet]
        [Route(routeBase + "/EsValidadoRenaper")]
        public ResultadoServicio<bool?> EsValidadoRenaper(string token)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool?>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).EsValidadoRenaper();
        }

        /// <summary>
        /// Devuelve los datos los usuario logeado
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <returns></returns>
        [HttpGet]
        [Route(routeBase)]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario> GetDatosUsuario(string token)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Usuario>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).GetDatosUsuario();
        }


        /// <summary>
        /// Genera un token de acceso para el usuario con las credenciales enviadas
        /// </summary>
        /// <param name="username">Username o CUIL</param>
        /// <param name="password">Password</param>
        /// <param name="keyVencimiento">Key privada, que de ser la correcta genera un token sin vencimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route(routeBase + "/IniciarSesion")]
        public ResultadoServicio<string> IniciarSesion(string username, string password, string keyVencimiento = null)
        {
            return new v1.MisRules.WSRules_Usuario(null).IniciarSesion(username, password, keyVencimiento);
        }

        /// <summary>
        /// Valida si el token siguen siendo valido
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <returns></returns>
        [HttpGet]
        [Route(routeBase + "/ValidarToken")]
        public ResultadoServicio<bool?> ValidarToken(string token)
        {
            return new v1.MisRules.WSRules_Usuario(null).ValidarToken(token);
        }

        /// <summary>
        /// Registra el FCM Token enviado al usuario logeado para que pueda recibir notificaciones.
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="fcmToken">Token de Firebase Cloud Messaging</param>
        /// <returns></returns>
        [HttpGet]
        [Route(routeBase + "/AgregarFCMToken")]
        public ResultadoServicio<bool?> AgregarFCMToken(string token, string fcmToken)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool?>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }


            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).AgregarFCMToken(fcmToken);
        }

        /// <summary>
        /// Cierra sesion. Lo que ocasiona que el token no sirva mas. Ademas si se envia un FMC token, le da de baja para que deje de recibir notificaciones.
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="fcmToken">Token de Firebase Cloud Messaging</param>
        /// <returns></returns>
        [HttpGet]
        [Route(routeBase + "/CerrarSesion")]
        public ResultadoServicio<bool?> CerrarSesion(string token, string fcmToken = null)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool?>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).CerrarSesion(fcmToken);
        }

        /// <summary>
        /// Cambia o define un username para el usuario logeado
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="username">Username nuevo</param>
        /// <returns></returns>
        [HttpPut]
        [Route(routeBase + "/CambiarUsername")]
        public ResultadoServicio<bool?> CambiarUsername(string token, string username)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool?>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).CambiarUsername(username);
        }

        /// <summary>
        /// Cambia la password del usuario logeado
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="passwordAnterior">Password anterior</param>
        /// <param name="passwordNuevo">Password nueva</param>
        /// <returns></returns>
        [HttpPut]
        [Route(routeBase + "/CambiarPassword")]
        public ResultadoServicio<bool?> CambiarPassword(string token, string passwordAnterior, string passwordNuevo)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool?>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Usuario(resultadoUser.Return).CambiarPassword(passwordAnterior, passwordNuevo);
        }

        /// <summary>
        /// Envia un e-mail (a la casilla registrada en el usuario) para recuperar la contraseña del usuario
        /// </summary>
        /// <param name="username">Username o CUIL</param>
        /// <param name="urlRetorno">Password</param>
        /// <returns></returns>
        [HttpPut]
        [Route(routeBase + "/RecuperarCuenta")]
        public ResultadoServicio<bool?> IniciarRecuperacionCuenta(string username, string urlRetorno)
        {
            return new v1.MisRules.WSRules_Usuario(null).IniciarRecuperacionCuenta(username, urlRetorno);
        }

        /// <summary>
        /// Envia un e-mail (a la casilla registrada en el usuario) para activar el usuario
        /// </summary>
        /// <param name="username">Username o CUIL</param>
        /// <param name="urlRetorno">Password</param>
        /// <returns></returns>
        [HttpPut]
        [Route(routeBase + "/IniciarActivacionUsuario")]
        public ResultadoServicio<string> IniciarActivacionUsuario(v1.Entities.Comandos.ComandoApp_UsuarioIniciarActivacion comando)
        {
            return new v1.MisRules.WSRules_Usuario(null).IniciarActivacionUsuario(comando);
        }

        /// <summary>
        /// Valida si el usuario se encuentra activado por E-Mail
        /// </summary>
        /// <param name="username">Username o CUIL</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        [HttpGet]
        [Route(routeBase + "/ValidarUsuarioActivado")]
        public ResultadoServicio<bool?> ValidarUsuarioActivadoByUserPass(string username, string password)
        {
            return new v1.MisRules.WSRules_Usuario(null).ValidarUsuarioActivadoByUserPass(username, password);
        }
    }
}
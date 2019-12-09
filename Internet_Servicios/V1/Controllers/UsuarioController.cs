using Internet_Servicios.Utils;
using Internet_Servicios.V1.Entities.Comandos;
using Internet_Servicios.V1.Entities.Resultados;
using System;
using System.Linq;
using System.Web.Http;

namespace Internet_Servicios.V1.Controllers
{
    /// <summary>
    /// Controlador para Usuarios
    /// </summary>
    [RoutePrefix("v1/Usuario")]
    public class Usuario_v1Controller : ApiController
    {
        /// <summary>
        /// Crea un usuario. Lo valida con RENAPER.
        /// </summary>
        /// <param name="comando">Información del usuario</param>
        /// <param name="passwordDefault">Si desea usar una password autogenerada</param>
        /// <param name="urlServidor">Url que se encargara de la activacion del usuario</param>
        /// <param name="urlRetorno">Url a la que se debe retornar al terminar la activacion del usuario</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public ResultServicio<ResultadoApp_Usuario> Insertar(ComandoApp_UsuarioNuevo comando, bool passwordDefault, string urlRetorno)
        {
            return RestCall.Call<ResultadoApp_Usuario>(Request, comando);
        }

        /// <summary>
        /// Actualiza los datos del usuario logeado. Si el usuario no esta validado por RENAPER lo valida con los datos enviados en el comando. Si si se encuentra validado ignora los campos de datos personales y solo edita los datos de contacto.
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="comando">Datos del usuario</param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public ResultServicio<ResultadoApp_Usuario> Actualizar(string token, ComandoApp_UsuarioEditar comando)
        {
            return RestCall.Call<ResultadoApp_Usuario>(Request, comando);
        }

        /// <summary>
        /// Actualiza la foto personal del usuario
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="comando">Datos personales del usuario</param>
        /// <returns></returns>
        [HttpPut]
        [Route("ActualizarFotoPersonal")]
        public ResultServicio<string> ActualizarFotoPersonal(string token, ComandoApp_UsuarioEditarFotoPersonal comando)
        {
            return RestCall.Call<string>(Request, comando);
        }

        /// <summary>
        /// Actualiza los datos personales del usuario. Solo si no esta validado por RENAPER
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="comando">Datos personales del usuario</param>
        /// <returns></returns>
        [HttpPut]
        [Route("ActualizarDatosPersonales")]
        public ResultServicio<ResultadoApp_Usuario> ActualizarDatosPersonales(string token, ComandoApp_UsuarioEditarDatosPersonales comando)
        {
            return RestCall.Call<ResultadoApp_Usuario>(Request, comando);
        }

        /// <summary>
        /// Actualiza los datos del contacto del usuario
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="comando">Datos de contacto del usuario</param>
        /// <returns></returns>
        [HttpPut]
        [Route("ActualizarDatosContacto")]
        public ResultServicio<ResultadoApp_Usuario> ActualizarDatosContacto(string token, ComandoApp_UsuarioEditarDatosContacto comando)
        {
            return RestCall.Call<ResultadoApp_Usuario>(Request, comando);
        }

        [HttpPut]
        [Route("ActualizarDomicilio")]
        public ResultServicio<ResultadoApp_Usuario> ActualizarDomicilio(string token, ComandoApp_UsuarioDomicilio comando)
        {
            return RestCall.Call<ResultadoApp_Usuario>(Request, comando);
        }

        [HttpPut]
        [Route("ActualizarEstadoCivil")]
        public ResultServicio<ResultadoApp_Usuario> ActualizarEstadoCivil(string token, int idEstadoCivil)
        {
            return RestCall.Call<ResultadoApp_Usuario>(Request);
        }

        /// <summary>
        /// Verifica si es validado por ReNaPer
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <returns></returns>
        [HttpGet]
        [Route("EsValidadoRenaper")]
        public ResultServicio<bool?> EsValidadoRenaper(string token)
        {
            return RestCall.Call<bool?>(Request);
        }

        /// <summary>
        /// Devuelve los datos los usuario logeado
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public ResultServicio<ResultadoApp_Usuario> GetDatosUsuario(string token)
        {
            return RestCall.Call<ResultadoApp_Usuario>(Request);
        }

        /// <summary>
        /// Genera un token de acceso para el usuario con las credenciales enviadas
        /// </summary>
        /// <param name="username">Username o CUIL</param>
        /// <param name="password">Password</param>
        /// <param name="keyVencimiento">Key privada, que de ser la correcta genera un token sin vencimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IniciarSesion")]
        public ResultServicio<string> IniciarSesion(string username, string password, string keyVencimiento = null)
        {
            return RestCall.Call<string>(Request);
        }

        /// <summary>
        /// Valida si el token siguen siendo valido
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ValidarToken")]
        public ResultServicio<bool?> ValidarToken(string token)
        {
            return RestCall.Call<bool?>(Request);
        }

        /// <summary>
        /// Registra el FCM Token enviado al usuario logeado para que pueda recibir notificaciones.
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="fcmToken">Token de Firebase Cloud Messaging</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AgregarFCMToken")]
        public ResultServicio<bool?> AgregarFCMToken(string token, string fcmToken)
        {
            return RestCall.Call<bool?>(Request);
        }

        /// <summary>
        /// Cierra sesion. Lo que ocasiona que el token no sirva mas. Ademas si se envia un FMC token, le da de baja para que deje de recibir notificaciones.
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="fcmToken">Token de Firebase Cloud Messaging</param>
        /// <returns></returns>
        [HttpGet]
        [Route("CerrarSesion")]
        public ResultServicio<bool?> CerrarSesion(string token, string fcmToken = null)
        {
            return RestCall.Call<bool?>(Request);
        }

        /// <summary>
        /// Cambia o define un username para el usuario logeado
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="username">Username nuevo</param>
        /// <returns></returns>
        [HttpPut]
        [Route("CambiarUsername")]
        public ResultServicio<bool?> CambiarUsername(string token, string username)
        {
            return RestCall.Call<bool?>(Request);
        }

        /// <summary>
        /// Cambia la password del usuario logeado
        /// </summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <param name="passwordAnterior">Password anterior</param>
        /// <param name="passwordNuevo">Password nueva</param>
        /// <returns></returns>
        [HttpPut]
        [Route("CambiarPassword")]
        public ResultServicio<bool?> CambiarPassword(string token, string passwordAnterior, string passwordNuevo)
        {
            return RestCall.Call<bool?>(Request);
        }

        /// <summary>
        /// Envia un e-mail (a la casilla registrada en el usuario) para recuperar la contraseña del usuario
        /// </summary>
        /// <param name="username">Username o CUIL</param>
        /// <param name="urlRetorno">Password</param>
        /// <returns></returns>
        [HttpPut]
        [Route("RecuperarCuenta")]
        public ResultServicio<bool?> IniciarRecuperacionCuenta(string username, string urlRetorno)
        {
            return RestCall.Call<bool?>(Request);
        }

        /// <summary>
        /// Envia un e-mail a la casilla del usuario para que el active su usuario
        /// </summary>
        /// <param name="comando"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("IniciarActivacionUsuario")]
        public ResultServicio<string> IniciarActivacionUsuario(ComandoApp_UsuarioIniciarActivacion comando)
        {
            return RestCall.Call<string>(Request, comando);
        }

        /// <summary>
        /// Valida si el usuario se encuentra activado por E-Mail
        /// </summary>
        /// <param name="username">Username o CUIL</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ValidarUsuarioActivado")]
        public ResultServicio<bool?> ValidarUsuarioActivadoByUserPass(string username, string password)
        {
            return RestCall.Call<bool?>(Request);
        }
    }
}
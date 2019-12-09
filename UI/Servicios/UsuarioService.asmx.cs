using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model.Entities;
using Rules.Rules;
using Rules;
using UI.Resources;
using Model.Consultas;
using Model.Resultados;
using Model;
using Intranet_UI.Utils;
using Model.Comandos;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class UsuarioService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public bool IsLogin()
        {
            try
            {
                ValidarSesion(Session);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [WebMethod(EnableSession = true)]
        public Result<List<_Resultado_VecinoVirtualUsuario>> GetByFilters(Consulta_VecinoVirtualUsuario filtros)
        {
            ValidarSesion(Session);
            return new _VecinoVirtualUsuarioRules(GetUsuarioLogeado()).GetResultadoByFilters(filtros);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> SetOrigen(int id)
        {
            ValidarSesion(Session);
            return SessionKey.SetOrigen(Session, id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InitData> GetInitData()
        {
            ValidarSesion(Session);
            return new _VecinoVirtualUsuarioRules(GetUsuarioLogeado()).GetInitData();
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> SetToken(string token)
        {
            var resultado = new Result<bool>();

            var resultadoLogin = SessionKey.IniciarSesionEmpleado(Session, token);
            if (!resultadoLogin.Ok)
            {
                resultado.Copy(resultadoLogin.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public Result<UsuarioLogueado> ActualizarDatosCerrojo()
        {

            ValidarSesion(Session);
            var idOrigen = SessionKey.getOrigen(Session);
            var resultado = SessionKey.IniciarSesionEmpleado(Session, GetUsuarioLogeado().Token);
            if (!resultado.Ok)
            {
                return resultado;
            }

            if (idOrigen.HasValue)
            {
                SessionKey.SetOrigen(Session, idOrigen.Value);
            }
            resultado.Return.IdOrigenElegido = idOrigen;
            return resultado;
        }

        [WebMethod]
        public Result<bool> EsAplicacionBloqueada()
        {
            return new _VecinoVirtualUsuarioRules(null).EsAplicacionBloqueada();
        }


        [WebMethod(EnableSession = true)]
        public Result<_Resultado_VecinoVirtualUsuario> CrearUsuario(Comando_UsuarioVecinoVirtualNuevo usuario)
        {
            ValidarSesion(Session);
            return new _VecinoVirtualUsuarioRules(null).CrearUsuario(usuario, true, true, false,null);
        }

        [WebMethod(EnableSession = true)]
        public Result<_Resultado_VecinoVirtualUsuario> CrearUsuarioEmpleado(Comando_UsuarioVecinoVirtualNuevo usuario)
        {
            ValidarSesion(Session);
            return new _VecinoVirtualUsuarioRules(null).CrearUsuario(usuario, true, true, true, null);
        }

        //[WebMethod(EnableSession = true)]
        //public Result<UsuarioLogueado> ActualizarUsuario(Comando_UsuarioCerrojo usuario)
        //{
        //    ValidarSesion(Session);
        //    var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
        //    var resultado = new Result<UsuarioLogueado>();

        //    var resultActualizar = new _CerrojoUsuarioRules(usuarioLogeado).ActualizarUsuario(usuario, false);
        //    if (!resultActualizar.Ok)
        //    {
        //        resultado.AddErrorPublico("Error al actualizar el perfil");
        //        return resultado;
        //    }

        //    return SessionKey.login(Session, GetUsuarioLogeado().Token);
        //}

        [WebMethod(EnableSession = true)]
        public Result<_Resultado_VecinoVirtualUsuario> ActualizarUsuario(Comando_UsuarioVecinoVirtualEditar usuario)
        {
            ValidarSesion(Session);
            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new _VecinoVirtualUsuarioRules(usuarioLogeado).ActualizarUsuario(usuario);
        }

        [WebMethod(EnableSession = true)]
        public Result<string> CambiarFoto(int id, string content)
        {
            ValidarSesion(Session);
            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new _VecinoVirtualUsuarioRules(usuarioLogeado).CambiarFoto(id, content);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarPassword(string passwordAnterior, string passwordNueva)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new _VecinoVirtualUsuarioRules(userLogueado).CambiarPassword(passwordAnterior, passwordNueva);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarUsername(string username)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new _VecinoVirtualUsuarioRules(userLogueado).CambiarUsername(username);
        }
    }
}

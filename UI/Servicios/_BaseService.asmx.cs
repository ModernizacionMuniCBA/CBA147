using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using Model.Entities;
using Rules;
using UI.Resources;
using Rules.Rules;

namespace UI.Servicios
{
    /// <summary>
    /// Descripción breve de BaseService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public abstract class _BaseService : System.Web.Services.WebService
    {

        public void ValidarSesion(HttpSessionState session)
        {
            if (session == null)
            {
                throw new Exception("Debe iniciar sesion");
            }

            if (!SessionKey.IsLogin(session))
            {
                  throw new Exception("Debe iniciar sesion");
            }

            ////Token
            //var token = Session[SessionKey.Token] as string;
            //if (string.IsNullOrEmpty(token))
            //{
            //    throw new Exception("Debe iniciar sesion");
            //}
            //SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Token = token;

            //Usuario
            //var usuario = Session[SessionKey.UserLogeado] as Usuario;
            //if (usuario == null)
            //{
            //    throw new Exception("Debe iniciar sesion");
            //}

            //SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Usuario = usuario;

            //Rol
            //var rol = Session[SessionKey.Rol] as UsuarioRol;
            //if (rol == null)
            //{
            //    throw new Exception("Debe iniciar sesion");
            //}
            //SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Rol = rol;

            //Menu
            //var menu = Session[SessionKey.Menu] as Rules.Cerrojo1.CerrojoMenu[];
            //if (menu == null)
            //{
            //    throw new Exception("Debe iniciar sesion");
            //}
            //SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Menu = menu;
        }

        public UsuarioLogueado GetUsuarioLogeado()
        {
            return SessionKey.getUsuarioLogueado(Session);
        }
    }
}

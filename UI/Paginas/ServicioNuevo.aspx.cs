using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Rules.Rules;
using Rules;
using UI.Resources;
using System.Web;

namespace UI
{
    public partial class ServicioNuevo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            //new DomicilioRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).CambiarDomicilios();
            
            //var resultadoAmbitos = new UsuarioRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).ActualizarAmbitoDeTodosLosUsuarios();

            //if (resultadoAmbitos.Return)
            //{
            //    //Usuario
            //    InitUsuario(Request.RawUrl);
            //}
        }

        private void InitUsuario(string rawUrl)
        {
            //var rol = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Rol;

            ////Permiso ejecutar
            //if (!rol.TienePermiso(rawUrl, Model.Enums.NivelAccesoPermiso.EJECUCION))
            //{
            //    Response.Redirect("~/Inicio.aspx");
            //    return;
            //}

            ////Permiso escribir
            //if (!rol.TienePermiso(rawUrl, Model.Enums.NivelAccesoPermiso.WRITE))
            //{
            //    Response.Redirect("~/Inicio.aspx");
            //    return;
            //}
        }

    }
}
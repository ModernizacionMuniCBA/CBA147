using System;
using System.Linq;
using Model.Entities;
using Rules;
using UI.Resources;
using System.Collections.Generic;
using Rules.Rules;
using System.Web;
using System.Web.UI;
using Intranet_UI.Utils;
using Model.Resultados;
using Model.Consultas;
using UI.Servicios;

namespace UI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {

            var data = new Dictionary<string, object>();

            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            if (user == null)
            {
                Response.Redirect(ResolveUrl("~/Paginas/_CerrarSesion.aspx"), true);
                return;
            }


            var rol = user.Rol;
            if (rol == null)
            {
                Response.Redirect(ResolveUrl("~/Paginas/_CerrarSesion.aspx"), true);
                return;
            }

            //Valido el acceso que tiene el rol para la pagina a cargar
            var usuarioValido = SessionKey.ValidarAcceso(rol, Request.RawUrl);
            if (!usuarioValido)
            {
                Response.Redirect(ResolveUrl("~/Error?mensaje='No tiene los permisos necesarios para acceder a la página solicitada'"), true);
                return;
            }

            initJs(data);
        }

        private void initJs(object data)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + JsonUtils.toJson(data) + "'); initBase(data); });", true);
        }
    }


}
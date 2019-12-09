using System;
using System.Linq;
using Model.Entities;
using Rules;
using UI.Resources;
using System.Web.UI;
using System.Collections.Generic;
using Rules.Rules;
using System.Web;
using Intranet_UI.Utils;
using UI.Servicios;
using System.Text;

namespace UI.IFrame
{
    public partial class _MasterPageIFrame : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = ConfiguracionGeneral.SiglasSistema + " - " + ConfiguracionGeneral.NombreSistema;


        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            if (user == null)
            {
                Response.Redirect("CerrarSesion.aspx");
                return;
            }

            Page.Title = "#CBA147";

            if (Request.RawUrl.Contains("Error.aspx"))
            {
                return;
            }

            var rol = user.Rol;
            if (rol == null)
            {
                Response.Redirect("CerrarSesion.aspx");
                return;
            }

            var data = new Dictionary<string, object>();

            data.Add("UsuarioLogeado", user);
            data.Add("InitData", new UsuarioService().GetInitData().Return);

            var funcion = "$(function () { var data = parse('" + JsonUtils.toJson(data) + "'); initBase(data); });";
            funcion = "$(window).on('load', function(){" + funcion + "} );";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptIFrame", funcion, true);

        }

    }
}
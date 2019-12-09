using Intranet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Resources;

namespace UI
{
    public partial class Acceso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147";

            //if (SessionKey.IsLogin(Session))
            //{
            //    var user = SessionKey.getUsuarioLogueado(Session);
            //    if (!user.IdOrigenElegido.HasValue)
            //    {
            //        Response.Redirect("~/Origen", false);
            //    }
            //    else
            //    {
            //        Response.Redirect("~/Sistema", false);
            //    }
            //}

            string token = Request.QueryString["token"];
            var resultadoLogin = new Servicios.UsuarioService().SetToken(token);
            if (resultadoLogin.Ok && resultadoLogin.Return == true)
            {
                var user = SessionKey.getUsuarioLogueado(Session);
                if (!user.IdOrigenElegido.HasValue)
                {
                    Response.Redirect("~/Origen", false);
                }
                else
                {
                    Response.Redirect("~/Sistema", false);
                }

                return;
            }

            if (token != null)
            {
                Response.Redirect("~/AccesoError.aspx?Error=" + resultadoLogin.Error, false);
            }
            else
            {
                var url = ConfigurationManager.AppSettings["URL_LOGIN"];
                Response.Redirect(url, false);
            }



            ////Devuelvo la data
            //var data = JsonUtils.toJson(resultado);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}
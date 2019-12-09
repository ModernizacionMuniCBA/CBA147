using System;
using System.Linq;
using Internet_UI.Utils;
using System.Configuration;

namespace Internet_UI.Paginas
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Vecino Virtual · Acceder a #CBA147";

            string token = Request.QueryString["token"];
            var resultadoLogin = new Servicios.ServicioUsuario().SetToken(token);
            if (resultadoLogin.Ok && resultadoLogin.Return == true)
            {
                Response.Redirect("~/" + Consts.PAGE_INICIO, false);
                return;
            }

            var url=ConfigurationManager.AppSettings["URL_LOGIN"];
            Response.Redirect(url, false);

        }
    }
}
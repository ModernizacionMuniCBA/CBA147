using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Internet_UI
{
    public partial class Default : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            var resultToken = new Servicios.ServicioUsuario().ValidarToken();
            if (!resultToken.Ok || !resultToken.Return.Value)
            {
                //pagina de fede
                Response.Redirect(ConfigurationManager.AppSettings["URL_LOGIN"]);
                return;
            }
            Response.Redirect("~/Inicio");
        }
    }
}
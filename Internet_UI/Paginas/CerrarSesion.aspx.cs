using System;
using System.Linq;
using Internet_UI.Utils;

namespace Internet_UI.Paginas
{
    public partial class CerrarSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new Servicios.ServicioUsuario().CerrarSesion();
            Response.Redirect("~/" + Consts.PAGE_LOGIN, false);
        }
    }
}
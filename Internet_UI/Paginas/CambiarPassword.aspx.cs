using Internet_UI.Utils;
using System;
using System.Linq;

namespace Internet_UI.Paginas
{
    public partial class CambiarPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147 · Cambiar contraseña";
            JsUtils.InitPage(this, null);
        }
    }
}
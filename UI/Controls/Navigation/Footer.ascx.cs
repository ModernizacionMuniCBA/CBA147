using System;
using System.Linq;
using UI.Resources;

namespace UI.Controls.Navigation
{
    public partial class Footer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ////Muni
            //textoMuni.InnerText = ConfiguracionGeneral.NombreMuni;

            ////Sistema
            //textoSistema.InnerText = ConfiguracionGeneral.NombreSistema;

            ////Version
            //textoVersion.InnerText = "V" + ConfiguracionGeneral.Version;

            ////Copyright
            //textoCopyRight.InnerHtml = "©" + DateTime.Now.Year;
        }
    }
}
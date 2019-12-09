using System.Collections.Generic;
using Internet_UI.Utils;
using System;
using System.Linq;

namespace Internet_UI.Paginas
{
    public partial class Ajustes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147 · Ajustes";

            var resultado = new Dictionary<string, object>();

            // Busco ajustes
            var resultadoAppData = new Servicios.ServicioAjustes().GetAppData();
            if (!resultadoAppData.Ok)
            {
                resultado.Add("Error", resultadoAppData.Error);
                JsUtils.InitPage(this, resultado);
                return;
            }
            resultado.Add("AppData", resultadoAppData.Return);

            JsUtils.InitPage(this, resultado);
        }
    }
}
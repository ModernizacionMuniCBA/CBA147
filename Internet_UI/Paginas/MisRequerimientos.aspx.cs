using Internet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Internet_UI.Paginas
{
    public partial class MisRequerimientos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147 · Mis requerimientos";

            var resultado = new Dictionary<string, object>();

            // Busco requerimientos
            var resultadoRequerimientos = new Servicios.ServicioRequerimiento().GetMisRequerimientos();
            if (!resultadoRequerimientos.Ok)
            {
                resultado.Add("Error", resultadoRequerimientos.Error);
                JsUtils.InitPage(this, resultado);
                return;
            }
            resultado.Add("Requerimientos", resultadoRequerimientos.Return);

            JsUtils.InitPage(this, resultado);
        }
    }
}
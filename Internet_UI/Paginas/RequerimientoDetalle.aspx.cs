using Internet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Internet_UI.Paginas
{
    public partial class RequerimientoDetalle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147 · Detalle de requerimientos";

            var resultado = new Dictionary<string, object>();

            int id = 0;
            if (Request.Params["id"] == null || !int.TryParse(Request.Params["id"], out id))
            {
                resultado.Add("Error", "Error inicializando la pagina");
                JsUtils.InitPage(this, resultado);
                return;
            }

            //Busco requerimiento
            var resultadoRequerimiento = new Servicios.ServicioRequerimiento().GetDetalle(id);
            if (!resultadoRequerimiento.Ok)
            {
                resultado.Add("Error", resultadoRequerimiento.Error);
                JsUtils.InitPage(this, resultado);
                return;
            }
            resultado.Add("Requerimiento", resultadoRequerimiento.Return);
            JsUtils.InitPage(this, resultado);
        }
    }
}
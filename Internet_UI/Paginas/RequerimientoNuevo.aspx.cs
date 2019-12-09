using Internet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Internet_UI.Paginas
{
    public partial class RequerimientoNuevo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147 · Nuevo requerimiento";

            var resultado = new Dictionary<string, object>();

            // Busco servicios
            var resultadoServicios = new Servicios.ServicioServicio().Get();
            if (!resultadoServicios.Ok)
            {
                resultado.Add("Error", resultadoServicios.Error);
                JsUtils.InitPage(this, resultado);
                return;
            }
            resultado.Add("Servicios", resultadoServicios.Return);

            // Busco motivos
            var resultadoMotivos = new Servicios.ServicioMotivo().GetParaBusqueda();
            if (!resultadoMotivos.Ok)
            {
                resultado.Add("Error", resultadoMotivos.Error);
                JsUtils.InitPage(this, resultado);
                return;
            }
            resultado.Add("Motivos", resultadoMotivos.Return.OrderBy(x => x.MotivoNombre));

            JsUtils.InitPage(this, resultado);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Controls.Navigation;
using UI.Resources;
using Model.Resultados;
using Intranet_UI.Utils;
using Model.Consultas;

namespace UI
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();
            var cantidad = 5;
            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            new ArchivoPorRequerimientoRules(user).Migrar();

            //Ultimos
            var resultadoUltimosRQ = new RequerimientoRules(user).GetUltimos(cantidad);
            if (!resultadoUltimosRQ.Ok || resultadoUltimosRQ.Return == null)
            {
                resultado.Add("Error", "Error leyendo los últimos requerimientos");
                InitJs(resultado);
                return;
            }
            resultado.Add("Ultimos_RQ", resultadoUltimosRQ.Return.Data);

            var resultadoAntiguosRQ = new RequerimientoRules(user).GetIdsPeligrososUltimos(cantidad);
            if (!resultadoAntiguosRQ.Ok || resultadoAntiguosRQ.Return == null)
            {
                resultado.Add("Error", "Error leyendo los requerimientos antiguos");
                InitJs(resultado);
                return;
            }
            resultado.Add("Ultimos_RQPeligrosos", resultadoAntiguosRQ.Return.Data);

            var resultadoEstadoRQ = new EstadoRequerimientoRules(user).GetByKeyValue(Enums.EstadoRequerimiento.NUEVO);
            if (!resultadoEstadoRQ.Ok || resultadoEstadoRQ.Return == null)
            {
                resultado.Add("Error", "Error leyendo las ordenes de trabajo");
                InitJs(resultado);
                return;
            }

            resultado.Add("EstadoRQ", new Resultado_EstadoRequerimiento(resultadoEstadoRQ.Return));

            var resultadoUltimasOT = new OrdenTrabajoRules(user).GetUltimas(cantidad);
            if (!resultadoUltimasOT.Ok || resultadoUltimasOT.Return == null)
            {
                resultado.Add("Error", "Error leyendo las ordenes de trabajo");
                InitJs(resultado);
                return;
            }
            resultado.Add("Ultimas_OT", resultadoUltimasOT.Return.Data);

            var resultadoEstadoOrdenes = new EstadoOrdenTrabajoRules(user).GetByKeyValue(Enums.EstadoOrdenTrabajo.ENPROCESO);
            if (!resultadoEstadoOrdenes.Ok || resultadoEstadoOrdenes.Return == null)
            {
                resultado.Add("Error", "Error leyendo las ordenes de trabajo");
                InitJs(resultado);
                return;
            }
            resultado.Add("EstadoOrdenes", new Resultado_EstadoOrdenTrabajo(resultadoEstadoOrdenes.Return));

            //Devuelvo la info
            InitJs(resultado);

        }

        private void InitJs(object resultado)
        {
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = JSON.parse('" + data + "'); init(data); });", true);
        }
    }
}
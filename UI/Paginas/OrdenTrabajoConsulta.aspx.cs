using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI
{
    public partial class OrdenTrabajoConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();

            //Areas
            //var areas = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Areas;
            //if (areas == null)
            //{
            //    Response.Redirect("CerrarSesion.aspx");
            //    return;
            //}
            //resultado.Add("Areas", Area.ToDictionary(areas));

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var estadoOrdenTrabajoRules = new EstadoOrdenTrabajoRules(userLogeado);

            //Estados
            var resultEstados = estadoOrdenTrabajoRules.GetAll(false);
            if (!resultEstados.Ok)
            {
                Response.Redirect("Error.aspx?error='Error leyendo los estados'");
                return;
            }
            resultado.Add("Estados", Resultado_EstadoOrdenTrabajo.ToList(resultEstados.Return));

            //Estados por defecto para Mis Trabajos
            var resultEstadosParaEditar = estadoOrdenTrabajoRules.GetEstadosPorDefectoMisTrabajos();
            if (!resultEstadosParaEditar.Ok)
            {
                Response.Redirect("Error.aspx?error='Error leyendo los estados'");
                return;
            }
            resultado.Add("EstadosPorDefectoMisTrabajos", Resultado_EstadoOrdenTrabajo.ToList(resultEstadosParaEditar.Return));

            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init2( '" + data + "' ); });", true);
        }

        //public void GenerarReporteListadoOrdenTrabajo(object sender, System.EventArgs e)
        //{
        //    List<int> idsRequerimientos = null;

        //    var resultRequerimientos = new RequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetAll(false);

        //    if (!resultRequerimientos.Ok)
        //    {
        //        return;
        //    }


        //    //ReporteModal.GenerarReporteListadoRequerimientos(resultRequerimientos.Return);
        //}
    }
}
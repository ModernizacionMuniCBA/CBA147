using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model.Entities;
using Rules;
using Rules.Rules;
using Model;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;
using UI.Servicios;

namespace UI
{
    public partial class RequerimientoConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();

            //Areas
            var areas = new List<Resultado_Area>();
            var resultadoAreas = new _CerrojoAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetAll(false);
            if (!resultadoAreas.Ok)
            {
                Response.Redirect("CerrarSesion.aspx");
                return;
            }

            areas = resultadoAreas.Return.Select(x => new Resultado_Area(x)).ToList();
            if (areas == null)
            {
                Response.Redirect("CerrarSesion.aspx");
                return;
            }
            resultado.Add("Areas", areas);

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //CPC
            var resultCpc = new CpcRules(userLogeado).GetAll();
            if (!resultCpc.Ok || resultCpc.Return == null)
            {
                Response.Redirect("Error.aspx?error='Error leyendo los CPC disponibles'");
                return;
            }
            resultado.Add("CPC", Resultado_Cpc.ToList(resultCpc.Return));

            //Barrios
            var resultBarrio = new BarrioRules(userLogeado).GetAll();
            if (!resultBarrio.Ok || resultBarrio.Return == null)
            {
                Response.Redirect("Error.aspx?error='Error leyendo los Barrios disponibles'");
                return;
            }
            resultado.Add("Barrios", Resultado_Barrio.ToList(resultBarrio.Return));

            //Origenes
            var resultOrigenes = new OrigenRules(userLogeado).GetAll(false);
            if (!resultOrigenes.Ok || resultOrigenes.Return == null)
            {
                Response.Redirect("Error.aspx?error='Error leyendo los orígenes disponibles'");
                return;
            }
            resultado.Add("Origenes", Resultado_Origen.ToList(resultOrigenes.Return));

            //Estados
            var estadoRequerimientoRules = new EstadoRequerimientoRules(userLogeado);
            var resultEstados = estadoRequerimientoRules.GetAll(false);
            if (!resultEstados.Ok || resultEstados.Return == null)
            {
                Response.Redirect("Error.aspx?error='Error leyendo los Estados'");
                return;

            }
            resultado.Add("Estados", Resultado_EstadoRequerimiento.ToList(resultEstados.Return));

            //Estado Nuevo
            var resultEstadoNuevo = estadoRequerimientoRules.GetByKeyValue(Enums.EstadoRequerimiento.NUEVO);
            if (!resultEstadoNuevo.Ok || resultEstadoNuevo.Return == null)
            {
                Response.Redirect("Error.aspx?error='Error leyendo los Estados'");
                return;

            }
            resultado.Add("EstadoNuevo", new Resultado_EstadoRequerimiento(resultEstadoNuevo.Return));

            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}
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
using Model.Resultados;
using Intranet_UI.Utils;

namespace UI
{
    public partial class OrdenAtencionCriticaConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var estadoOrdenAtencionCriticaRules = new EstadoOrdenEspecialRules(userLogeado);


            //Areas
            var areas = new _CerrojoAreaRules(userLogeado).GetAll(false);
            resultado.Add("Areas", Resultado_Area.ToList(areas.Return));

            //Estados
            var resultEstados = estadoOrdenAtencionCriticaRules.GetAll(false);
            if (!resultEstados.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                initJs(resultado);
                return;
            }
            resultado.Add("Estados", Resultado_EstadoOrdenAtencionCritica.ToList(resultEstados.Return));

            //Estados Para Editar
            var resultEstadosParaEditar = estadoOrdenAtencionCriticaRules.GetEstadosValidosParaEdicion();
            if (!resultEstadosParaEditar.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                initJs(resultado);
                return;
            }
            resultado.Add("EstadosParaEditar", Resultado_EstadoOrdenAtencionCritica.ToList(resultEstadosParaEditar.Return));

            //Estados Para Cerrar
            var resultEstadosParaCompletar = estadoOrdenAtencionCriticaRules.GetEstadosValidosParaCompletar();
            if (!resultEstadosParaCompletar.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                initJs(resultado);
                return;
            }
            resultado.Add("EstadosParaCompletar", Resultado_EstadoOrdenAtencionCritica.ToList(resultEstadosParaCompletar.Return));

            //Devuelvo la info
            initJs(resultado);
        }

        private void initJs(object resultado)
        {
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}
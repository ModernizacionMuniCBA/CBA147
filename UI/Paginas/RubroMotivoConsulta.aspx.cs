using Intranet_UI.Utils;
using Model.Entities;
using Model.Resultados;
using Rules;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using UI.Resources;

namespace UI
{
    public partial class RubroMotivoConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Grupo
            var resultGrupo = new GrupoRubroMotivoRules(userLogueado).GetAll(false);
            if (!resultGrupo.Ok || resultGrupo.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar los grupos.");
                var dataError = JsonUtils.toJson(resultado);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + dataError + "' ); });", true);
                return;
            }
            resultado.Add("Grupos", Resultado_GrupoCategoriaMotivo.ToList(resultGrupo.Return));

            //Categorias motivo
            var resultCategorias = new RubroMotivoRules(userLogueado).GetAll(false);
            if (!resultCategorias.Ok || resultCategorias.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar las categorias.");
                var dataError = JsonUtils.toJson(resultado);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + dataError + "' ); });", true);
                return;
            }
            resultado.Add("RubrosMotivo", ResultadoTabla_RubroMotivo.ToList(resultCategorias.Return));

            //Devuelvo la data
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }
        private void InitUsuario(string rawUrl)
        {
            var rol = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Rol;
        }
    }
}
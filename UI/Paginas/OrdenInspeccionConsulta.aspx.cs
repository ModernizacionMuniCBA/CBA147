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
    public partial class OrdenInspeccionConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var estadoOrdenInspeccionRules = new EstadoOrdenInspeccionRules(userLogeado);

            //Estados
            var resultEstados = estadoOrdenInspeccionRules.GetAll(false);
            if (!resultEstados.Ok)
            {
                Response.Redirect("Error.aspx?error='Error leyendo los estados'");
                return;
            }
            resultado.Add("Estados", Resultado_EstadoOrdenInspeccion.ToList(resultEstados.Return));

            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}
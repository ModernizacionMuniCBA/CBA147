using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IOrdenAtencionCriticaDetalle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Request.QueryString["Id"] == null)
            {
                return;
            }

            int id = Int32.Parse(Request.QueryString["Id"]);

            //Orden Atencion Critica
            var resultOAC = new OrdenEspecialRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetById(id);
            if (!resultOAC.Ok || resultOAC.Return == null)
            {
                Response.Redirect("Error.aspx?error='Error consultando la orden de atención crítica");
                return;

            }
            var oac = new Resultado_OrdenAtencionCritica(resultOAC.Return);

            //Domicilio
            var resultado = new Dictionary<string, object>();

            resultado.Add("OrdenAtencionCritica", oac);

            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }

        private void LLamarJavasCript(string funcion)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { " + funcion + "  });", true);
        }

    }
}
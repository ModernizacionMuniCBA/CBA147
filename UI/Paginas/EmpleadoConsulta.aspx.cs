using Intranet_UI.Utils;
using Model.Consultas;
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
    public partial class EmpleadoConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();

            //Funciones
            var resultFunciones = new FuncionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetByMisAreas(false);
            if (!resultFunciones.Ok || resultFunciones.Return == null)
            {
                Response.Redirect("Error.aspx?error='Error leyendo las Funciones'");
                return;

            }
            resultado.Add("Funciones", resultFunciones.Return);

            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);

        }
    }
}
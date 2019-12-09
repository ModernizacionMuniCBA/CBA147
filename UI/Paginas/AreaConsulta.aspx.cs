using System.Web.Script.Serialization;
using Model.Entities;
using Rules;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI
{
    public partial class AreaConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var areaRules = new _CerrojoAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session));            

            //Areas
            var resultAreas = areaRules.GetAll();
            if (!resultAreas.Ok || resultAreas.Return == null)
            {
                return;
            }
            resultado.Add("Areas", Resultado_Area.ToList(resultAreas.Return));

            //Devuelvo la data
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}
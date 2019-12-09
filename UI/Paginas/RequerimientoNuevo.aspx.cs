using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;

namespace UI
{
    public partial class RequerimientoNuevo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            //Me fijo si es interno
            if (Request.Params["Tipo"] != null)
            {
                var resultado = new Dictionary<string, object>();
                resultado.Add("Tipo", Request.Params["Tipo"]);
                initJs(resultado);
                return;
            }
        }

        private void initJs(object dictionary)
        {
            var data = JsonUtils.toJson(dictionary);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + data + "'); init(data); });", true);
        }
    }
}
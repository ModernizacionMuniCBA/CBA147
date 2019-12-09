using Intranet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Resources;

namespace UI
{
    public partial class AccesoError: System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147";
            var resultado = new Dictionary<string, string>();
            if (Request.Params["Error"] != null)
            {
                resultado.Add("Error", Request.Params["Error"]);
            }

           InitJs(resultado);
        }

        public void InitJs(Object data)
        {
            var funcion = "$(function () { var data = parse('" + JsonUtils.toJson(data) + "'); init(data); });";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptIFrame2", funcion, true);
        }
    }
}
using Intranet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI.IFrame
{
    public partial class _IFrame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void InitJs(Object data)
        {
            var funcion = "$(function () { var data = parse('" + JsonUtils.toJson(data) + "'); init(data); });";
            funcion = "$(window).on('load', function(){ setTimeout(function(){" + funcion+ "},100);});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptIFrame2", funcion, true);
        }
    }
}
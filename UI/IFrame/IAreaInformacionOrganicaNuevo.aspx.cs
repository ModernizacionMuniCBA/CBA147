using Intranet_UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI.IFrame
{
    public partial class IAreaInformacionOrganicaNuevo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            LLamarJavasCript(resultado);
        }

        private void LLamarJavasCript(object data)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + JsonUtils.toJson(data) + "'); init(data);  });", true);
        }
    }
}
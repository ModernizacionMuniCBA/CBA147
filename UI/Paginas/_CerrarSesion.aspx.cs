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

namespace UI
{
    public partial class _CerrarSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "parent.cerrarSesion();", true);
        }
    }
}
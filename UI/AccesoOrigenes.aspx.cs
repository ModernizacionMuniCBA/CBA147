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
    public partial class AccesoOrigenes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "#CBA147";

            if (!SessionKey.IsLogin(Session))
            {
                Response.Redirect("~/Login", false);
                return;
            }

            var user = SessionKey.getUsuarioLogueado(Session);
            if (user.IdOrigenElegido.HasValue)
            {
                Response.Redirect("~/Sistema", false);
                return;
            }

            var resultado = new Dictionary<string, object>();
            resultado.Add("Origenes", user.OrigenesDisponibles);
            InitJs(resultado);
        }

        public void InitJs(Object data)
        {
            var funcion = "$(function () { var data = parse('" + JsonUtils.toJson(data) + "'); init(data); });";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptIFrame2", funcion, true);
        }
    }
}
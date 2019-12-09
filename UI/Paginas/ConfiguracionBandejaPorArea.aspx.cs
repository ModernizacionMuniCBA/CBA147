using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using System.Web.UI;

using Intranet_UI.Utils;
using Model.Resultados;

namespace UI
{
    public partial class ConfiguracionBandejaPorArea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("TiposMotivo", new TipoMotivoRules(SessionKey.getUsuarioLogueado(Session)).GetAll());
            initJs(dictionary);
        }

        private void initJs(object dictionary)
        {
            var data = JsonUtils.toJson(dictionary);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { var data = parse('" + data + "'); init(data); });", true);
        }
    }
}
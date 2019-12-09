using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using Intranet_UI.Utils;

namespace UI.Controls
{
    public partial class SelectorPersona : System.Web.UI.UserControl
    
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();

            //Tipo Documento
            var resultTipoDocumento = new TipoDocumentoRules(            SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetAll(false);
            if (!resultTipoDocumento.Ok || resultTipoDocumento.Return == null)
            {
                return;
            }
            //resultado.Add("TiposDocumento", TipoDocumento.ToDictionary(resultTipoDocumento.Return));


            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { initSelectorPersona( '" + data + "' ); });", true);
        }

    }
}
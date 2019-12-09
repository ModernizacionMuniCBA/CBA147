using Intranet_UI.Utils;
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
using UI.Servicios;

namespace UI
{
    public partial class CatalogoConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var resultadoInfo = new MotivoService().GetInfo();
            if (!resultadoInfo.Ok)
            {
                resultado.Add("Error", resultadoInfo.Errores.ToStringPublico());
                init(resultado);
                return;
            }

            resultado.Add("Info", resultadoInfo.Return);

            //Devuelvo la data
            init(resultado);
        }

        private void init(object resultado)
        {
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { let data = parse('" + data + "'); init(data); });", true);
        }
    }
}
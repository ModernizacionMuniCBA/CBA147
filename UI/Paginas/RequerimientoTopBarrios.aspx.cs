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
using UI.Servicios;

namespace UI.Paginas
{
    public partial class RequerimientoTopBarrios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var resultadoTop = new RequerimientoService().GetTopPorBarrio();
            if (!resultadoTop.Ok)
            {
                resultado.Add("Error", resultadoTop.Error);
                InitJs(resultado);
                return;
            }

            var resultadoMarcadores = new RequerimientoService().GetTopMarcadoresPorBarrio();
            if (!resultadoMarcadores.Ok)
            {
                resultado.Add("Error", resultadoMarcadores.Error);
                InitJs(resultado);
                return;
            }

            var resultadoInfo = new MotivoService().GetInfo();
            if (!resultadoInfo.Ok)
            {
                resultado.Add("Error", resultadoInfo.Errores.ToStringPublico());
                InitJs(resultado);
                return;
            }

            resultado.Add("Info", resultadoInfo.Return);
            resultado.Add("Top", resultadoTop.Return);
            resultado.Add("Marcadores", resultadoMarcadores.Return);

            InitJs(resultado);
        }

        private void InitJs(object resultado)
        {
            //Devuelvo la info
            var data = JsonUtils.toJson(resultado);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { init( '" + data + "' ); });", true);
        }
    }
}
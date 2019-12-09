using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;
using System.Web.UI;
using System.Web.Script.Serialization;
using Intranet_UI.Utils;

namespace UI.IFrame
{
    public partial class IOrigenPorAmbitoNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var resultadoAmbito = new _CerrojoAmbitoRules(SessionKey.getUsuarioLogueado(Session)).GetAll(false);
            if (!resultadoAmbito.Ok)
            {
                resultado.Add("Error", resultadoAmbito.ToStringPublico());
                InitJs(resultado);
                return;
            }

            var resultadoOrigenes = new OrigenRules(SessionKey.getUsuarioLogueado(Session)).GetAll(false);
            if (!resultadoOrigenes.Ok)
            {
                resultado.Add("Error", resultadoOrigenes.ToStringPublico());
                InitJs(resultado);
                return;
            }

            resultado.Add("Ambitos", Resultado_Ambito.ToList(resultadoAmbito.Return));
            resultado.Add("Origenes", Resultado_Origen.ToList(resultadoOrigenes.Return));

            InitJs(resultado);
        }

    }
}
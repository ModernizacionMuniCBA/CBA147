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
    public partial class IOrigenPorAreaNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idArea = -1;
            int.TryParse(Request.QueryString["IdArea"] + "", out idArea);
            int idOrigen = -1;
            int.TryParse(Request.QueryString["IdOrigen"] + "", out idOrigen);

            var resultado = new Dictionary<string, object>();

            var resultadoAreas = new _CerrojoAreaRules(SessionKey.getUsuarioLogueado(Session)).GetAll(false);
            if (!resultadoAreas.Ok)
            {
                resultado.Add("Error", resultadoAreas.ToStringPublico());
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

            resultado.Add("Areas", Resultado_Area.ToList(resultadoAreas.Return));
            resultado.Add("Origenes", Resultado_Origen.ToList(resultadoOrigenes.Return));
            resultado.Add("IdArea", idArea);
            resultado.Add("IdOrigen", idOrigen);

            InitJs(resultado);
        }

    }
}
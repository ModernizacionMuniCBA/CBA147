using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;
using Intranet_UI.Utils;
using UI.IFrame;

namespace UI
{
    public partial class IOrigenDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }

            int id = Int32.Parse(Request.QueryString["Id"]);
            var resultadoOrigen = new OrigenRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetById(id);
            if (!resultadoOrigen.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }

            if (resultadoOrigen.Return == null)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }

            resultado.Add("Origen", new Resultado_Origen(resultadoOrigen.Return));
            InitJs(resultado);
        }
    }
}
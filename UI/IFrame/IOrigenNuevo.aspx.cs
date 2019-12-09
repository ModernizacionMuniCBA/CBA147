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
    public partial class IOrigenNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] == null)
            {
                initNuevo();
            }
            else
            {
                initEditar();
            }
        }

        private void initNuevo()
        {
            InitJs(new Dictionary<string, object>());
        }

        private void initEditar()
        {
            var resultado = new Dictionary<string, object>();

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
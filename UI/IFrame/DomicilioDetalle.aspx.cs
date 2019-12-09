using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using UI.IFrame;
using Model.Resultados;

namespace UI
{
    public partial class DomicilioDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }

            var resultado = new Dictionary<string, object>();

            int id = Int32.Parse(Request.QueryString["Id"]);
            var resultDomicilio = new DomicilioRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetById(id);
            if (!resultDomicilio.Ok)
            {
                resultado.Add("Error", resultDomicilio.ToStringPublico());
                InitJs(resultado);
                return;
            }

            if (resultDomicilio.Return == null)
            {
                resultado.Add("Error", "No existe el domicilio");
                InitJs(resultado);
                return;
            }

            resultado.Add("Domicilio", new Resultado_Domicilio(resultDomicilio.Return));
            
            //linea
            InitJs(resultado);
        }

    }
}
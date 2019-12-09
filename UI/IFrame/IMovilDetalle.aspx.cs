using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IMovilDetalle : _IFrame
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

            //Persona Fisisca
            var resultMovil = new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetResultadoById(id);
            if (!resultMovil.Ok || resultMovil.Return == null)
            {
                resultado.Add("Error", resultMovil.Error);
                InitJs(resultado);
                return;
            }
            resultado.Add("Movil", resultMovil.Return);

            //Devuelvo la info
            InitJs(resultado);
        }

    }
}
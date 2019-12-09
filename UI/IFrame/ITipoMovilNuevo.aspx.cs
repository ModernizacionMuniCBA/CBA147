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
    public partial class ITipoMovilNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var tiposRules = new TipoMovilRules(userLogueado);

            //Zonas
            var resultTipos = tiposRules.GetAll();
            if (!resultTipos.Ok || resultTipos.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar los tipos.");
                InitJs(resultado);
                return;
            }
            resultado.Add("TiposMovil", Resultado_TipoMovil.ToList(resultTipos.Return));

            //Devuelvo la data
            InitJs(resultado);
        }

    }
}
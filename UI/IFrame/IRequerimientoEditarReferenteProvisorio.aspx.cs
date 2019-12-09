using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IRequerimientoEditarReferenteProvisorio : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Error consultando el requerimiento");
                InitJs(resultado);
                return;
            }

            int id = Int32.Parse(Request.QueryString["Id"]);

            var resultProvisorio = new RequerimientoRules(user).GetById(id);
            if (!resultProvisorio.Ok)
            {
                resultado.Add("Error", "Error consultando el requerimiento");
                InitJs(resultado);
                return;
            }

            resultado.Add("ReferenteProvisorio", new Resultado_ReferenteProvisorio(resultProvisorio.Return.ReferenteProvisorio));
            resultado.Add("IdRequerimiento", id);

            //Devuelvo la data
            InitJs(resultado);
        }
    }
}
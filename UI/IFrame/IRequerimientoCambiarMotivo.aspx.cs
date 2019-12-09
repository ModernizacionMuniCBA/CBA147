using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IRequerimientoCambiarMotivo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();
            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            int id = 0;
            if (!int.TryParse("" + Request.QueryString["Id"], out id) || id <= 0)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            if (Request.QueryString["DesdeOT"] != null)
            {
                resultado.Add("DesdeOT", Boolean.Parse(Request.QueryString["DesdeOT"]));
            }

            if (Request.QueryString["TipoMotivo"] != null)
            {
                resultado.Add("TipoMotivo", Boolean.Parse(Request.QueryString["TipoMotivo"]));
            }

            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            resultado.Add("IdRequerimiento", id);
            InitJs(resultado);
        }

    }
}
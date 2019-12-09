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
    public partial class IOrdenTrabajoCambiarSeccion : _IFrame
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

            if (Request.QueryString["IdArea"] == null)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            int idArea = 0;
            if (!int.TryParse("" + Request.QueryString["IdArea"], out idArea) || idArea <= 0)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            int idSeccion = 0;
            if (!int.TryParse("" + Request.QueryString["IdSeccion"], out idSeccion) )
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            resultado.Add("IdOrdenTrabajo", id);
            resultado.Add("IdSeccion", idSeccion);

            var consultaSecciones = new SeccionRules(usuarioLogeado).GetByArea(idArea);
            if (!consultaSecciones.Ok)
            {
                resultado.Add("Error", consultaSecciones.Error);
                InitJs(resultado);
                return;
            }

            resultado.Add("Secciones", consultaSecciones.Return);
            InitJs(resultado);
        }
    }
}
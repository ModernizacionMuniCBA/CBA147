using Intranet_UI.Utils;
using Model.Entities;
using Model.Resultados;
using Rules;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using UI.Resources;

namespace UI
{
    public partial class EdificioMunicipalConsulta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Categorias
            var consultaCategorias = new CategoriaEdificioMunicipalRules(userLogueado).GetAll(false);
            if (!consultaCategorias.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }
            resultado.Add("Categorias", Resultado_CategoriaEdificioMunicipal.ToList( consultaCategorias.Return));
            InitJs(resultado);
        }

        public void InitJs(Object data)
        {
            var funcion = "$(function () { var data = parse('" + JsonUtils.toJson(data) + "'); init(data); });";
            funcion = "$(window).on('load', function(){ setTimeout(function(){" + funcion + "},100);});";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptIFrame2", funcion, true);
        }
    }
}
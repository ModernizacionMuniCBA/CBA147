using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules.Rules;
using System.Collections.Generic;
using System.Web.Services;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IRequerimientoNuevo : _IFrame
    {
        public int Tipo { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            var resultado = new Dictionary<string, object>();
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Me fijo si es interno
            if (Request.Params["Tipo"] != null)
            {
                var valor = Request.Params["Tipo"];
                resultado.Add("Tipo", valor);
                //return;
            }
            
                        var resultCategorias = new CategoriaEdificioMunicipalRules(userLogeado).GetAllConEdificios();
            if (!resultCategorias.Ok)
            {
                resultado.Add("Error", resultCategorias.Error);
                InitJs(resultado);
                return;
            }

            resultado.Add("Categorias", resultCategorias.Return);

            //Devuelvo la info
            InitJs(resultado);
        }
    }
}
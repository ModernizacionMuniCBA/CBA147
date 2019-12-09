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
using Model.Entities;

namespace UI.IFrame
{
    public partial class ICategoriaMotivoAreaNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var categoriaRules = new CategoriaMotivoAreaRules(userLogueado);

            if (Request.Params["IdArea"] == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar las categorías del área.");
                InitJs(resultado);
                return;
            }

            var idArea = int.Parse(Request.Params["IdArea"]);
            resultado.Add("IdArea", idArea);
            
            //Categorias
            var resultCategorias = categoriaRules.GetByIdArea(idArea);
            if (!resultCategorias.Ok || resultCategorias.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar las categorías del área.");
                InitJs(resultado);
                return;
            }
            resultado.Add("CategoriasMotivosArea", resultCategorias.Return);

            //Devuelvo la data
            InitJs(resultado);
        }

    }
}
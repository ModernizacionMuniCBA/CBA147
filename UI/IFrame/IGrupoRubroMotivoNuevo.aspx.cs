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
    public partial class IGrupoRubroMotivoNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //Categorias
            var resultGrupoCategorias =  new GrupoRubroMotivoRules(userLogueado).GetAll(false);
            if (!resultGrupoCategorias.Ok || resultGrupoCategorias.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar las categorías.");
                InitJs(resultado);
                return;
            }
            resultado.Add("GruposRubro", Resultado_GrupoCategoriaMotivo.ToList(resultGrupoCategorias.Return));

            //Devuelvo la data
            InitJs(resultado);
        }

    }
}
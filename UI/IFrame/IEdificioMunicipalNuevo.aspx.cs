using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Resultados;
using System.Web.UI;
using System.Web.Script.Serialization;
using Intranet_UI.Utils;
using Model.Entities;

namespace UI.IFrame
{
    public partial class IEdificioMunicipalNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

            if (Request.Params["Id"] != null)
            {
                var id = int.Parse(Request.Params["Id"] + "");

                //Edificio a editar
                var consulta = new EdificioMunicipalRules(usuarioLogeado).GetById(id);
                if (!consulta.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }
                resultado.Add("EdificioMunicipal", new Resultado_EdificioMunicipal(consulta.Return));
            }


            if (Request.Params["IdCategoria"] != null)
            {
                var id = int.Parse(Request.Params["IdCategoria"] + "");
                resultado.Add("IdCategoria",id);
            }
            
            //Categorias
            var consultaCategorias = new CategoriaEdificioMunicipalRules(usuarioLogeado).GetAll(false);
            if (!consultaCategorias.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }


            resultado.Add("Categorias", Resultado_CategoriaEdificioMunicipal.ToList( consultaCategorias.Return));
            InitJs(resultado);
        }

    }
}
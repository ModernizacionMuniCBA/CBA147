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
    public partial class IRubroMotivoPorMotivoNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

            if (Request.Params["Id"] != null)
            {
                var id = int.Parse(Request.Params["Id"] + "");

                //Rubro a editar
                var consulta = new RubroMotivoRules(usuarioLogeado).GetById(id);
                if (!consulta.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }
                resultado.Add("RubroMotivo", new Resultado_CategoriaMotivo(consulta.Return));
            }


            if (Request.Params["IdRubro"] != null)
            {
                var id = int.Parse(Request.Params["IdRubro"] + "");
                resultado.Add("IdRubro", id);
            }
            
            //Categorias
            var consultaCategorias = new GrupoRubroMotivoRules(usuarioLogeado).GetAll(false);
            if (!consultaCategorias.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }


            resultado.Add("Grupos", Resultado_GrupoCategoriaMotivo.ToList( consultaCategorias.Return));
            InitJs(resultado);
        }

    }
}
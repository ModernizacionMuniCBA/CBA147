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
    public partial class ICampoPorMotivoNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            if (Request.QueryString["IdMotivo"] != null)
            {
                int idMotivo = Int32.Parse(Request.QueryString["IdMotivo"]);
                resultado.Add("IdMotivo", idMotivo);

                var resultMotivo = new MotivoRules(userLogueado).GetById(idMotivo);
                if (!resultMotivo.Ok) {
                    resultado.Add("Error", "Error al consultar el motivo");
                    InitJs(resultado);
                }

                var grupos = resultMotivo.Return.Campos.Select(z => z.Grupo).ToList().Distinct();
                resultado.Add("Grupos", grupos);
            }
            
            //Tipos
            var resultTipos = new TipoCampoPorMotivoRules(userLogueado).GetAll();
            if (!resultTipos.Ok || resultTipos.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar los tipos.");
                InitJs(resultado);
                return;
            }
            resultado.Add("TiposCampos", Resultado_TipoCampoPorMotivo.ToList(resultTipos.Return));

            if (Request.QueryString["Id"] != null)
            {
                int id = Int32.Parse(Request.QueryString["Id"]);

                //Campo
                var resultCampo = new CampoPorMotivoRules(userLogueado).GetById(id);
                if (!resultCampo.Ok || resultCampo.Return == null)
                {
                    //Devuelvo la data
                    resultado.Add("Error", "Error al consultar el campo.");
                    InitJs(resultado);
                    return;
                }
                resultado.Add("Campo", new Resultado_CampoPorMotivo(resultCampo.Return));
            }       

            //Devuelvo la data
            InitJs(resultado);
        }

    }
}
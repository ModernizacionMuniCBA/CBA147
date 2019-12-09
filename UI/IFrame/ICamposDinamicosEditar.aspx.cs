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
    public partial class ICamposDinamicosEditar : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            int idMotivo = 0;
            if (Request.QueryString["IdMotivo"] != null)
            {
                idMotivo = Int32.Parse(Request.QueryString["IdMotivo"]);
                resultado.Add("IdMotivo", idMotivo);
            }
            
            //Tipos
            var resultCampos = new CampoPorMotivoRules(userLogueado).GetByIdMotivo(idMotivo);
            if (!resultCampos.Ok || resultCampos.Return == null)
            {
                //Devuelvo la data
                resultado.Add("Error", "Error al consultar los tipos.");
                InitJs(resultado);
                return;
            }
            resultado.Add("CamposPorMotivo", resultCampos.Return.ToList());

            if (Request.QueryString["Id"] != null)
            {
                int id = Int32.Parse(Request.QueryString["Id"]);

                //Campo
                var resultCamposXRq = new RequerimientoRules(userLogueado).GetDetalleCamposDinamicosById(id);
                if (!resultCamposXRq.Ok || resultCamposXRq.Return == null)
                {
                    //Devuelvo la data
                    resultado.Add("Error", "Error al consultar el campo.");
                    InitJs(resultado);
                    return;
                }
                resultado.Add("CamposPorRequerimiento", resultCamposXRq.Return);
                resultado.Add("IdRequerimiento", id);
            }       

            //Devuelvo la data
            InitJs(resultado);
        }

    }
}
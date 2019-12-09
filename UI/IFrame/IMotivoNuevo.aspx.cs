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
    public partial class IMotivoNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            resultado.Add("Areas", SessionKey.getUsuarioLogueado(Session).Areas);
            resultado.Add("Servicios", Resultado_Servicio.ToList(new ServicioRules(null).GetAll(false).Return));
            resultado.Add("IdServicio", Request.Params["IdServicio"]);
            resultado.Add("IdCategoria", Request.Params["IdCategoria"]);

            var lis=(Enum.GetValues(typeof(Enums.EsfuerzoMotivo))).OfType<Enums.EsfuerzoMotivo>().ToList();

            resultado.Add("Esfuerzos", new EnumsRules(SessionKey.getUsuarioLogueado(Session)).GetAll(typeof(Enums.EsfuerzoMotivo)).Return);

            var idArea=Request.Params["IdArea"];
            if (idArea != null)
            {
                resultado.Add("IdArea", idArea);
                resultado.Add("Categorias", (new CategoriaMotivoAreaRules(null)).GetByIdArea(Int32.Parse(idArea)).Return);
            }            

            try
            {
                if (Request.QueryString["Id"] != null)
                {
                    int id = Int32.Parse(Request.QueryString["Id"]);
                    var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                    var resultadoInfo = new MotivoRules(userLogeado).GetByIdObligatorio(id);
                    if (!resultadoInfo.Ok)
                    {
                        resultado.Add("Error", resultadoInfo.Errores.ToStringPublico());
                        InitJs(resultado);
                        return;
                    }

                    resultado.Add("Motivo", new Resultado_Motivo(resultadoInfo.Return));
                }
                InitJs(resultado);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
            }
        }
    }
}
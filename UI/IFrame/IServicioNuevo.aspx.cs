using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IServicioNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                if (Request.QueryString["Id"] != null)
                {
                    int id = Int32.Parse(Request.QueryString["Id"]);
                    var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                    var resultadoInfo = new ServicioRules(userLogeado).GetByIdObligatorio(id);
                    if (!resultadoInfo.Ok)
                    {
                        resultado.Add("Error", resultadoInfo.Errores.ToStringPublico());
                        InitJs(resultado);
                        return;
                    }

                    resultado.Add("Servicio", new Resultado_Servicio(resultadoInfo.Return));
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
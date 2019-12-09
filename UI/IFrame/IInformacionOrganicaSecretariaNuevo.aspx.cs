using Intranet_UI.Utils;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.Resources;

namespace UI.IFrame
{
    public partial class IInformacionOrganicaSecretariaNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

                if (Request.QueryString["Id"] != null)
                {
                    int id = int.Parse("" + Request.QueryString["Id"]);
                    var resultadoConsulta = new InformacionOrganicaSecretariaRules(usuarioLogeado).GetByIdObligatorio(id);
                    if (!resultadoConsulta.Ok)
                    {
                        resultado.Add("Error", "Error procesando la solicitud");
                        InitJs(resultado);
                        return;
                    }

                    resultado.Add("Secretaria", new Resultado_InformacionOrganicaSecretaria(resultadoConsulta.Return));
                }
                InitJs(resultado);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }
        }
    }
}
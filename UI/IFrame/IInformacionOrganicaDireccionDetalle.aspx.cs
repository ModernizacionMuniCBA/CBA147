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
    public partial class IInformacionOrganicaDireccionDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

                int id = int.Parse("" + Request.QueryString["Id"]);
                var resultadoConsulta = new InformacionOrganicaDireccionRules(usuarioLogeado).GetById(id);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Direccion", new Resultado_InformacionOrganicaDireccion(resultadoConsulta.Return));

                InitJs(resultado);
            }
            catch (Exception)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }
        }
    }
}
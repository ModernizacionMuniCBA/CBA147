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
using UI.Servicios;
using Model.Consultas;

namespace UI.IFrame
{
    public partial class IInformacionOrganicaSecretariaDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                int id = Int32.Parse(Request.QueryString["Id"]);
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                var resultadoConsulta = new InformacionOrganicaSecretariaService().GetById(id);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Add("Error", resultadoConsulta.Errores.ToStringPublico());
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Entity", resultadoConsulta.Return);

                var resultadoConsultaDirecciones = new InformacionOrganicaDireccionService().GetByIdSecretaria(id);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Add("Error", resultadoConsulta.Errores.ToStringPublico());
                    InitJs(resultado);
                    return;
                }
                resultado.Add("Direcciones", resultadoConsultaDirecciones.Return);
                InitJs(resultado);
            }
            catch (Exception)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
            }
        }
    }
}
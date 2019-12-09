using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using System.Collections.Generic;
using Model;
using Intranet_UI.Utils;
using UI.Resources;
using System.Web;
using Model.Resultados;
using UI.Servicios;

namespace UI.IFrame
{
    public partial class IMovilDetalle2 : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();


            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Error consultando el móvil");
                InitJs(resultado);
                return;
            }

            int id = Int32.Parse(Request.QueryString["Id"]);

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var requerimientoRules = new MovilRules(userLogeado);

            //Movil
            var resultConsultaMovil = new MovilService().GetDetalleById(id);
            if (!resultConsultaMovil.Ok)
            {
                resultado.Add("Error", "Error consultando el móvil");
                InitJs(resultado);
                return;
            }

            var movil = resultConsultaMovil.Return;
            if (movil == null)
            {
                resultado.Add("Error", "El móvil no existe");
                InitJs(resultado);
                return;
            }

            resultado.Add("Movil", movil);

            var resultEstadosOcupado = new EstadoMovilRules(userLogeado).GetAllOcupados();
            if (!resultEstadosOcupado.Ok)
            {
                resultado.Add("Error", "Error consultando alguno de los datos");
                InitJs(resultado);
                return;
            }

            resultado.Add("EstadosOcupado", resultEstadosOcupado.Return);
            //Devuelvo la info
            InitJs(resultado);
        }
    }
}
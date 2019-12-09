using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using Intranet_UI.Utils;
using Model.Resultados;
using UI.Servicios;
using Model.Consultas;

namespace UI.IFrame
{
    public partial class IOrdenInspeccionDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();

            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Error consultando la orden de inspección");
                InitJs(resultado);
                return;
            }

            int id = Int32.Parse(Request.QueryString["Id"]);

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //OI
            var resultadoConsulta= new OrdenInspeccionService().GetDetalleById(id);
            var entity = resultadoConsulta.Return;

            if (!resultadoConsulta.Ok)
            {
                resultado.Add("Error", "Error consultando la orden de inspección");
                InitJs(resultado);
                return;
            }

            if (resultadoConsulta.Return == null)
            {
                resultado.Add("Error", "La orden de inspección no existe");
                InitJs(resultado);
                return;
            }

            //Permisos
            var consultaPermisos = new PermisoEstadoOrdenInspeccionRules(userLogeado).GetPermisos();
            if (!consultaPermisos.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }

            resultado.Add("OrdenInspeccion", resultadoConsulta.Return);
            resultado.Add("Permisos", consultaPermisos.Return);
            InitJs(resultado);
        }
    }
}
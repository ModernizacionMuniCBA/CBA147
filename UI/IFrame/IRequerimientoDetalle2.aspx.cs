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
    public partial class IRequerimientoDetalle2 : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();


            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Error consultando el requerimiento");
                InitJs(resultado);
                return;
            }

            int id = Int32.Parse(Request.QueryString["Id"]);

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var requerimientoRules = new RequerimientoRules(userLogeado);
            var ordenTrabajoRules = new OrdenTrabajoRules(userLogeado);

            //RQ
            var resultConsultaRequerimiento = new RequerimientoService().GetDetalleById(id);
            var requerimiento = resultConsultaRequerimiento.Return;

            if (!resultConsultaRequerimiento.Ok)
            {
                resultado.Add("Error", "Error consultando el requerimiento");
                InitJs(resultado);
                return;
            }

            if (resultConsultaRequerimiento.Return == null)
            {
                resultado.Add("Error", "El requerimiento no existe");
                InitJs(resultado);
                return;
            }


            //Permisos
            var consultaPermisos = new PermisoEstadoRequerimientoRules(userLogeado).GetPermisos();
            if (!consultaPermisos.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }

            var tieneTareas = false;
            if (requerimiento.Tareas.Count() != 0)
            {
                tieneTareas = true;
            }
            else
            {
                var consultaTareas = new TareaPorAreaRules(userLogeado).GetCantidadByArea((int)requerimiento.AreaId);
                if (!consultaPermisos.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                tieneTareas = consultaTareas.Return == 0 ? false : true;
            }

            resultado.Add("TieneTareas", tieneTareas);

            var tieneCamposDinamicos = false;
            if (requerimiento.CamposDinamicos.Count() != 0)
            {
                tieneCamposDinamicos = true;
            }
            else
            {
                var consultaCamposDinamicos= new CampoPorMotivoRules(userLogeado).GetByIdMotivo((int)requerimiento.MotivoId);
                if (!consultaCamposDinamicos.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                tieneCamposDinamicos = consultaCamposDinamicos.Return.Count == 0 ? false : true;
            }

            resultado.Add("TieneCamposDinamicos", tieneCamposDinamicos);

            resultado.Add("Requerimiento", requerimiento);
            resultado.Add("Permisos", consultaPermisos.Return);

            //Devuelvo la info
            InitJs(resultado);
        }
    }
}
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
    public partial class IOrdenTrabajoDetalle : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();

            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Error consultando la orden de trabajo");
                InitJs(resultado);
                return;
            }

            int id = Int32.Parse(Request.QueryString["Id"]);

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            //OT
            var resultadoConsulta = new OrdenTrabajoService().GetDetalleById(id);
            var entity = resultadoConsulta.Return;

            if (!resultadoConsulta.Ok)
            {
                resultado.Add("Error", "Error consultando la orden de trabajo");
                InitJs(resultado);
                return;
            }

            if (resultadoConsulta.Return == null)
            {
                resultado.Add("Error", "La orden de trabajo no existe");
                InitJs(resultado);
                return;
            }

            //Permisos
            var consultaPermisos = new PermisoEstadoOrdenTrabajoRules(userLogeado).GetPermisos();
            if (!consultaPermisos.Ok)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }

            if (userLogeado.EsAmbitoMunicipalidad())
            {
                //Secciones 
                var consultaSecciones = new SeccionRules(userLogeado).GetIdsByArea(resultadoConsulta.Return.AreaId);
                if (!consultaSecciones.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                //Cantidad de Moviles 
                var consultaMoviles = new MovilRules(userLogeado).GetCantidadParaAgregarAOT(resultadoConsulta.Return.AreaId);
                if (!consultaMoviles.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                //Cantidad de Flotas 
                var consultaFlotas = new FlotaRules(userLogeado).GetCantidadParaAgregarAOT(resultadoConsulta.Return.AreaId);
                if (!consultaFlotas.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                //Cantidad de Empleados 
                var consultaEmpleados = new EmpleadoPorAreaRules(userLogeado).GetCantidadParaAgregarAOT(resultadoConsulta.Return.AreaId);
                if (!consultaEmpleados.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                //Cantidad de Tareas 
                var consultaTareas = new TareaPorAreaRules(userLogeado).GetCantidadByArea(resultadoConsulta.Return.AreaId);
                if (!consultaTareas.Ok)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Secciones", consultaSecciones.Return);
                resultado.Add("CantidadMoviles", consultaMoviles.Return);
                resultado.Add("CantidadFlotas", consultaFlotas.Return);
                resultado.Add("CantidadEmpleados", consultaEmpleados.Return);
                resultado.Add("CantidadTareas", consultaTareas.Return);
            }

            resultado.Add("OrdenTrabajo", resultadoConsulta.Return);
            resultado.Add("Permisos", consultaPermisos.Return);
            InitJs(resultado);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Model;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using UI.Servicios;

namespace UI.IFrame
{
    public partial class IOrdenTrabajoCerrar : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);
                var requerimientos=new List<int>();
                var permiso = 0;

                var id = 0;
                if (int.TryParse(Request.Params["IdOT"], out id))
                {
                    //OT
                    var resultadoOt = new OrdenTrabajoRules(usuarioLogeado).GetDetalleById(id);
                    if (!resultadoOt.Ok)
                    {
                        resultado.Add("Error", "Error buscando la orden de trabajo");
                        InitJs(resultado);
                        return;
                    }
                    resultado.Add("OrdenTrabajo", resultadoOt.Return);

                    var resultadoValidarPermiso = new PermisoEstadoOrdenTrabajoRules(null).TienePermiso((Enums.EstadoOrdenTrabajo)resultadoOt.Return.EstadoKeyValue, Enums.PermisoEstadoOrdenTrabajo.Cerrar);
                    if (!resultadoValidarPermiso.Ok)
                    {
                        resultado.Add("Error", "La orden de trabajo no se encuentra en un estado valido para cerrar");
                        InitJs(resultado);
                        return;
                    }

                    requerimientos = resultadoOt.Return.Requerimientos.Select(x => x.Id).ToList();
                    permiso = (int)Enums.PermisoEstadoRequerimiento.VerEnCerrarOrdenDeTrabajo;
                }

                if (int.TryParse(Request.Params["IdOI"], out id))
                {
                    //OT
                    var resultadoOi = new OrdenInspeccionRules(usuarioLogeado).GetDetalleById(id);
                    if (!resultadoOi.Ok)
                    {
                        resultado.Add("Error", "Error buscando la orden de inspección");
                        InitJs(resultado);
                        return;
                    }
                    resultado.Add("OrdenInspeccion", resultadoOi.Return);

                    var resultadoValidarPermiso = new PermisoEstadoOrdenInspeccionRules(null).TienePermiso((Enums.EstadoOrdenInspeccion)resultadoOi.Return.EstadoKeyValue, Enums.PermisoEstadoOrdenInspeccion.Cerrar);
                    if (!resultadoValidarPermiso.Ok)
                    {
                        resultado.Add("Error", "La orden de inspección no se encuentra en un estado valido para cerrar");
                        InitJs(resultado);
                        return;
                    }

                    requerimientos = resultadoOi.Return.Requerimientos.Select(x => x.Id).ToList();
                    permiso = (int)Enums.PermisoEstadoRequerimiento.VerEnCerrarOrdenDeInspeccion;
                }

                //Requerimientos
                var resultadoRequerimientos = new RequerimientoService().GetResultadoTablaByIds(requerimientos);
                if (!resultadoRequerimientos.Ok)
                {
                    resultado.Add("Error", "Error consultando los requerimientos");
                    InitJs(resultado);
                    return;
                }

                resultado.Add("Requerimientos", resultadoRequerimientos.Return);
                resultado.Add("Permiso", permiso);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
            }

            //Devuelvo la info
            InitJs(resultado);
        }

    }
}
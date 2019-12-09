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
    public partial class IOrdenTrabajoAgregarRequerimiento : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();
            int cantidad = 30;
            var consulta = new Consulta_Requerimiento_Bandeja();

            try
            {
                var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

                int id = 0;
                if (int.TryParse(Request.Params["IdOT"] + "", out id))
                {
                    var resultadoOT = new OrdenTrabajoService().GetDetalleById(id);
                    if (!resultadoOT.Ok)
                    {
                        resultado.Add("Error", resultadoOT.ToStringPublico());
                        InitJs(resultado);
                        return;
                    }

                    var ot = resultadoOT.Return;
                    if (ot == null)
                    {
                        resultado.Add("Error", "No existe la orden de trabajo");
                        InitJs(resultado);
                        return;
                    }

                    resultado.Add("OrdenTrabajo", ot);
                    consulta = new Model.Consultas.Consulta_Requerimiento_Bandeja()
                 {
                     IdArea = ot.AreaId,
                     DadosDeBaja = false,
                     Limite = cantidad
                 };
                }

                if (int.TryParse(Request.Params["IdOI"] + "", out id))
                {
                    var resultadoOI = new OrdenInspeccionService().GetDetalleById(id);
                    if (!resultadoOI.Ok)
                    {
                        resultado.Add("Error", resultadoOI.ToStringPublico());
                        InitJs(resultado);
                        return;
                    }

                    var oi = resultadoOI.Return;
                    if (oi == null)
                    {
                        resultado.Add("Error", "No existe la orden de inspección");
                        InitJs(resultado);
                        return;
                    }

                    resultado.Add("OrdenInspeccion", oi);

                    consulta = new Model.Consultas.Consulta_Requerimiento_Bandeja()
                    {
                        DadosDeBaja = false,
                        Limite = cantidad
                    };
                }

                var resultadoRequerimientos = new RequerimientoService().GetResultadoTablaParaOrdenInspeccion(consulta);

                if (!resultadoRequerimientos.Ok)
                {
                    resultado.Add("Error", resultadoRequerimientos.ToStringPublico());
                    InitJs(resultado);
                    return;
                }


                resultado.Add("Limite", cantidad);
                resultado.Add("Requerimientos", resultadoRequerimientos.Return);

            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
            }
            InitJs(resultado);
        }
    }
}
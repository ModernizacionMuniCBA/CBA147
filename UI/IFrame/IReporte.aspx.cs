using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using Rules.Rules;
using Rules.Rules.Reportes;
using Telerik.Reporting.Processing;
using UI.Resources;
using System.Web;
using Model.Consultas;
using Model;
using UI.Servicios;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IReporte : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();
            var tipo = Request.Params["tipo"];
            if (tipo == null)
            {
                InitJs(resultado);
                return;
            }

            Telerik.Reporting.Report reporte = null;
            Telerik.Reporting.ReportBook reporteBook = null;
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);


            switch (tipo)
            {
                case "rq_nuevo":
                case "rq_detalle":
                    {
                        int id = int.Parse("" + Request.Params["Id"]);

                        var resultConsultaRequerimiento = new RequerimientoService().GetDetalleById(id);

                        if (!resultConsultaRequerimiento.Ok || resultConsultaRequerimiento.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }

                        var resultReporte = new RequerimientoReporteRules(userLogeado).GenerarReporteRequerimientoConMapa2(resultConsultaRequerimiento.Return, null, false);
                        if (!resultReporte.Ok || resultReporte.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }
                        reporte = resultReporte.Return;
                    } break;

                case "rq_detalle_mapa":
                    {
                        int id = int.Parse("" + Request.Params["Id"]);

                        var resultConsultaRequerimiento = new RequerimientoService().GetDetalleById(id);

                        if (!resultConsultaRequerimiento.Ok || resultConsultaRequerimiento.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }

                        var resultReporte = new RequerimientoReporteRules(userLogeado).GenerarReporteRequerimientoConMapa2(resultConsultaRequerimiento.Return, null, true);
                        if (!resultReporte.Ok || resultReporte.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }
                        reporte = resultReporte.Return;
                    } break;

                case "ot_detallada":
                    {
                        int id = int.Parse("" + Request.Params["Id"]);
                        var resultConsultaOrden = new OrdenTrabajoService().GetDetalleById(id);

                        if (!resultConsultaOrden.Ok || resultConsultaOrden.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }

                        var resultReporte = new OrdenTrabajoReporteRules(userLogeado).GenerarReporteOrdenTrabajoDetallado(resultConsultaOrden.Return, true);

                        if (!resultReporte.Ok || resultReporte.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }
                        reporteBook = resultReporte.Return;
                    } break;

                case "oi_detallada":
                    {
                        int id = int.Parse("" + Request.Params["Id"]);
                        var resultConsultaOrden = new OrdenInspeccionService().GetDetalleById(id);

                        if (!resultConsultaOrden.Ok || resultConsultaOrden.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }

                        var resultReporte = new OrdenInspeccionReporteRules(userLogeado).GenerarReporteOrdenInspeccionDetallado(resultConsultaOrden.Return, true);

                        if (!resultReporte.Ok || resultReporte.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }
                        reporteBook = resultReporte.Return;
                    } break;

                case "ot_detalle_caratula_sinMapa":
                    {

                        int id = int.Parse("" + Request.Params["Id"]);

                        var resultConsultaOrden = new OrdenTrabajoService().GetDetalleById(id);

                        if (!resultConsultaOrden.Ok || resultConsultaOrden.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }

                        var resultReporte = new OrdenTrabajoReporteRules(userLogeado).GenerarReporteOrdenTrabajoListadoRequerimientos(resultConsultaOrden.Return);

                        if (!resultReporte.Ok || resultReporte.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }
                        reporte = resultReporte.Return;
                    } break;

                case "oi_detalle_caratula_sinMapa":
                    {

                        int id = int.Parse("" + Request.Params["Id"]);

                        var resultConsultaOrden = new OrdenInspeccionService().GetDetalleById(id);

                        if (!resultConsultaOrden.Ok || resultConsultaOrden.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }

                        var resultReporte = new OrdenInspeccionReporteRules(userLogeado).GenerarReporteOrdenInspeccionListadoRequerimientos(resultConsultaOrden.Return);

                        if (!resultReporte.Ok || resultReporte.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }
                        reporte = resultReporte.Return;
                    } break;

                case "rq_detalle_sinMapa":
                    {
                        int id = int.Parse("" + Request.Params["Id"]);
                        //var rq = new RequerimientoRules(userLogeado).GetById(id);
                        var resultConsultaRequerimiento = new RequerimientoService().GetDetalleById(id);
                        if (!resultConsultaRequerimiento.Ok || resultConsultaRequerimiento.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }

                        var resultReporte = new RequerimientoReporteRules(userLogeado).GenerarReporteRequerimientoConMapa2(resultConsultaRequerimiento.Return, null, false);
                        if (!resultReporte.Ok || resultReporte.Return == null)
                        {
                            resultado.Add("Error", "Error procesando la solicitud");
                            InitJs(resultado);
                            return;
                        }

                        reporte = resultReporte.Return;
                    } break;
            }

            if (reporte == null && reporteBook == null)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }

            try
            {
                RenderingResult result = null;
                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                if (reporte != null)
                {
                    result = reportProcessor.RenderReport("PDF", reporte, null);
                }
                else
                {
                    result = reportProcessor.RenderReport("PDF", reporteBook, null);
                }

                var bytes = result.DocumentBytes;
                if (bytes == null)
                {
                    resultado.Add("Error", "Error procesando la solicitud");
                    InitJs(resultado);
                    return;
                }

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", "" + bytes.Length);
                Response.BinaryWrite(bytes);
            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error procesando la solicitud");
                InitJs(resultado);
                return;
            }
        }


        [WebMethod]
        public static Result<string> GenerarReporteListadoRequerimiento(List<int> ids, string filtros)
        {
            var resultado = new Result<string>();

            try
            {
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                var reporte = new RequerimientoReporteRules(userLogeado).GenerarReporteListadoRequerimientos(ids, filtros);
                if (reporte == null)
                {
                    resultado.AddErrorPublico("Error generando el reporte");
                    return resultado;
                }

                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

                resultado.Return = base64;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        [WebMethod]
        public static Result<string> GenerarReporteRequerimientoListadoV2(List<int> ids, string filtros)
        {
            var resultado = new Result<string>();

            try
            {
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                var reporte = new RequerimientoReporteRules(userLogeado).GenerarReporteRequerimientoListadoV2(ids, filtros);
                if (reporte == null)
                {
                    resultado.AddErrorPublico("Error generando el reporte");
                    return resultado;
                }

                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte.Return , null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

                resultado.Return = base64;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        [WebMethod]
        public static Result<string> GenerarReporteListadoOrdenTrabajo(List<int> ids, string filtros)
        {
            var resultado = new Result<string>();

            try
            {
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                Telerik.Reporting.Report reporte = null;

                var resultReporte = new OrdenTrabajoReporteRules(userLogeado).GenerarReporteListadoOrdenes(ids, filtros);
                if (!resultReporte.Ok || resultReporte.Return == null)
                {
                    resultado.AddErrorPublico("Error generando el reporte");
                    return resultado;
                }
                reporte = resultReporte.Return;

                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

                resultado.Return = base64;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        [WebMethod]
        public static Result<string> GenerarReporteListadoOrdenInspeccion(List<int> ids, string filtros)
        {
            var resultado = new Result<string>();

            try
            {
                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                Telerik.Reporting.Report reporte = null;

                var resultReporte = new OrdenInspeccionReporteRules(userLogeado).GenerarReporteListadoOrdenes(ids, filtros);
                if (!resultReporte.Ok || resultReporte.Return == null)
                {
                    resultado.AddErrorPublico("Error generando el reporte");
                    return resultado;
                }
                reporte = resultReporte.Return;

                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

                resultado.Return = base64;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        [WebMethod]
        public static string GenerarReporteEstadisticaCPC(string[] ids, Consulta_EstadisticaCPC consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaCpc(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return null;
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaCPC(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                return null;
            }
            reporte = resultReporte.Return;

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }

        [WebMethod]
        public static string GenerarReporteEstadisticaOrigen(string[] ids, Consulta_EstadisticaOrigen consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaOrigen(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaOrigen(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;


            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }

        [WebMethod]
        public static string GenerarReporteEstadisticaEficacia(string[] ids, Consulta_EstadisticaEficacia consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaEficacia(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaEficacia(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }
        [WebMethod]
        public static string GenerarReporteEstadisticaResueltos(string[] ids, Consulta_EstadisticaResueltos consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaResueltos(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaResueltos(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }

        [WebMethod]
        public static string GenerarReporteEstadisticaZona(string[] ids, Consulta_EstadisticaZona consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaZona(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaZona(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }

        [WebMethod]
        public static string GenerarReporteEstadisticaServicios(string[] ids, Consulta_EstadisticaServicios consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaServicios(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaServicios(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;


            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }
        [WebMethod]
        public static string GenerarReporteEstadisticaArea(string[] ids, Consulta_EstadisticaArea consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaArea(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaArea(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }
        [WebMethod]
        public static string GenerarReporteEstadisticaSubArea(string[] ids, Consulta_EstadisticaSubArea consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaSubArea(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaSubArea(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }
        [WebMethod]
        public static string GenerarReporteEstadisticaMotivos(string[] ids, Consulta_EstadisticaMotivos consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaMotivos(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaMotivos(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;


            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }
        [WebMethod]
        public static string GenerarReporteEstadisticaUsuario(string[] ids, Consulta_EstadisticaUsuario consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaUsuario(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticaUsuario(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;

            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }
        [WebMethod]
        public static string GenerarReporteEstadisticaRubros(string[] ids, Consulta_EstadisticaRubros consulta, string htmlFiltros)
        {
            Telerik.Reporting.Report reporte = null;

            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var resultEstadisticas = new EstadisticaRules(userLogeado).GetDatosEstadisticaRubros(consulta);

            if (!resultEstadisticas.Ok || resultEstadisticas.Return == null)
            {
                resultEstadisticas.AddErrorPublico("Error al generar el resumen estadístico");
                return "Error resultEstadisticas";
            }

            var resultReporte = new EstadisticaReporteRules(userLogeado).GenerarReporteEstadisticRubros(ids, resultEstadisticas.Return, htmlFiltros);
            if (!resultReporte.Ok || resultReporte.Return == null)
            {
                resultReporte.AddErrorPublico("Error");
                return "Error resultReporte";
            }
            reporte = resultReporte.Return;


            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
            var bytes = result.DocumentBytes;
            var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

            return base64;
        }

        /*Catalogos*/

        [WebMethod]
        public static Result<string> GenerarReporteCatalogoUsuarios(int idArea)
        {
            var resultado = new Result<string>();
            try
            {

                Telerik.Reporting.Report reporte = null;

                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                var resultDataInfoOrganica = new InformacionOrganicaRules(userLogeado).GetByIdArea(idArea);
                if (!resultDataInfoOrganica.Ok || resultDataInfoOrganica.Return == null)
                {
                    resultado.AddErrorPublico("Falta cargar la información orgánica del área");
                    return resultado;
                }

                var resultReporte = new CatalogoRules(userLogeado).GenerarReporteCatalogoUsuarios(idArea, resultDataInfoOrganica.Return);

                if (!resultReporte.Ok || resultReporte.Return == null)
                {

                    resultado.AddErrorPublico("Error generando el reporte");
                    return resultado;
                }
                reporte = resultReporte.Return;


                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

                resultado.Return = base64;


            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
        [WebMethod]
        public static Result<string> GenerarReporteCatalogoMotivos(int idArea)
        {
            var resultado = new Result<string>();
            try
            {

                Telerik.Reporting.Report reporte = null;

                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                var resultDataInfoOrganica = new InformacionOrganicaRules(userLogeado).GetByIdArea(idArea);
                if (!resultDataInfoOrganica.Ok || resultDataInfoOrganica.Return == null)
                {
                    resultado.AddErrorPublico("Falta cargar la información orgánica del área");
                    return resultado;
                }


                var resultReporte = new CatalogoRules(userLogeado).GenerarReporteCatalogoMotivos(idArea, resultDataInfoOrganica.Return);
                if (!resultReporte.Ok || resultReporte.Return == null)
                {

                    resultado.AddErrorPublico("Error generando el reporte");
                    return resultado;
                }
                reporte = resultReporte.Return;


                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

                resultado.Return = base64;


            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
        [WebMethod]
        public static Result<string> GenerarReporteCatalogoTareas(int idArea)
        {
            var resultado = new Result<string>();
            try
            {

                Telerik.Reporting.Report reporte = null;

                var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

                var resultDataInfoOrganica = new InformacionOrganicaRules(userLogeado).GetByIdArea(idArea);
                if (!resultDataInfoOrganica.Ok || resultDataInfoOrganica.Return == null)
                {
                    resultado.AddErrorPublico("Falta cargar la información orgánica del área");
                    return resultado;
                }



                var resultReporte = new CatalogoRules(userLogeado).GenerarReporteCatalogoTareas(idArea, resultDataInfoOrganica.Return);
                if (!resultReporte.Ok || resultReporte.Return == null)
                {

                    resultado.AddErrorPublico("No existen tareas definidas");
                    return resultado;
                }
                reporte = resultReporte.Return;


                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", reporte, null);
                var bytes = result.DocumentBytes;
                var base64 = "data:application/pdf;base64," + Convert.ToBase64String(bytes);

                resultado.Return = base64;


            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
    }
}
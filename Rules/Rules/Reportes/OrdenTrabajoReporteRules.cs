using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DAO.DAO;
using Model;
using Model.Entities;
using System.Net;
using System.IO;
using Model.Resultados;

namespace Rules.Rules.Reportes
{
    public class OrdenTrabajoReporteRules
    {
        private UsuarioLogueado data;

        protected UsuarioLogueado getUsuarioLogueado()
        {
            return data;
        }

        public OrdenTrabajoReporteRules(UsuarioLogueado data)
        {
            this.data = data;
        }


        /*Nuevos Metodos para reportes*/
        public Result<Telerik.Reporting.Report> GenerarReporteListadoOrdenes(List<int> ids, string filtros)
        {
            var result = new Result<Telerik.Reporting.Report>();
            try
            {

                var resultData = OrdenTrabajoDAO.Instance.GetResultadoTablaByIds(int.MaxValue, ids);
                if (resultData.Return == null || !resultData.Ok)
                {
                    result.AddErrorInterno("No existe el reporte del requerimiento");
                    return result;
                }


                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/OrdenTrabajo/Reporte_OrdenTrabajoListado.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();

                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderRequerimiento = System.Xml.XmlReader.Create(rutaReporte, settings);

                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerRequerimiento = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerRequerimiento.Deserialize(xmlReaderRequerimiento);

                Telerik.Reporting.ObjectDataSource objectDataSourceRequerimiento = new Telerik.Reporting.ObjectDataSource();

                DataTable tabla = new DataTable();
                tabla.Columns.Add("Numero");
                tabla.Columns.Add("Fecha");

                tabla.Columns.Add("Estado");
                tabla.Columns.Add("Filtros");

                tabla.Columns.Add("Informacion");

                tabla.Columns.Add("IdArea");
                tabla.Columns.Add("Area");

                foreach (var o in resultData.Return.Data)
                {
                    DataRow fila = tabla.NewRow();
                    fila["Filtros"] = filtros;
                    fila["Numero"] = (o.Numero);
                    fila["Fecha"] = o.FechaAlta.ToString("dd/MM/yyyy");
                    fila["Estado"] = Utils.toTitleCase(o.EstadoNombre);


                    //Agregar una columna con info de la orden
                    /*string informacion = null;
                
                
                    fila["Informacion"] = informacion;
                    */
                    fila["Area"] = Utils.toTitleCase(o.AreaNombre);
                    tabla.Rows.Add(fila);
                }

                objectDataSourceRequerimiento.DataSource = tabla;
                reporte.DataSource = objectDataSourceRequerimiento;
                result.Return = reporte;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }

            return result;
        }

        public Result<Telerik.Reporting.Report> GenerarReporteOrdenTrabajoListadoRequerimientos(Resultado_OrdenTrabajoDetalle orden)
        {

            var result = new Result<Telerik.Reporting.Report>();

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/OrdenTrabajo/ResumenOT/Reporte_OrdenEncabezado.trdx");

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);

                //---------------------------------
                // Defino el DT
                //---------------------------------

                Telerik.Reporting.ObjectDataSource objectDataSourceOrden = new Telerik.Reporting.ObjectDataSource();
                DataTable dtOrden = new DataTable();

                //Lado izq del reporte
                dtOrden.Columns.Add("Numero");
                dtOrden.Columns.Add("Area");
                dtOrden.Columns.Add("Zona");
                dtOrden.Columns.Add("Seccion");
                dtOrden.Columns.Add("Descripcion");

                //Indicadores             
                dtOrden.Columns.Add("EstadoActual");
                dtOrden.Columns.Add("CantidadRecursos");

                //Detalle
                dtOrden.Columns.Add("Motivo");

                //Recursos Adicionales
                dtOrden.Columns.Add("Materiales");
                dtOrden.Columns.Add("Personal");
                dtOrden.Columns.Add("Observaciones");




                //-------------------------------------
                // Creo una fila y le cargo los datos
                //-------------------------------------

                DataRow filaOrden = dtOrden.NewRow();

                //****************Encabezado*********************************

                //Numero
                string numero = orden.Numero + "/" + orden.Año;
                if (string.IsNullOrEmpty(numero))
                {
                    numero = "Sin Datos";
                }
                filaOrden["Numero"] = numero;

                //Area
                string area = orden.AreaNombre;
                if (string.IsNullOrEmpty(area))
                {
                    area = "Sin Datos";
                }
                filaOrden["Area"] = Utils.toTitleCase(area);


                //Descripcion
                string descripcion = orden.Descripcion;
                if (string.IsNullOrEmpty(descripcion))
                {
                    descripcion = "Sin Datos";
                }
                filaOrden["Descripcion"] = descripcion;

                //Estado
                string estado = orden.EstadoNombre;
                if (string.IsNullOrEmpty(estado))
                {
                    estado = "Sin Datos";
                }
                filaOrden["EstadoActual"] = Utils.toTitleCase(estado);

                //Zonas                       
                List<string> zonas = new List<string>();
                string zonasString = "";
                if (orden.Barrios.Count > 0)
                {
                    foreach (var b in orden.Barrios)
                    {
                        if (!zonas.Contains(b.ZonaNombre))
                        {
                            zonas.Add(b.ZonaNombre);
                        }
                    }
                    zonasString = string.Join(", ", zonas);
                }
                else
                {
                    zonasString = "Sin zonas registradas";
                }
                filaOrden["Zona"] = Utils.toTitleCase(zonasString);

                //Seccion
                string seccion = "";
                if (string.IsNullOrEmpty(seccion))
                {
                    seccion = "Sin Datos";
                }
                filaOrden["Seccion"] = seccion;


                //Recursos Cant
                int cantidadRecursos = orden.Moviles.Count;
                if (cantidadRecursos == null)
                {
                    cantidadRecursos = 0;
                }
                filaOrden["CantidadRecursos"] = cantidadRecursos;

                //Materiales
                string materiales = orden.RecursoMaterial;
                if (string.IsNullOrEmpty(materiales))
                {
                    materiales = "Sin Datos";
                }
                filaOrden["Materiales"] = materiales;

                //Personal
                string personal = orden.RecursoPersonal;
                if (string.IsNullOrEmpty(personal))
                {
                    personal = "Sin Datos";
                }
                filaOrden["Personal"] = personal;




                //*********************Tabla Estados***********************                
                DataTable dtEstados = new DataTable();
                dtEstados.Columns.Add("EstadoNombre");
                dtEstados.Columns.Add("EstadoObservaciones");
                dtEstados.Columns.Add("UsuarioNombreApellido");
                dtEstados.Columns.Add("EstadoFecha");



                foreach (var e in orden.Estados)
                {
                    DataRow filaEstados = dtEstados.NewRow();
                    filaEstados["EstadoNombre"] = Utils.toTitleCase(e.EstadoNombre);
                    filaEstados["EstadoObservaciones"] = e.EstadoObservaciones;
                    filaEstados["UsuarioNombreApellido"] = e.UsuarioNombre + " " + e.UsuarioApellido;
                    filaEstados["EstadoFecha"] = e.EstadoFecha.Value.ToString("dd/MM/yyyy");
                    dtEstados.Rows.Add(filaEstados);
                }
                Telerik.Reporting.Table tpEstados = (Telerik.Reporting.Table)(reporte.Items.Find("tablaEstados", true)[0]);
                tpEstados.DataSource = dtEstados;


                //*********************Tabla Notas***********************                
                DataTable dtNotas = new DataTable();
                dtNotas.Columns.Add("NotaFecha");
                dtNotas.Columns.Add("NotaDescripcion");

                foreach (var n in orden.Notas)
                {
                    DataRow filaNotas = dtNotas.NewRow();
                    filaNotas["NotaFecha"] = n.Fecha.Value.ToString("dd/MM/yyyy");
                    filaNotas["NotaDescripcion"] = n.Observaciones;
                    dtNotas.Rows.Add(filaNotas);
                }
                Telerik.Reporting.Table tpNotas = (Telerik.Reporting.Table)(reporte.Items.Find("tablaNotas", true)[0]);
                tpNotas.DataSource = dtNotas;


                Telerik.Reporting.SubReport subReportItem = (Telerik.Reporting.SubReport)(reporte.Items.Find("subreporteRqs", true)[0]);
                subReportItem.ReportSource = crearSubreporte(orden).Return;


                //Agrego la fila del requerimiento
                dtOrden.Rows.Add(filaOrden);

                //Seteo el datasource al reclamo
                objectDataSourceOrden.DataSource = dtOrden;
                reporte.DataSource = objectDataSourceOrden;

                result.Return = reporte;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }

            return result;
        }

        /*Crea el detalle con el listado de los reclamos*/
        public Result<Telerik.Reporting.Report> crearSubreporte(Resultado_OrdenTrabajoDetalle orden)
        {
            var result = new Result<Telerik.Reporting.Report>();

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/OrdenTrabajo/ResumenOT/Reporte_OrdenDetalleListadoRequerimientos.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();

                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);

                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();

                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);

                Telerik.Reporting.ObjectDataSource objectDataSourceOrden = new Telerik.Reporting.ObjectDataSource();

                //*********************Tabla Rqs***********************                
                DataTable dtRequerimientos = new DataTable();
                dtRequerimientos.Columns.Add("MotivoNombre");
                dtRequerimientos.Columns.Add("NumeroRequerimiento");
                dtRequerimientos.Columns.Add("Descripcion");
                dtRequerimientos.Columns.Add("Direccion");
                dtRequerimientos.Columns.Add("IdMotivo");



                foreach (var r in orden.Requerimientos)
                {
                    DataRow filaRequerimiento = dtRequerimientos.NewRow();
                    filaRequerimiento["MotivoNombre"] = Utils.toTitleCase(r.MotivoNombre);
                    filaRequerimiento["NumeroRequerimiento"] = r.Numero + "/" + r.Año;
                    filaRequerimiento["Descripcion"] = r.Descripcion;

                    //Ubicacion del rq
                    string direccion = null;

                    //Cpc
                    if (!string.IsNullOrEmpty(r.BarrioNombre))
                    {
                        if (!string.IsNullOrEmpty(direccion)) direccion += " | ";
                        direccion += "CPC: N° " + r.CpcNumero + " " + r.CpcNombre;
                    }

                    //Barrio
                    if (!string.IsNullOrEmpty(r.BarrioNombre))
                    {
                        if (!string.IsNullOrEmpty(direccion)) direccion += " | ";
                        direccion += "Barrio: " + r.BarrioNombre;
                    }

                    //Direccion
                    if (!string.IsNullOrEmpty(r.DomicilioDireccion))
                    {
                        if (!string.IsNullOrEmpty(direccion)) direccion += " | ";
                        direccion += "Dirección: " + r.DomicilioDireccion;
                    }

                    //Obs
                    if (!string.IsNullOrEmpty(r.DomicilioObservaciones))
                    {
                        if (!string.IsNullOrEmpty(direccion)) direccion += " | ";
                        direccion += "Descripción: " + r.DomicilioObservaciones;
                    }
                    filaRequerimiento["Direccion"] = direccion;

                    filaRequerimiento["IdMotivo"] = r.MotivoId;
                    dtRequerimientos.Rows.Add(filaRequerimiento);
                }

                objectDataSourceOrden.DataSource = dtRequerimientos;
                reporte.DataSource = objectDataSourceOrden;
                result.Return = reporte;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        /*Imprimir la OT con una hoja por reclamo*/
        public Result<Telerik.Reporting.ReportBook> GenerarReporteOrdenTrabajoDetallado(Resultado_OrdenTrabajoDetalle orden, bool conMapa)
        {
            var result = new Result<Telerik.Reporting.ReportBook>();

            try
            {
                Telerik.Reporting.ReportBook book = new Telerik.Reporting.ReportBook();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;


                /*Corresponde al encabezado */
                var resultadoReporteEncabezado = GenerarReporteOrdenTrabajoListadoRequerimientos(orden);
                if (!resultadoReporteEncabezado.Ok)
                {
                    result.Copy(resultadoReporteEncabezado.Errores);
                    return result;
                }

                if (resultadoReporteEncabezado.Return == null)
                {
                    result.AddErrorInterno("No existe la primera pagina del reporte");
                    return result;
                }

                book.Reports.Add(resultadoReporteEncabezado.Return);

                //Itero Los requerimientos
                var requerimientoReporteRules = new RequerimientoReporteRules(getUsuarioLogueado());

                foreach (var rqxot in orden.Requerimientos)
                {
                    var resultadoConsultaRq = new RequerimientoRules(getUsuarioLogueado()).GetDetalleById(rqxot.Id);
                    if (!resultadoConsultaRq.Ok)
                    {
                        result.Copy(resultadoConsultaRq.Errores);
                        return result;
                    }

                    if (resultadoConsultaRq.Return == null)
                    {
                        result.AddErrorInterno("No existe el reporte del requerimiento");
                        return result;
                    }

                    var rq = resultadoConsultaRq.Return;

                    //crear un reporte por cada requerimiento y setearselo al book
                    var resultadoReporteRequerimiento = requerimientoReporteRules.GenerarReporteRequerimientoParaOT(rq, true);
                    if (!resultadoReporteRequerimiento.Ok)
                    {
                        result.Copy(resultadoReporteRequerimiento.Errores);
                        return result;
                    }

                    if (resultadoReporteRequerimiento.Return == null)
                    {
                        result.AddErrorPublico("Error procesando la solicitud");
                        return result;
                    }

                    book.Reports.Add(resultadoReporteRequerimiento.Return);
                }

                result.Return = book;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

    }















}

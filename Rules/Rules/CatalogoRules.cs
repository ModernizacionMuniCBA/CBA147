using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using System.Web;
using Model.Consultas;
using Model.Resultados.Estadisticas;
using System.Data;

namespace Rules.Rules
{
    public class CatalogoRules : BaseRules<BaseEntity>
    {

        private readonly CatalogoDAO dao;

        public CatalogoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CatalogoDAO.Instance;
        }
              
        public Result<Telerik.Reporting.Report> GenerarReporteCatalogoUsuarios(int idArea, Resultado_InformacionOrganica organica)
        {
            var result = new Result<Telerik.Reporting.Report>();
            try
            {

                var resultData = dao.GetDatosCatalogoUsuarios(idArea);
                if (resultData.Return == null || !resultData.Ok)
                {
                    result.AddErrorInterno("No existe el reporte");
                    return result;
                }
                
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Catalogos/Reporte_CatalogoUsuarios.trdx");
                
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();

                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderCatalogos = System.Xml.XmlReader.Create(rutaReporte, settings);

                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerCatalogo = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerCatalogo.Deserialize(xmlReaderCatalogos);

                Telerik.Reporting.ObjectDataSource objectDataSourceCatalogo = new Telerik.Reporting.ObjectDataSource();

                DataTable tabla = new DataTable();
                tabla.Columns.Add("InfoOrganica");
                
                var secretaria = organica.Direccion.Secretaria.Nombre;
                var direccion = organica.Direccion.Nombre;
                var area = organica.Area.Nombre;
                
                string infoOrganica = null;
                
                if (!string.IsNullOrEmpty(secretaria))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Secretaría: " + secretaria;
                }

                if (!string.IsNullOrEmpty(direccion))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Dirección: " + direccion;
                }

                if (!string.IsNullOrEmpty(area))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Area: " + area;
                }
                
                Telerik.Reporting.TextBox txtInfoOrganica = reporte.Items.Find("txtInfoOrganica", true)[0] as Telerik.Reporting.TextBox;
                txtInfoOrganica.Value = infoOrganica;
                
                tabla.Columns.Add("Apellido");
                tabla.Columns.Add("Nombre");

                tabla.Columns.Add("Dni");
                tabla.Columns.Add("Usuario");

                tabla.Columns.Add("Rol");

                tabla.Columns.Add("Email");
                tabla.Columns.Add("Telefono");
                tabla.Columns.Add("Ubicacion");

                foreach (Resultado_CatalogoUsuarios c in resultData.Return)
                {
                    DataRow fila = tabla.NewRow();
                    fila["apellido"] = c.Apellido;
                    fila["nombre"] = c.Nombre;
                    fila["dni"] = c.Dni;
                    fila["usuario"] = c.Usuario;
                    fila["rol"] = c.Rol;
                    fila["email"] = c.Email;
                    fila["telefono"] = c.Telefono;
                    if (c.Ubicacion.ToLower() == "área operativa")
                    {
                        fila["ubicacion"] = "Ciudad";
                    }
                    else
                    {
                        fila["ubicacion"] = c.Ubicacion;
                    }
                    
                    tabla.Rows.Add(fila);
                }

                objectDataSourceCatalogo.DataSource = tabla;
                reporte.DataSource = objectDataSourceCatalogo;
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
        public Result<Telerik.Reporting.Report> GenerarReporteCatalogoMotivos(int idArea, Resultado_InformacionOrganica organica)
        {
            var result = new Result<Telerik.Reporting.Report>();
            try
            {

                var resultData = dao.GetDatosCatalogoMotivos(idArea);
                if (resultData.Return == null || !resultData.Ok)
                {
                    result.AddErrorInterno("No existe el reporte");
                    return result;
                }
                
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Catalogos/Reporte_CatalogoMotivos.trdx");

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();

                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderCatalogos = System.Xml.XmlReader.Create(rutaReporte, settings);

                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerCatalogo = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerCatalogo.Deserialize(xmlReaderCatalogos);

                Telerik.Reporting.ObjectDataSource objectDataSourceCatalogo = new Telerik.Reporting.ObjectDataSource();

                DataTable tabla = new DataTable();
                tabla.Columns.Add("InfoOrganica");
                var secretaria = organica.Direccion.Secretaria.Nombre;
                var direccion = organica.Direccion.Nombre;
                var area = organica.Area.Nombre;

                string infoOrganica = null;

                if (!string.IsNullOrEmpty(secretaria))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Secretaría: " + secretaria;
                }

                if (!string.IsNullOrEmpty(direccion))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Dirección: " + direccion;
                }

                if (!string.IsNullOrEmpty(area))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Area: " + area;
                }

                Telerik.Reporting.TextBox txtInfoOrganica = reporte.Items.Find("txtInfoOrganica", true)[0] as Telerik.Reporting.TextBox;
                txtInfoOrganica.Value = infoOrganica;

                tabla.Columns.Add("Nombre");
                tabla.Columns.Add("Peligroso");
                tabla.Columns.Add("Tipo");
                tabla.Columns.Add("Destacado");
                tabla.Columns.Add("Criticidad");

                tabla.Columns.Add("idCategoria");
                tabla.Columns.Add("Categoria");


                
                foreach (Resultado_CatalogoMotivos m in resultData.Return)
                {
                    DataRow fila = tabla.NewRow();
              
                    fila["nombre"] = m.Nombre;
                    var peligroso = "";
                    if (m.Urgente == true)
                    {
                        peligroso = "Si";
                    }
                    else { peligroso = "No"; }

                    fila["peligroso"] = peligroso;

                    var tipo = "";
                    switch (m.Tipo)
                    {
                        case(Enums.TipoMotivo.GENERAL):
                            tipo = "General";
                            break;
                        case (Enums.TipoMotivo.INTERNO):
                            tipo = "Interno";
                            break;
                        case (Enums.TipoMotivo.PRIVADO):
                            tipo = "Privado";
                            break;

                    }
                    fila["Tipo"] = tipo;

                    var principal = "";
                    if (m.Principal == true)
                    {
                        principal = "Si";
                    }
                    else { principal = "No"; }
                    fila["destacado"] = principal;

                    var prioridad = "";
                    switch(m.Prioridad){
                        case 1:
                            prioridad = "Normal";
                            break;
                        case 2:
                            prioridad = "Media";
                            break;
                        case 3:
                            prioridad = "Alta";
                            break;
                    }
                    fila["criticidad"] = prioridad;

                    if (m.Categoria != null)
                    {
                        fila["Categoria"] = Utils.toTitleCase(m.Categoria);
                    }
                   
                    
                    tabla.Rows.Add(fila);
                }

                objectDataSourceCatalogo.DataSource = tabla;
                reporte.DataSource = objectDataSourceCatalogo;
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
        public Result<Telerik.Reporting.Report> GenerarReporteCatalogoTareas(int idArea, Resultado_InformacionOrganica organica)
        {
            var result = new Result<Telerik.Reporting.Report>();
            try
            {

                var resultData = dao.GetDatosCatalogoTareas(idArea);
                if (resultData.Return == null || !resultData.Ok || resultData.Return.Count == 0)
                {
                    result.AddErrorInterno("Tareas no definidas");
                    return result;
                }


                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Catalogos/Reporte_CatalogoTareas.trdx");

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();

                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderCatalogos = System.Xml.XmlReader.Create(rutaReporte, settings);

                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerCatalogo = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerCatalogo.Deserialize(xmlReaderCatalogos);

                Telerik.Reporting.ObjectDataSource objectDataSourceCatalogo = new Telerik.Reporting.ObjectDataSource();

                DataTable tabla = new DataTable();
                tabla.Columns.Add("InfoOrganica");

                var secretaria = organica.Direccion.Secretaria.Nombre;
                var direccion = organica.Direccion.Nombre;
                var area = organica.Area.Nombre;

                string infoOrganica = null;

                if (!string.IsNullOrEmpty(secretaria))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Secretaría: " + secretaria;
                }

                if (!string.IsNullOrEmpty(direccion))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Dirección: " + direccion;
                }

                if (!string.IsNullOrEmpty(area))
                {
                    if (!string.IsNullOrEmpty(infoOrganica)) infoOrganica += " | ";
                    infoOrganica += "Area: " + area;
                }

                Telerik.Reporting.TextBox txtInfoOrganica = reporte.Items.Find("txtInfoOrganica", true)[0] as Telerik.Reporting.TextBox;
                txtInfoOrganica.Value = infoOrganica;
              


                tabla.Columns.Add("Nombre");
                tabla.Columns.Add("Descripcion");


                foreach (Resultado_CatalogoTareas t in resultData.Return)
                {
                    DataRow fila = tabla.NewRow();
                    fila["nombre"] = t.Nombre;
                    fila["descripcion"] = t.Observaciones;
                    tabla.Rows.Add(fila);
                }

                objectDataSourceCatalogo.DataSource = tabla;
                reporte.DataSource = objectDataSourceCatalogo;
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

        public Result<int> GetCantidadUsuariosByIdArea(int idArea)
        {
            return dao.GetCantidadUsuariosByIdArea(idArea);
        }
        public Result<int> GetCantidadMotivosByIdArea(int idArea)
        {
            return dao.GetCantidadMotivosByIdArea(idArea);
        }
        public Result<int> GetCantidadTareasByIdArea(int idArea)
        {
            return dao.GetCantidadTareasByIdArea(idArea);
        }

     



    }
}

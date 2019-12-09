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
using Model.Consultas;
using Model.Resultados;
using Model.Resultados.Estadisticas;

namespace Rules.Rules.Reportes
{
    public class EstadisticaReporteRules
    {
        private UsuarioLogueado data;

        protected UsuarioLogueado getUsuarioLogueado()
        {
            return data;
        }

        public EstadisticaReporteRules(UsuarioLogueado data)
        {
            this.data = data;
        }
        /*ESTADISTICAS V2*/
        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaCPC(string[] base64, List<Resultado_DatosEstadisticaPanel_Cpc> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaCPC.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);

   

                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();
                DataTable tablaGeneral = new DataTable();

                //Datos Generales

               // tablaGeneral.Columns.Add("Mapa");
             

                DataRow filaGeneral = tablaGeneral.NewRow();

               
                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }                


                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);


                #region
                //Estados
                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Nombre");
                dtGrilla.Columns.Add("Numero");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Nombre"] = Utils.toTitleCase(datos.Cpc.Nombre);
                    fila["Numero"] = datos.Cpc.Numero;
                    fila["Cantidad"] = datos.CantidadRequerimientos;
                    fila["Porcentaje"] = datos.Porcentaje +"%";
                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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

        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaOrigen(string[] base64, List<Resultado_DatosEstadisticaOrigen> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaOrigen.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }      



                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);
               
              


                #region
                
                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Origen");                
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Origen"] = Utils.toTitleCase(datos.Origen);          
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";

                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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

        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaEficacia(string[] base64, List<Resultado_DatosEstadisticaEficacia> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaEficacia.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }

                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Condicion");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Condicion"] = Utils.toTitleCase(datos.Condicion);
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";

                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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

        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaResueltos(string[] base64, List<Resultado_DatosEstadisticaResueltos> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaResueltos.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }

                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Etiqueta");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje"); 

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Etiqueta"] = datos.Etiqueta;           
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";
                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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
        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaServicios(string[] base64, List<Resultado_DatosEstadisticaServicios> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaServicios.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }



                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Servicio");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Servicio"] = Utils.toTitleCase(datos.Servicio);
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";

                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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
        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaArea(string[] base64, List<Resultado_DatosEstadisticaArea> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaArea.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }

                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Area");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Area"] = datos.Area;
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";
                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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
        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaSubArea(string[] base64, List<Resultado_DatosEstadisticaArea> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaSubArea.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }

                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Area");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Area"] = datos.Area;
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";
                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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

        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaZona(string[] base64, List<Resultado_DatosEstadisticaZona> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaZona.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }

                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Zona");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Zona"] = datos.Zona;
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";
                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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
        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaUsuario(string[] base64, List<Resultado_DatosEstadisticaUsuario> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaUsuario.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }

                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Usuario");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Usuario"] = datos.Usuario;
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";
                    dtGrilla.Rows.Add(fila);
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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

        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticaMotivos(string[] base64, List<Resultado_DatosEstadisticaMotivos> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaMotivos.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }



                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Motivo");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");
               
                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Motivo"] = Utils.toTitleCase(datos.Motivo);
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";
                    if (datos.Cantidad != 0)
                    {
                        dtGrilla.Rows.Add(fila);
                    }
                    
                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);                
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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
        public Result<Telerik.Reporting.Report> GenerarReporteEstadisticRubros(string[] base64, List<Resultado_DatosEstadisticaRubros> grilla, string htmlFiltros)
        {
            var result = new Result<Telerik.Reporting.Report>();


            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Estadisticas/EstadisticaRubros.trdx");
                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();

                //Deserialización para convertir el reporte trdx (xml) al objeto Report de Telerik
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                System.Xml.XmlReader xmlReaderOrden = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerOrden = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerOrden.Deserialize(xmlReaderOrden);



                //Data
                Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                DataTable tablaGeneral = new DataTable();

                //Datos Generales

                tablaGeneral.Columns.Add("Mapa");

                DataRow filaGeneral = tablaGeneral.NewRow();

                //Html texto
                if (!string.IsNullOrEmpty(htmlFiltros))
                {
                    Telerik.Reporting.HtmlTextBox html = reporte.Items.Find("htmlFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                    html.Value = htmlFiltros;
                }



                //GraficoBarra
                Telerik.Reporting.PictureBox pictureBarra = reporte.Items.Find("fotoGraficoBarra", true)[0] as Telerik.Reporting.PictureBox;
                var bytesBarra = Convert.FromBase64String(base64[0]);

                Image imagenChartBarra = null;
                using (var ms1 = new MemoryStream(bytesBarra))
                {
                    imagenChartBarra = Image.FromStream(ms1);
                }
                pictureBarra.Value = imagenChartBarra;

                //GraficoRadio
                Telerik.Reporting.PictureBox pictureRadio = reporte.Items.Find("fotoGraficoRadio", true)[0] as Telerik.Reporting.PictureBox;
                var bytesRadio = Convert.FromBase64String(base64[1]);

                Image imagenChartRadio = null;
                using (var ms2 = new MemoryStream(bytesRadio))
                {
                    imagenChartRadio = Image.FromStream(ms2);
                }
                pictureRadio.Value = imagenChartRadio;


                tablaGeneral.Rows.Add(filaGeneral);




                #region

                DataTable dtGrilla = new DataTable();
                dtGrilla.Columns.Add("Rubro");
                dtGrilla.Columns.Add("Cantidad");
                dtGrilla.Columns.Add("Porcentaje");

                foreach (var datos in grilla)
                {
                    DataRow fila = dtGrilla.NewRow();
                    fila["Rubro"] = Utils.toTitleCase(datos.Rubro);
                    fila["Cantidad"] = datos.Cantidad;
                    fila["Porcentaje"] = datos.Porcentaje + "%";
                    if (datos.Cantidad != 0)
                    {
                        dtGrilla.Rows.Add(fila);
                    }

                }
                Telerik.Reporting.Table tablaCpc = (Telerik.Reporting.Table)(reporte.Items.Find("tablaCpc", true)[0]);
                tablaCpc.DataSource = dtGrilla;
                #endregion



                objectDataSource.DataSource = tablaGeneral;
                reporte.DataSource = objectDataSource;

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
    }
}

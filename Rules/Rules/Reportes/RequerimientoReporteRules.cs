using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAO.DAO;
using Rules.Rules;
using Model;
using Model.Entities;
using System.Reflection.Emit;
using Telerik.Reporting.Processing;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Model.Resultados;
using System.Text.RegularExpressions;
using System.Security;
using System.Xml;
using System.Configuration;

namespace Rules.Rules.Reportes
{
    public class RequerimientoReporteRules
    {
        private UsuarioLogueado data;

        protected UsuarioLogueado getUsuarioLogueado()
        {
            return data;
        }

        public RequerimientoReporteRules(UsuarioLogueado data)
        {
            this.data = data;
        }

        /*NUEVOS METODS*/
        public Telerik.Reporting.Report GenerarReporteListadoRequerimientos(List<int> ids, string filtros)
        {
            var resultData = RequerimientoDAO.Instance.GetResultadoTablaByIds(int.MaxValue, ids);
            if (!resultData.Ok)
            {
                /*Reportar error*/
                return null;
            }

            var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Requerimiento/Reporte_RequerimientoListado.trdx");
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
            tabla.Columns.Add("Motivo");
            tabla.Columns.Add("Direccion");
            tabla.Columns.Add("Filtros");

            tabla.Columns.Add("IdArea");
            tabla.Columns.Add("Area");

            foreach (var r in resultData.Return.Data)
            {
                DataRow fila = tabla.NewRow();
                fila["Filtros"] = filtros;
                fila["Numero"] = (r.Numero + "/" + r.Año);
                fila["Fecha"] = r.FechaAlta.ToString("dd/MM/yyyy");
                fila["Estado"] = Utils.toTitleCase(r.EstadoNombre);
                fila["Motivo"] = Utils.toTitleCase(r.MotivoNombre);

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
                    direccion += "BARRIO: " + r.BarrioNombre;
                }

                //Direccion
                if (!string.IsNullOrEmpty(r.DomicilioDireccion))
                {
                    if (!string.IsNullOrEmpty(direccion)) direccion += " | ";
                    direccion += "DIRECCION: " + r.DomicilioDireccion;
                }

                //Obs
                if (!string.IsNullOrEmpty(r.DomicilioObservaciones))
                {
                    if (!string.IsNullOrEmpty(direccion)) direccion += " | ";
                    direccion += "DESCRIPCIÓN: " + r.DomicilioObservaciones;
                }

                fila["Direccion"] = direccion;

                fila["Area"] = Utils.toTitleCase(r.AreaNombre);
                tabla.Rows.Add(fila);
            }

            objectDataSourceRequerimiento.DataSource = tabla;
            reporte.DataSource = objectDataSourceRequerimiento;
            return reporte;


        }

        /*Reporte requerimiento V3*/
        public Result<Telerik.Reporting.Report> GenerarReporteRequerimientoConMapa2(Resultado_RequerimientoDetalle2 requerimiento, int? idUsuario, bool conMapa)
        {
            var result = new Result<Telerik.Reporting.Report>();

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Requerimiento/Reporte_RequerimientoDetalleV3.trdx");

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                System.Xml.XmlReader xmlReaderRequerimiento = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerRequerimiento = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerRequerimiento.Deserialize(xmlReaderRequerimiento);

                //---------------------------------
                // Defino el DT
                //---------------------------------

                Telerik.Reporting.ObjectDataSource objectDataSourceRequerimiento = new Telerik.Reporting.ObjectDataSource();
                DataTable dtRequerimiento = new DataTable();

                //Lado izq del reporte
                dtRequerimiento.Columns.Add("Numero");
                dtRequerimiento.Columns.Add("rqFechaCreacion");
                dtRequerimiento.Columns.Add("diasCreacion");
                dtRequerimiento.Columns.Add("Servicio");
                dtRequerimiento.Columns.Add("Motivo");
                dtRequerimiento.Columns.Add("Area");
                dtRequerimiento.Columns.Add("Descripcion");


                //Indicadores             
                dtRequerimiento.Columns.Add("EstadoActual");
                dtRequerimiento.Columns.Add("Prioridad");
                dtRequerimiento.Columns.Add("Peligroso");
                dtRequerimiento.Columns.Add("OrdenAtencionCritica");
                dtRequerimiento.Columns.Add("Marcado");
                dtRequerimiento.Columns.Add("Favorito");
                dtRequerimiento.Columns.Add("CantidadFotos");
                dtRequerimiento.Columns.Add("CantidadArchivos");

                //Ubicacion
                dtRequerimiento.Columns.Add("domicilioDireccion");
                dtRequerimiento.Columns.Add("domicilioSugerido");
                dtRequerimiento.Columns.Add("domicilioDistancia");
                dtRequerimiento.Columns.Add("domicilioObservacion");
                dtRequerimiento.Columns.Add("domicilioBarrio");
                dtRequerimiento.Columns.Add("cpcNumero");
                dtRequerimiento.Columns.Add("cpcNombre");

                //Estados Historial
                dtRequerimiento.Columns.Add("Estado");
                dtRequerimiento.Columns.Add("NumeroOT");
                dtRequerimiento.Columns.Add("UsuarioOT");
                dtRequerimiento.Columns.Add("FechaOT");

                //UsuarioReferente
                dtRequerimiento.Columns.Add("ApellidoReferente");
                dtRequerimiento.Columns.Add("NombreReferente");
                dtRequerimiento.Columns.Add("UsuarioReferente");
                dtRequerimiento.Columns.Add("DocumentoReferente");
                dtRequerimiento.Columns.Add("EmailReferente");

                //Info Adicional
                dtRequerimiento.Columns.Add("FechaCreacion");
                dtRequerimiento.Columns.Add("UsuarioCreador");
                dtRequerimiento.Columns.Add("Origen");
                dtRequerimiento.Columns.Add("FechaModificacion");
                dtRequerimiento.Columns.Add("UsuarioModificador");

                //-------------------------------------
                // Creo una fila y le cargo los datos
                //-------------------------------------

                DataRow filaRequerimiento = dtRequerimiento.NewRow();

                //****************Encabezado*********************************

                //Numero
                string numero = requerimiento.Numero + "/" + requerimiento.Año;
                if (string.IsNullOrEmpty(numero))
                {
                    numero = "Sin Datos";
                }
                filaRequerimiento["Numero"] = numero;

                //Fecha creacion
                string rqFechaCreacion = requerimiento.FechaAlta.ToString("dd-MM-yyyy");
                if (string.IsNullOrEmpty(rqFechaCreacion))
                {
                    rqFechaCreacion = "";
                }
                filaRequerimiento["rqFechaCreacion"] = rqFechaCreacion;

                //Dias creacion
                var diasConsulta = new RequerimientoRules(getUsuarioLogueado()).GetDiasDesdeCreacion((int)requerimiento.Id);
                if (!diasConsulta.Ok)
                {
                    result.Copy(diasConsulta.Errores);
                    result.AddErrorPublico("Error procesando la solicitud");
                    result.AddErrorInterno("Error consultando los dias");
                    return result;
                }

                string diasCreacion = diasConsulta.Return.ToString();
                if (string.IsNullOrEmpty(diasCreacion))
                {
                    diasCreacion = "";
                }
                filaRequerimiento["diasCreacion"] = diasCreacion;

                //Servicio
                string servicio = requerimiento.ServicioNombre;
                if (string.IsNullOrEmpty(servicio))
                {
                    servicio = "Sin Datos";
                }
                filaRequerimiento["Servicio"] = Utils.toTitleCase(servicio);

                //Motivo
                string motivo = requerimiento.MotivoNombre;
                if (string.IsNullOrEmpty(motivo))
                {
                    servicio = "Sin Datos";
                }
                filaRequerimiento["Motivo"] = Utils.toTitleCase(motivo);

                //Area
                string area = requerimiento.AreaNombre;
                if (string.IsNullOrEmpty(servicio))
                {
                    servicio = "Sin Datos";
                }
                filaRequerimiento["Area"] = area;

                //Estado
                string estado = requerimiento.EstadoNombre;
                if (string.IsNullOrEmpty(estado))
                {
                    servicio = "Sin Datos";
                }
                filaRequerimiento["EstadoActual"] = Utils.toTitleCase(estado);

                //Prioridad
                Enums.PrioridadRequerimiento prioridad = requerimiento.Prioridad;
                string textoPrioridad = "";

                if (prioridad == null)
                {
                    servicio = "Sin Datos";
                }
                else
                {
                    switch (prioridad)
                    {
                        case Enums.PrioridadRequerimiento.ALTA:
                            {
                                textoPrioridad = "Prioridad Alta";

                            } break;
                        case Enums.PrioridadRequerimiento.MEDIA:
                            {
                                textoPrioridad = "Prioridad Media";

                            } break;
                        case Enums.PrioridadRequerimiento.NORMAL:
                            {
                                textoPrioridad = "Prioridad Normal";
                            } break;
                    }
                }
                filaRequerimiento["Prioridad"] = textoPrioridad;

                //Peligroso
                string peligroso = "";
                if (requerimiento.Peligroso)
                {
                    peligroso = "Peligroso";
                }
                else
                {
                    peligroso = "No Peligroso";
                }
                filaRequerimiento["Peligroso"] = peligroso;

                //Orden atencion Critica
                string ordenAtencion = "";

                filaRequerimiento["OrdenAtencionCritica"] = ordenAtencion;

                //CPC
                string marcado = "";
                if (requerimiento.Marcado)
                {
                    marcado = "En control de CPC";
                }
                else
                {
                    marcado = "En control de Área Operativa";
                }
                filaRequerimiento["Marcado"] = marcado;

                //Favorito
                string favorito = "";
                if (requerimiento.Favorito)
                {
                    favorito = "Es favorito";
                }
                else
                {
                    favorito = "No es favorito";
                }
                filaRequerimiento["Favorito"] = favorito;

                //Descripcion
                string descripcion = requerimiento.Descripcion;
                if (string.IsNullOrEmpty(descripcion))
                {
                    descripcion = "Sin Datos";
                }
                filaRequerimiento["Descripcion"] = descripcion;

                //Adjuntos
                int? cantidadFotos = requerimiento.CantidadFotos;
                if (cantidadFotos == null)
                {
                    cantidadFotos = 0;
                }
                filaRequerimiento["CantidadFotos"] = cantidadFotos;


                int? cantidadArchivos = requerimiento.CantidadDocumentos;
                if (cantidadArchivos == null)
                {
                    cantidadArchivos = 0;
                }
                filaRequerimiento["CantidadArchivos"] = cantidadArchivos;

                //Ubicacion del rq
                string direccion = requerimiento.DomicilioDireccion;
                string observaciones = requerimiento.DomicilioObservaciones;
                if (string.IsNullOrEmpty(direccion))
                {
                    direccion = "";
                    if (string.IsNullOrEmpty(observaciones))
                    {
                        observaciones = "";
                    }
                    // direccion = direccion;
                }
                filaRequerimiento["domicilioDireccion"] = SecurityElement.Escape(direccion);
                filaRequerimiento["domicilioObservacion"] = observaciones;

                int? domicilioDistancia = requerimiento.DomicilioDistancia;
                bool? domicilioSugerido = requerimiento.DomicilioSugerido;

                filaRequerimiento["domicilioDistancia"] = domicilioDistancia;
                filaRequerimiento["domicilioSugerido"] = domicilioSugerido;

                string barrio = requerimiento.DomicilioBarrioNombre;
                if (string.IsNullOrEmpty(barrio))
                {
                    barrio = "";
                }
                filaRequerimiento["domicilioBarrio"] = Utils.toTitleCase(barrio);

                string cpcNombre = requerimiento.DomicilioCpcNombre;
                if (string.IsNullOrEmpty(cpcNombre))
                {
                    cpcNombre = "";
                }
                filaRequerimiento["cpcNombre"] = Utils.toTitleCase(cpcNombre);

                int? cpcNumero = requerimiento.DomicilioCpcNumero;
                if (cpcNumero == null)
                {
                    cpcNumero = 0;
                }
                filaRequerimiento["cpcNumero"] = cpcNumero;



                //******************Usuario Referente**********************
                var usuarioRef = requerimiento.UsuariosReferentes.Where(x => x.Id == idUsuario).FirstOrDefault();
                if (usuarioRef == null && requerimiento.UsuariosReferentes.Count > 0)
                {
                    usuarioRef = requerimiento.UsuariosReferentes[0];
                    idUsuario = requerimiento.UsuariosReferentes[0].Id;
                }
                /*SI NO TIENE USUARIO REF se le pone a todo sin datos*/
                var provisorioApellido = requerimiento.ReferenteProvisorioApellido;
                var provisorioNombre = requerimiento.ReferenteProvisorioNombre;
                var provisorioDni = requerimiento.ReferenteProvisorioDni;
                var provisorioGenero = requerimiento.ReferenteProvisorioGeneroMasculino;
                var provisorioObs = requerimiento.ReferenteProvisorioObservaciones;
                var provisorioTel = requerimiento.ReferenteProvisorioTelefono;

                if (requerimiento.UsuariosReferentes.Count == 0)
                {
                    filaRequerimiento["NombreReferente"] = "Sin Datos";
                    filaRequerimiento["ApellidoReferente"] = "Sin Datos";
                    filaRequerimiento["UsuarioReferente"] = "Sin Datos";
                    filaRequerimiento["DocumentoReferente"] = "Sin Datos";
                    filaRequerimiento["emailReferente"] = "Sin Datos";
                    /*este if es por el usuariuo provisorio para emergencia*/
                    if (provisorioDni != null)
                    {
                        Telerik.Reporting.TextBox txtUsuario = reporte.Items.Find("txtUsuario", true)[0] as Telerik.Reporting.TextBox;
                        txtUsuario.Value = "Usuario Provisorio";

                        if (string.IsNullOrEmpty(provisorioApellido))
                        {
                            filaRequerimiento["ApellidoReferente"] = "Sin Datos";
                        }
                        filaRequerimiento["ApellidoReferente"] = provisorioApellido;

                        if (string.IsNullOrEmpty(provisorioNombre))
                        {
                            filaRequerimiento["NombreReferente"] = "Sin Datos";
                        }
                        filaRequerimiento["NombreReferente"] = provisorioNombre;

                        if (string.IsNullOrEmpty(provisorioDni.ToString()))
                        {
                            filaRequerimiento["DocumentoReferente"] = "Sin Datos";
                        }
                        filaRequerimiento["DocumentoReferente"] = provisorioDni;
                        /*aca cambio el mail por el telefono*/
                        Telerik.Reporting.TextBox txtMail = reporte.Items.Find("txtMail", true)[0] as Telerik.Reporting.TextBox;
                        txtMail.Value = "Tel:";
                        if (string.IsNullOrEmpty(provisorioTel.ToString()))
                        {
                            filaRequerimiento["emailReferente"] = "Sin Datos";
                        }
                        filaRequerimiento["emailReferente"] = provisorioTel;

                        /*aca cambio el usuario por el genero*/
                        Telerik.Reporting.TextBox txtUsr = reporte.Items.Find("txtUsr", true)[0] as Telerik.Reporting.TextBox;
                        txtUsr.Value = "Género: ";
                        if (string.IsNullOrEmpty(provisorioGenero.ToString()))
                        {
                            filaRequerimiento["UsuarioReferente"] = "Sin Datos";
                        }
                        var textoGenero = "";
                        if (provisorioGenero = true)
                        {
                            textoGenero = "Masculino";
                        }
                        else
                        {
                            textoGenero = "Femenino";
                        }

                        filaRequerimiento["UsuarioReferente"] = textoGenero;







                    }


                }
                else
                {
                    int? v1 = idUsuario;
                    int v2 = v1 ?? default(int);
                    var usuarioLogeado = getUsuarioLogueado();

                    //Usuario Fisico
                    var resultUsuario = new BaseRules<_VecinoVirtualUsuario>(usuarioLogeado).GetByIdObligatorio(v2);

                    //if (!resultUsuario.Ok)
                    //{
                    //    resultado.Add("Error", resultadoConsulta.ToStringPublico());
                    //    InitJs(resultado);
                    //    return null;
                    //}         
                    var usuario = new _Resultado_VecinoVirtualUsuario(resultUsuario.Return);



                    //if (resultUsuario.Return.AmbitoTrabajo != null && resultUsuario.Return != null)
                    //{
                    //    usuario.Ambito = new Resultado_Ambito(resultUsuario.Return.AmbitoTrabajo);
                    //}

                    string nombreReferente = usuarioRef.Nombre;
                    if (string.IsNullOrEmpty(nombreReferente))
                    {
                        nombreReferente = "Sin Datos";
                    }
                    filaRequerimiento["NombreReferente"] = nombreReferente;

                    string apellidoReferente = usuarioRef.Apellido;
                    if (string.IsNullOrEmpty(apellidoReferente))
                    {
                        apellidoReferente = "Sin Datos";
                    }
                    filaRequerimiento["ApellidoReferente"] = apellidoReferente;

                    string usuarioReferente = usuarioRef.Username;
                    if (string.IsNullOrEmpty(usuarioReferente))
                    {
                        usuarioReferente = "Sin Datos";
                    }
                    filaRequerimiento["UsuarioReferente"] = usuarioReferente;

                    int documentoReferente = usuario.Dni;
                    if (documentoReferente == 0)
                    {
                        usuarioReferente = "Sin Datos";
                    }
                    filaRequerimiento["DocumentoReferente"] = documentoReferente;

                    string emailReferente = usuario.Email;
                    if (string.IsNullOrEmpty(emailReferente))
                    {
                        usuarioReferente = "Sin Datos";
                    }
                    filaRequerimiento["emailReferente"] = emailReferente;

                }





                //***************Informacion Adicional**************

                //Creacion


                string fechaCreacion = "";
                if (requerimiento.FechaAlta != null)
                {
                    fechaCreacion = requerimiento.FechaAlta.ToString("dd/MM/yyyy HH:mm:ss");
                }
                else { fechaCreacion = "sin datos"; }
                filaRequerimiento["FechaCreacion"] = fechaCreacion;

                string usuarioCreador = requerimiento.UsuarioCreadorNombre + " " + requerimiento.UsuarioCreadorApellido;
                if (string.IsNullOrEmpty(usuarioCreador))
                {
                    usuarioCreador = "Sin Datos";
                }
                //filaRequerimiento["UsuarioCreador"] = "#CBA147";


                string origen = requerimiento.OrigenNombre;
                if (string.IsNullOrEmpty(origen))
                {
                    origen = "Sin Datos";
                }
                filaRequerimiento["Origen"] = origen;

                //Modificacion

                string fechaModificacion = "";
                string usuarioModificacionNombre = requerimiento.UsuarioModificacionNombre;
                string usuarioModificacionApellido = requerimiento.UsuarioModificacionApellido;
                Telerik.Reporting.TextBox txtModificar = reporte.Items.Find("txtModificador", true)[0] as Telerik.Reporting.TextBox;

                if (requerimiento.FechaModificacion != null)
                {
                    fechaModificacion = requerimiento.FechaModificacion.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    if (!string.IsNullOrEmpty(usuarioModificacionNombre))
                    {
                        usuarioModificacionNombre = requerimiento.UsuarioModificacionNombre + " " + requerimiento.UsuarioModificacionApellido;
                    }
                    else
                    {
                        txtModificar.Visible = false;
                        /*hide*/
                    }

                }
                else
                {
                    txtModificar.Visible = false;

                }
                filaRequerimiento["FechaModificacion"] = fechaModificacion;
                filaRequerimiento["UsuarioModificador"] = usuarioModificacionNombre;



                //*********************Tabla Estados***********************                
                DataTable dtEstados = new DataTable();
                dtEstados.Columns.Add("EstadoNombre");
                dtEstados.Columns.Add("EstadoObservaciones");
                dtEstados.Columns.Add("UsuarioNombreApellido");
                dtEstados.Columns.Add("EstadoFecha");



                foreach (var e in requerimiento.Estados)
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

                var url1 = "https://maps.googleapis.com/maps/api/staticmap?autoscale=2&size=600x600&maptype=roadmap&format=png&visual_refresh=true&markers=size:mid%7Ccolor:0xff0000%7Clabel:%7C&key=" + ConfigurationManager.AppSettings["KEY_GOOGLE_MAPS"] ;
                var marcadores = "";

                //Mapa
                if (conMapa)
                {
                    var x = requerimiento.DomicilioLatitud.Replace(",", ".");
                    var y = requerimiento.DomicilioLongitud.Replace(",", ".");

                    marcadores = x + "," + y;
                    url1 += "&markers=" + marcadores;

                    Telerik.Reporting.TextBox txtMapa = reporte.Items.Find("txtMapa", true)[0] as Telerik.Reporting.TextBox;
                    txtMapa.Visible = true;
                    Telerik.Reporting.PictureBox picture = reporte.Items.Find("imgMapa", true)[0] as Telerik.Reporting.PictureBox;
                    picture.Value = url1;
                    picture.Visible = true;
                }

                //Agrego la fila del requerimiento
                dtRequerimiento.Rows.Add(filaRequerimiento);

                //Seteo el datasource al reclamo
                objectDataSourceRequerimiento.DataSource = dtRequerimiento;
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

        public Result<Telerik.Reporting.Report> GenerarReporteRequerimientoParaOT(Resultado_RequerimientoDetalle2 requerimiento, bool conMapa)
        {
            var result = new Result<Telerik.Reporting.Report>();

            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/OrdenTrabajo/DetalladaOT/Reporte_RequerimientoDetalleParaOT.trdx");

                Telerik.Reporting.Report reporte = new Telerik.Reporting.Report();
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                System.Xml.XmlReader xmlReaderRequerimiento = System.Xml.XmlReader.Create(rutaReporte, settings);
                Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializerRequerimiento = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                reporte = (Telerik.Reporting.Report)xmlSerializerRequerimiento.Deserialize(xmlReaderRequerimiento);

                //---------------------------------
                // Defino el DT
                //---------------------------------

                Telerik.Reporting.ObjectDataSource objectDataSourceRequerimiento = new Telerik.Reporting.ObjectDataSource();
                DataTable dtRequerimiento = new DataTable();

                //Lado izq del reporte
                dtRequerimiento.Columns.Add("Numero");
                dtRequerimiento.Columns.Add("rqFechaCreacion");
                dtRequerimiento.Columns.Add("diasCreacion");
                dtRequerimiento.Columns.Add("Servicio");
                dtRequerimiento.Columns.Add("Motivo");
                dtRequerimiento.Columns.Add("Area");
                dtRequerimiento.Columns.Add("Descripcion");


                //Indicadores             
                dtRequerimiento.Columns.Add("EstadoActual");
                dtRequerimiento.Columns.Add("Prioridad");
                dtRequerimiento.Columns.Add("Peligroso");
                dtRequerimiento.Columns.Add("OrdenAtencionCritica");
                dtRequerimiento.Columns.Add("Marcado");
                dtRequerimiento.Columns.Add("Favorito");
                dtRequerimiento.Columns.Add("CantidadFotos");
                dtRequerimiento.Columns.Add("CantidadArchivos");

                //Ubicacion
                dtRequerimiento.Columns.Add("domicilioDireccion");
                dtRequerimiento.Columns.Add("domicilioObservacion");
                dtRequerimiento.Columns.Add("domicilioBarrio");
                dtRequerimiento.Columns.Add("cpcNumero");
                dtRequerimiento.Columns.Add("cpcNombre");



                //-------------------------------------
                // Creo una fila y le cargo los datos
                //-------------------------------------

                DataRow filaRequerimiento = dtRequerimiento.NewRow();

                //****************Encabezado*********************************

                //Numero
                string numero = requerimiento.Numero + "/" + requerimiento.Año;
                if (string.IsNullOrEmpty(numero))
                {
                    numero = "Sin Datos";
                }
                filaRequerimiento["Numero"] = numero;

                //Fecha creacion
                string rqFechaCreacion = requerimiento.FechaAlta.ToString("dd-MM-yyyy");
                if (string.IsNullOrEmpty(rqFechaCreacion))
                {
                    rqFechaCreacion = "";
                }
                filaRequerimiento["rqFechaCreacion"] = rqFechaCreacion;

                //Dias creacion
                var diasConsulta = new RequerimientoRules(getUsuarioLogueado()).GetDiasDesdeCreacion((int)requerimiento.Id);
                if (!diasConsulta.Ok)
                {
                    result.Copy(diasConsulta.Errores);
                    result.AddErrorPublico("Error procesando la solicitud");
                    result.AddErrorInterno("Error consultando los dias");
                    return result;
                }

                string diasCreacion = diasConsulta.Return.ToString();
                if (string.IsNullOrEmpty(diasCreacion))
                {
                    diasCreacion = "";
                }
                filaRequerimiento["diasCreacion"] = diasCreacion;

                //Servicio
                string servicio = requerimiento.ServicioNombre;
                if (string.IsNullOrEmpty(servicio))
                {
                    servicio = "Sin Datos";
                }
                filaRequerimiento["Servicio"] = Utils.toTitleCase(servicio);

                //Motivo
                string motivo = requerimiento.MotivoNombre;
                if (string.IsNullOrEmpty(motivo))
                {
                    servicio = "Sin Datos";
                }
                filaRequerimiento["Motivo"] = Utils.toTitleCase(motivo);

                //Area
                string area = requerimiento.AreaNombre;
                if (string.IsNullOrEmpty(servicio))
                {
                    servicio = "Sin Datos";
                }
                filaRequerimiento["Area"] = area;

                //Estado
                string estado = requerimiento.EstadoNombre;
                if (string.IsNullOrEmpty(estado))
                {
                    servicio = "Sin Datos";
                }
                filaRequerimiento["EstadoActual"] = Utils.toTitleCase(estado);

                //Prioridad
                Enums.PrioridadRequerimiento prioridad = requerimiento.Prioridad;
                string textoPrioridad = "";

                if (prioridad == null)
                {
                    servicio = "Sin Datos";
                }
                else
                {
                    switch (prioridad)
                    {
                        case Enums.PrioridadRequerimiento.ALTA:
                            {
                                textoPrioridad = "Prioridad Alta";

                            } break;
                        case Enums.PrioridadRequerimiento.MEDIA:
                            {
                                textoPrioridad = "Prioridad Media";

                            } break;
                        case Enums.PrioridadRequerimiento.NORMAL:
                            {
                                textoPrioridad = "Prioridad Normal";
                            } break;
                    }
                }
                filaRequerimiento["Prioridad"] = textoPrioridad;

                //Peligroso
                string peligroso = "";
                if (requerimiento.Peligroso)
                {
                    peligroso = "Peligroso";
                }
                else
                {
                    peligroso = "No Peligroso";
                }
                filaRequerimiento["Peligroso"] = peligroso;

                //Orden atencion Critica
                string ordenAtencion = "";
                filaRequerimiento["OrdenAtencionCritica"] = ordenAtencion;

                //CPC
                string marcado = "";
                if (requerimiento.Marcado)
                {
                    marcado = "En control de CPC";
                }
                else
                {
                    marcado = "En control de Área Operativa";
                }
                filaRequerimiento["Marcado"] = marcado;

                //Favorito
                string favorito = "";
                if (requerimiento.Favorito)
                {
                    favorito = "Es favorito";
                }
                else
                {
                    favorito = "No es favorito";
                }
                filaRequerimiento["Favorito"] = favorito;

                //Descripcion
                string descripcion = requerimiento.Descripcion;
                if (string.IsNullOrEmpty(descripcion))
                {
                    descripcion = "Sin Datos";
                }
                filaRequerimiento["Descripcion"] = descripcion;

                //Adjuntos
                int? cantidadFotos = requerimiento.CantidadFotos;
                if (cantidadFotos == null)
                {
                    cantidadFotos = 0;
                }
                filaRequerimiento["CantidadFotos"] = cantidadFotos;


                int? cantidadArchivos = requerimiento.CantidadDocumentos;
                if (cantidadArchivos == null)
                {
                    cantidadArchivos = 0;
                }
                filaRequerimiento["CantidadArchivos"] = cantidadArchivos;

                //Ubicacion del rq
                string direccion = requerimiento.DomicilioDireccion;
                string observaciones = requerimiento.DomicilioObservaciones;
                if (string.IsNullOrEmpty(direccion))
                {
                    direccion = "";
                    if (string.IsNullOrEmpty(observaciones))
                    {
                        observaciones = "";
                    }
                    // direccion = direccion;
                }

                filaRequerimiento["domicilioDireccion"] = SecurityElement.Escape(direccion);
                filaRequerimiento["domicilioObservacion"] = observaciones;



                string barrio = requerimiento.DomicilioBarrioNombre;
                if (string.IsNullOrEmpty(barrio))
                {
                    barrio = "";
                }
                filaRequerimiento["domicilioBarrio"] = Utils.toTitleCase(barrio);

                string cpcNombre = requerimiento.DomicilioCpcNombre;
                if (string.IsNullOrEmpty(cpcNombre))
                {
                    cpcNombre = "";
                }
                filaRequerimiento["cpcNombre"] = Utils.toTitleCase(cpcNombre);

                int? cpcNumero = requerimiento.DomicilioCpcNumero;
                if (cpcNumero == null)
                {
                    cpcNumero = 0;
                }
                filaRequerimiento["cpcNumero"] = cpcNumero;

                /*La tabla materiales solo es visible para el area Alumbrado Publico*/
                if (requerimiento.AreaId == 285)
                {
                    Telerik.Reporting.Table tablaMateriales = reporte.Items.Find("tablaMateriales", true)[0] as Telerik.Reporting.Table;
                    tablaMateriales.Visible = true;
                }



                var url1 = "https://maps.googleapis.com/maps/api/staticmap?autoscale=2&size=600x600&maptype=roadmap&format=png&visual_refresh=true&markers=size:mid%7Ccolor:0xff0000%7Clabel:%7C&key=" + ConfigurationManager.AppSettings["KEY_GOOGLE_MAPS"];
                var marcadores = "";

                //Mapa
                if (conMapa && requerimiento.DomicilioLatitud != null && requerimiento.DomicilioLongitud != null)
                {
                    var x = requerimiento.DomicilioLatitud.Replace(",", ".");
                    var y = requerimiento.DomicilioLongitud.Replace(",", ".");

                    marcadores = x + "," + y;
                    url1 += "&markers=" + marcadores;

                    Telerik.Reporting.PictureBox picture = reporte.Items.Find("imgMapa", true)[0] as Telerik.Reporting.PictureBox;
                    picture.Value = url1;
                    picture.Visible = true;
                }

                //Agrego la fila del requerimiento
                dtRequerimiento.Rows.Add(filaRequerimiento);

                //Seteo el datasource al reclamo
                objectDataSourceRequerimiento.DataSource = dtRequerimiento;
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

        public Result<Telerik.Reporting.Report> GenerarReporteRequerimientoListadoV2(List<int> ids, string filtros)
        {
            var result = new Result<Telerik.Reporting.Report>();

            var resultData = RequerimientoDAO.Instance.GetResultadoByIds(ids);
            if (!resultData.Ok)
            {
                result.AddErrorPublico("Ddemasiados requerimientos para imprimir");
                /*Reportar error*/
                return null;
            }
            
            try
            {
                var rutaReporte = HttpContext.Current.Server.MapPath("~/Resources/Reportes/Requerimiento/Reporte_RequerimientoListadoNV2.trdx");
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
                dtRequerimientos.Columns.Add("IdMotivo");
                dtRequerimientos.Columns.Add("NumeroRequerimiento");                      
                dtRequerimientos.Columns.Add("AreaNombre");
                dtRequerimientos.Columns.Add("IdArea");
                dtRequerimientos.Columns.Add("CategoriaNombre");
                dtRequerimientos.Columns.Add("IdCategoria");
                //dtRequerimientos.Columns.Add("Filtros");
                dtRequerimientos.Columns.Add("stringUbicacion");
                dtRequerimientos.Columns.Add("stringDescripcion");
                dtRequerimientos.Columns.Add("fechaAlta");   


                foreach (Resultado_Requerimiento r in resultData.Return)
                {
                    DataRow filaRequerimiento = dtRequerimientos.NewRow();
                    string motivo = null;



                    motivo += Utils.toTitleCase(r.MotivoNombre);
                    //Categoria
                    //if (!string.IsNullOrEmpty(r.categoriaNombre))
                    //{
                    //    motivo += "Categoria: " + r.categoriaNombre;
                    //}
                    filaRequerimiento["MotivoNombre"] = motivo;
                    

                    filaRequerimiento["IdMotivo"] = r.MotivoId;

                    filaRequerimiento["CategoriaNombre"] = r.CategoriaNombre;
                    filaRequerimiento["IdCategoria"] = r.CategoriaId;

                    filaRequerimiento["AreaNombre"] = r.AreaNombre;
                    filaRequerimiento["IdArea"] = r.AreaId;


                    filaRequerimiento["NumeroRequerimiento"] = r.Numero + "/" + r.Año;


                    //Ubicacion del rq
                    var stringUbicacion = "";

                    if (!string.IsNullOrEmpty(r.FechaAlta.ToString()))
                    {
                        filaRequerimiento["fechaAlta"] = r.FechaAlta.Value.ToString("dd/MM/yyyy");
                    } 
                    
                    //Cpc
                    if (!string.IsNullOrEmpty(r.Domicilio.Barrio.Nombre))
                    {
                        //if (!string.IsNullOrEmpty(stringUbicacion)) stringUbicacion += " | ";
                        stringUbicacion += "CPC: N° " + r.Domicilio.Cpc.Numero + " " + r.Domicilio.Cpc.Nombre;
                    }

                    //Barrio
                    if (!string.IsNullOrEmpty(r.Domicilio.Barrio.Nombre))
                    {
                        if (!string.IsNullOrEmpty(stringUbicacion)) stringUbicacion += " | ";
                        stringUbicacion += "Barrio: " + r.Domicilio.Barrio.Nombre;
                    }

                    //Direccion
                    if (!string.IsNullOrEmpty(r.Domicilio.Direccion))
                    {
                        if (!string.IsNullOrEmpty(stringUbicacion)) stringUbicacion += " | ";
                        stringUbicacion += "Dirección: " + SecurityElement.Escape(r.Domicilio.Direccion);
                    }

                    //Obs
                    if (!string.IsNullOrEmpty(r.Domicilio.Observaciones))
                    {
                        if (!string.IsNullOrEmpty(stringUbicacion)) stringUbicacion += " | ";
                        stringUbicacion += "Observaciones: " + SecurityElement.Escape(r.Domicilio.Observaciones);
                    }                   
                    var stringDescripcion = "";
                    
                    //Descripcion Por Requerimietno
                    if (!string.IsNullOrEmpty(r.Descripcion))
                    {
                        if (!string.IsNullOrEmpty(r.Descripcion))
                        {
                            stringDescripcion += SecurityElement.Escape(r.Descripcion);
                        }
                    }
                    
                    filaRequerimiento["stringUbicacion"] = stringUbicacion;
                    filaRequerimiento["stringDescripcion"] = stringDescripcion;



                    //string encodedXml = HttpUtility.HtmlEncode(stringFinal);
                    //string test = HttpUtility.HtmlDecode(encodedXml);
                    
                    //stringFinal.Replace(" & ", " & amp; ").Replace(" < ", " & lt; ").Replace(" > ", " & gt; ").Replace(" & # 34; ", " & quot; ").Replace(" ' ", " & apos; "); 

                   


                    filaRequerimiento["IdMotivo"] = r.MotivoId;
                    dtRequerimientos.Rows.Add(filaRequerimiento);
                }

                Telerik.Reporting.HtmlTextBox txtFiltros = reporte.Items.Find("txtFiltros", true)[0] as Telerik.Reporting.HtmlTextBox;
                txtFiltros.Value = filtros.ToString();

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


    }
}

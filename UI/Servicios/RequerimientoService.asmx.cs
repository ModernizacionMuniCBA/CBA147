using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model;
using Model.Entities;
using Newtonsoft.Json;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Rules.Rules.Mails;
using Intranet_UI.Utils;
using Model.Resultados;
using Model.Consultas;
using Model.Comandos;
using System.IO;
using System.Configuration;
using Model.Resultados.Estadisticas;
//using Model.Resultados.Estadisticas;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class RequerimientoService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Requerimiento> Insertar(Comando_RequerimientoIntranet comando)
        {
            ValidarSesion(Session);

            var resultado = new Result<Resultado_Requerimiento>();

            //Busco el origen en la sesion
            var idOrigen = SessionKey.getOrigen(Session);
            if (!idOrigen.HasValue)
            {
                resultado.AddErrorPublico("Debe indicar el origen");
                return resultado;
            }

            var resultadoOrigen = new OrigenRules(SessionKey.getUsuarioLogueado(Session)).GetById(idOrigen.Value);
            if (!resultadoOrigen.Ok)
            {
                resultado.Errores.Copy(resultadoOrigen.Errores);
                return resultado;
            }

            if (resultadoOrigen.Return == null)
            {
                resultado.AddErrorPublico("El origen indicado es invalido");
                return resultado;
            }

            comando.OrigenAlias = resultadoOrigen.Return.KeyAlias;
            comando.OrigenSecret = resultadoOrigen.Return.KeySecret;

            return new RequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Requerimiento> UnirseARequerimiento(Comando_RequerimientoIntranet comando)
        {
            ValidarSesion(Session);

            var resultado = new Result<Resultado_Requerimiento>();

            //Busco el origen en la sesion
            var idOrigen = SessionKey.getOrigen(Session);
            if (!idOrigen.HasValue)
            {
                resultado.AddErrorPublico("Debe indicar el origen");
                return resultado;
            }

            var resultadoOrigen = new OrigenRules(SessionKey.getUsuarioLogueado(Session)).GetById(idOrigen.Value);
            if (!resultadoOrigen.Ok)
            {
                resultado.Errores.Copy(resultadoOrigen.Errores);
                return resultado;
            }

            if (resultadoOrigen.Return == null)
            {
                resultado.AddErrorPublico("El origen indicado es invalido");
                return resultado;
            }

            comando.OrigenAlias = resultadoOrigen.Return.KeyAlias;
            comando.OrigenSecret = resultadoOrigen.Return.KeySecret;

            return new RequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).UnirseARequerimiento(comando);
        }

        //[WebMethod(EnableSession = true)]
        //public Result<List<int>> GetIdsFotos(int id)
        //{
        //    ValidarSesion(Session);
        //    return new ArchivoPorRequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetIdsByFilters(new Consulta_ArchivoPorRequerimiento()
        //    {
        //        IdRequerimiento = id,
        //        Tipo = Enums.TipoArchivo.IMAGEN,
        //        DadosDeBaja = false
        //    });
        //}

        //[WebMethod(EnableSession = true)]
        //public Result<List<int>> GetIdsDocumentos(int id)
        //{
        //    ValidarSesion(Session);
        //    return new ArchivoPorRequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetIdsByFilters(new Consulta_ArchivoPorRequerimiento()
        //    {
        //        IdRequerimiento = id,
        //        Tipo = Enums.TipoArchivo.DOCUMENTO,
        //        DadosDeBaja = false
        //    });
        //}

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_ArchivoPorRequerimiento_Imagen>> GetImagenes(int id)
        {
            ValidarSesion(Session);
            string server = ConfigurationManager.AppSettings["URL_SERVER_ARCHIVO"];
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetImagenes(server, id);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_ArchivoPorRequerimiento_Documento>> GetDocumentos(int id)
        {
            ValidarSesion(Session);
            string server = ConfigurationManager.AppSettings["URL_SERVER_ARCHIVO"];
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetDocumentos(server, id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTabla(Consulta_Requerimiento consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetResultadoTablaByFilters(consulta, null);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaByIds(List<int> ids)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetResultadoTablaByIds(ids);
        }
        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_RequerimientoExportar>> GetResultadoTablaByIdsExportar(List<int> ids)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetResultadoTablaByIdsExportar(ids);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_Requerimiento> GetResultadoTablaById(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetResultadoTablaById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaBusquedaGlobal(string input)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetResultadoTablaBusquedaGlobal(input);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaParaOrdenTrabajo(Consulta_Requerimiento_Bandeja consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            //se llama al mismo metodo para ot y oi pero desde diferentes services por las dudas cambie
            return new RequerimientoRules(userLogeado).GetResultadoTablaParaBandeja(consulta);
        }


        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaParaOrdenInspeccion(Consulta_Requerimiento_Bandeja consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            //se llama al mismo metodo para ot y oi pero desde diferentes services por las dudas cambie
            consulta.OrdenInspeccion = true;
            return new RequerimientoRules(userLogeado).GetResultadoTablaParaBandeja(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_RequerimientoInfo>> GetInfoGlobal()
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetInfoGlobal();
        }


        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadUrgentesNuevos()
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetCantidadUrgentesNuevos();
        }

        //public string EnviarMailContacto(string usuario, string rol, string asunto, string descripcion, string mail, string mailContacto, string telefonoContacto)
        //{
        //    ValidarSesion(Session);

        //    Dictionary<string, object> dicUsuario = (Dictionary<string, object>)new JavaScriptSerializer().Deserialize(usuario, typeof(Dictionary<string, object>));
        //    Dictionary<string, object> dicRol = (Dictionary<string, object>)new JavaScriptSerializer().Deserialize(rol, typeof(Dictionary<string, object>));

        //    var resultado = new Dictionary<string, object>();
        //    //validar si los datos vienen null

        //    string nombreCompleto = (string)dicUsuario["Nombre"] + " " + (string)dicUsuario["Apellido"];
        //    string nombreRol = (string)dicRol["Rol"];


        //    //var areas = new List<string>();
        //    //if (dicUsuario.ContainsKey("Areas") && dicUsuario["Areas"] != null)
        //    //{
        //    //    var arrayAreas = (ArrayList)dicUsuario["Areas"];
        //    //    foreach (object p in arrayAreas)
        //    //    {
        //    //        var dicAreas = (Dictionary<string, object>)p;
        //    //        //Nombre
        //    //        var area = "" + dicAreas["Nombre"];
        //    //        areas.Add(area);
        //    //    }
        //    //}

        //    //Areas
        //    var areasUsuario = SessionKey.getUsuarioLogueado(HttpContext.Current.Session).Areas;

        //    var areas = new List<string>();

        //    foreach (object p in areasUsuario)
        //    {
        //        var dicAreas = (Dictionary<string, object>)p;
        //        //Nombre
        //        var area = "" + dicAreas["Nombre"];
        //        areas.Add(area);
        //    }

        //    var result = new RequerimientoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EnviarMailContacto(nombreCompleto, nombreRol, areas, asunto, descripcion, mail, mailContacto, telefonoContacto);

        //    if (!result.Ok)
        //    {
        //        resultado.Add("Error", result.ToDictionary());
        //        return JsonUtils.toJson(resultado);
        //    }

        //    return JsonUtils.toJson(resultado);
        //}

        //[WebMethod(EnableSession = true)]
        //public string ValidarEnAltura(string requerimiento)
        //{
        //    ValidarSesion(Session);
        //    var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
        //    var resultado = new Dictionary<string, object>();
        //    Dictionary<string, object> data = (Dictionary<string, object>)new JavaScriptSerializer().Deserialize(requerimiento, typeof(Dictionary<string, object>));
        //    int idCalle = -1;
        //    int altura = -1;
        //    int? idMotivo = null;

        //    //Ubicacion
        //    if (!data.ContainsKey("Ubicacion") || (data.ContainsKey("Ubicacion") && data["Ubicacion"] is string))
        //    {
        //        resultado.Add("DomicilioManual", "yes");
        //        return JsonUtils.toJson(resultado);
        //    }

        //    var dataDomicilio = (Dictionary<string, object>)data["Ubicacion"];
        //    var domicilio = new Domicilio();

        //    int idCalleCatastro;
        //    //si nno hay id calle catastro, es por barrio
        //    var porBarrio = !(Int32.TryParse(dataDomicilio["IdCalleCatastro"].ToString(), out idCalleCatastro));
        //    domicilio.PorBarrio = porBarrio;

        //    int idBarrioCatastro;
        //    Int32.TryParse(dataDomicilio["IdBarrioCatastro"].ToString(), out idBarrioCatastro);

        //    if (!porBarrio)
        //    {
        //        Result<Calle> resultCalle = new CalleRules(userLogeado).GetByIdCatastro(idCalleCatastro);
        //        if (!resultCalle.Ok)
        //        {
        //            resultado.Add("Error", resultCalle.ToDictionary());
        //            return JsonUtils.toJson(resultado);
        //        }

        //        if (resultCalle.Return != null)
        //        {
        //            idCalle = resultCalle.Return.Id;
        //        }

        //        //Altura
        //        altura = (int)dataDomicilio["Altura"];
        //    }

        //    if (data.ContainsKey("IdMotivo"))
        //    {
        //        idMotivo = Int32.Parse((string)data["IdMotivo"]);
        //    }

        //    //Consulto

        //    Result<List<int>> resultConsulta = new RequerimientoRules(userLogeado).GetIds(null, null, null, null, null, idMotivo, null, false, null, null, idCalle, null, null, null, null, false, null, false, altura, null);
        //    if (!resultConsulta.Ok)
        //    {
        //        resultado.Add("Error", resultConsulta.ToDictionary());
        //        return JsonUtils.toJson(resultado);
        //    }

        //    resultado.Add("Ids", resultConsulta.Return);
        //    return JsonUtils.toJson(resultado);
        //}

        //[WebMethod(EnableSession = true)]
        //public Result<Resultado_DatosEstadisticaPanel> GetDatosEstadisticaYMapa(int periodo)
        //{
        //    var fechaHasta = DateTime.Now.Date;
        //    var fechaDesde = DateTime.Now.Date;

        //    switch (periodo)
        //    {
        //        case 1:
        //            //Ultimos 30 dias
        //            fechaDesde = DateTime.Now.AddDays(-30).Date;
        //            break;
        //        case 2:
        //            //Ultimos 90 dias
        //            fechaDesde = DateTime.Now.AddDays(-90).Date;
        //            break;
        //        case 3:
        //            //Ultimos 180 dias
        //            fechaDesde = DateTime.Now.AddDays(-180).Date;
        //            break;
        //        case 4:
        //            fechaDesde = DateTime.Now.AddYears(-10).Date;
        //            break;
        //    }

        //    ValidarSesion(Session);
        //    var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
        //    var result = new EstadisticaRules(userLogeado).GetDatosEstadisticaYMapa(fechaDesde, fechaHasta);

        //    if (!result.Ok || result.Return == null)
        //    {
        //        result.AddErrorPublico("Error al generar el resumen estadístico");
        //        return result;
        //    }

        //    var ResultUrl = CrearMapaCpc(result.Return.ArrayDatosMapa);
        //    if (!ResultUrl.Ok)
        //    {
        //        result.AddErrorPublico(result.Errores.ErroresPublicos);
        //        return result;
        //    }

        //    result.Return.UrlMapa = ResultUrl.Return;
        //    return result;
        //}

        private Result<string> CrearMapaCpc(List<Resultado_DatosEstadisticaPanel_Cpc> CPCs)
        {
            var result = new Result<string>();

            result.AddErrorPublico("Error comunicaciondose con el servicio de mapas");

            /*
            try
            {
                var catastro = new CatastroMapas.CatastroWSDL();

        //        var datosCpc = new List<CatastroMapas.datosCPC>();

        //        foreach (var cpc in CPCs)
        //        {
        //            var datoCpc = new CatastroMapas.datosCPC();
        //            int cant = cpc.CantidadRequerimientos;
        //            datoCpc.CantReclamos = cant;
        //            datoCpc.NumeroCPC = cpc.Cpc.Numero;
        //            datoCpc.Color = cpc.Color;
        //            datoCpc.Criticidad = cpc.Criticidad;
        //            datosCpc.Add(datoCpc);
        //        }

        //        CatastroMapas.datosCPC[] datos = datosCpc.ToArray();

        //        var url = catastro.reclamosCPC(datos);

                var parsedQuery = HttpUtility.ParseQueryString(url);
                var id = parsedQuery["idmapa"];
               
                result.Return = url;
            }
            catch (Exception e)
            {
                result.AddErrorPublico("Error comunicaciondose con el servicio de mapas");
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }
            */

            return result;
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Requerimiento>> GetResultadoTablaCercanos(Consulta_RequerimientoCercano consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetResultadoTablaCercanos(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadCercanos(Consulta_RequerimientoCercano consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetCantidadCercanos(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea>> GetCantidadRequerimientosParaOrdenDeTrabajoPorArea()
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetCantidadRequerimientosParaOrdenDeTrabajoPorArea();
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo>> GetCantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo(int idArea)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetCantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo(idArea);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorArea>> GetCantidadRequerimientosParaOrdenDeInspeccionPorArea()
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetCantidadRequerimientosParaOrdenDeInspeccionPorArea();
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_RequerimientoDetalle2> GetDetalleById(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetDetalleById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<int>> GetIdsByNumero(string numero, int? año)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).GetIdsByNumero(numero, año);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_RequerimientoTopBarrios>> GetTopPorBarrio(Consulta_RequerimientoTopBarrios consulta = null)
        {
            ValidarSesion(Session);
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).GetTopPorBarrio(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_MarcadorGoogleMaps>> GetTopMarcadoresPorBarrio(Consulta_RequerimientoTopBarrios consulta = null)
        {
            ValidarSesion(Session);
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).GetTopMarcadoresPorBarrio(consulta);
        }

        /* Acciones */

        //Archivos
        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarArchivo(int id, Comando_Archivo comando)
        {
            var resultado = new Result<bool>();

            ValidarSesion(Session);
            //if (!HttpContext.Current.Request.Files.AllKeys.Any())
            //{
            //    resultado.AddErrorPublico("Debe mandar el contenido del archivo");
            //    return resultado;
            //}

            //// Get the uploaded image from the Files collection
            //var httpPostedFile = HttpContext.Current.Request.Files["Data"];
            //if (httpPostedFile == null)
            //{
            //    resultado.AddErrorPublico("Debe mandar el contenido del archivo");
            //    return resultado;
            //}

            //StreamReader stream = new StreamReader(httpPostedFile.InputStream);
            //comando.Data = stream.ReadToEnd();

            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).AgregarArchivo(id, comando);

        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarArchivo(int id, int idArchivo)
        {
            ValidarSesion(Session);
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).QuitarArchivo(id, idArchivo);
        }


        //Favorito
        [WebMethod(EnableSession = true)]
        public Result<bool> ToggleFavorito(int id)
        {
            ValidarSesion(Session);
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).ToggleFavorito(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> SetFavorito(int id)
        {
            ValidarSesion(Session);
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).SetFavorito(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> SetNoFavorito(int id)
        {
            ValidarSesion(Session);
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).SetNoFavorito(id);
        }


        //Estado
        [WebMethod(EnableSession = true)]
        public Result<Resultado_Requerimiento> CambiarEstado(int id, Enums.EstadoRequerimiento keyValue, string observaciones)
        {
            ValidarSesion(Session);
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).CambiarEstado(id, keyValue, observaciones);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Requerimiento> Cancelar(int id, string observaciones)
        {
            ValidarSesion(Session);
            return new RequerimientoRules(SessionKey.getUsuarioLogueado(Session)).Cancelar(id, observaciones);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<EstadoRequerimiento>> GetEstadoRequerimiento()
        {
            ValidarSesion(Session);

            var result = new EstadoRequerimientoRules(SessionKey.getUsuarioLogueado(Session)).GetAll();
            result.Return = result.Return.OrderBy(x => x.Orden).ToList();

            return result;
        }

        //Prioridad
        [WebMethod(EnableSession = true)]
        public Result<Enums.PrioridadRequerimiento> TogglePrioridad(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).TogglePrioridad(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Enums.PrioridadRequerimiento> SetPrioridad(int id, Enums.PrioridadRequerimiento prioridad)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).SetPrioridad(id, prioridad);
        }

        //Marcado
        [WebMethod(EnableSession = true)]
        public Result<bool> ToggleMarcado(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).ToggleMarcado(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> Marcar(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).MarcarRequerimiento(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> Desmarcar(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).DesmarcarRequerimiento(id);
        }

        //Referente
        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarReferente(int id, int idUsuario)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).AgregarReferente(id, idUsuario);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarReferente(int id, int idUsuario)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).QuitarReferente(id, idUsuario);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarReferenteProvisorio(Model.Comandos.Comando_RequerimientoIntranet.Comando_ReferenteProvisorio comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).EditarReferenteProvisorio(comando);
        }

        //Motivo
        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarMotivo(int id, int idMotivo)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).CambiarMotivo(id, idMotivo);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarMotivoDesdeOT(int id, int idMotivo)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).CambiarMotivoDesdeOT(id, idMotivo);
        }

        //Domicilio
        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarDomicilio(int id, Comando_Domicilio domicilio)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).CambiarDomicilio(id, domicilio);
        }

        //Comentarios
        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarComentario(Comando_RequerimientoComentario comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(userLogeado).AgregarComentario(comando);
        }

        //Tareas
        [WebMethod(EnableSession = true)]
        public Result<bool> EditarTareas(Comando_RequerimientoTareas comando)
        {
            ValidarSesion(Session);
            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(usuarioLogeado).EditarTareas(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarTarea(Comando_RequerimientoTareas comando)
        {
            ValidarSesion(Session);
            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(usuarioLogeado).QuitarTarea(comando);
        }


        [WebMethod(EnableSession = true)]
        public Result<bool> EditarCamposDinamicos(Comando_RequerimientoEditarCamposDinamicos comando)
        {
            ValidarSesion(Session);
            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new RequerimientoRules(usuarioLogeado).EditarCamposDinamicos(comando);
        }

        //Comprobante atencion
        [WebMethod(EnableSession = true)]
        public Result<bool> EnviarComprobanteAtencion(int id, IList<int> idsUsuarios, string email)
        {
            ValidarSesion(Session);
            return new RequerimientoMailRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EnviarComprobanteAtencion(id, idsUsuarios, email);
        }

        //Enviar mensaje
        [WebMethod(EnableSession = true)]
        public Result<bool> EnviarMensaje(int id, string mensaje)
        {
            ValidarSesion(Session);

            var resultado = new Dictionary<string, object>();
            var usuariologeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            return new RequerimientoMailRules(usuariologeado).EnviarMensajeAUsuarios(id, mensaje);
        }


        #region Mapa

        //[WebMethod(EnableSession = true)]
        //public Result<string> GenerarMapaPorIds(List<int> ids)
        //{
        //    ValidarSesion(Session);
        //    var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

        //    return new RequerimientoRules(userLogeado).CrearMapa(ids);

        //    //var result = new Dictionary<string, object>();
        //    //var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
        //    //var requerimientoRules = new RequerimientoRules(userLogeado);
        //    //var resultConsultarRequerimientos = requerimientoRules.GetById(idRequerimientos);
        //    //if (!resultConsultarRequerimientos.Ok)
        //    //{
        //    //    result.Add("Error", resultConsultarRequerimientos.ToDictionary());
        //    //    return JsonUtils.toJson(result);
        //    //}

        //    //var resultadoMapa = requerimientoRules.CrearMapa(resultConsultarRequerimientos.Return);
        //    //if (!resultadoMapa.Ok)
        //    //{
        //    //    result.Add("Error", resultadoMapa.ToDictionary());
        //    //    return JsonUtils.toJson(result);
        //    //}
        //    //result.Add("Url", resultadoMapa.Return);
        //    //return JsonUtils.toJson(result);
        //}

        //[WebMethod(EnableSession = true)]
        //public Result<string> GenerarMapaPorId(int id)
        //{
        //    ValidarSesion(Session);
        //    var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

        //    var ids = new List<int>() { id };
        //    return new RequerimientoRules(userLogeado).CrearMapa(ids);
        //}

        [WebMethod(EnableSession = true)]
        public string GenerarMapaPorCpc(string datosMapa)
        {
            ValidarSesion(Session);

            var result = new Dictionary<string, object>();

            var dicDatos = new List<Dictionary<string, object>>();


            if (datosMapa != null)
            {
                dicDatos = (List<Dictionary<string, object>>)new JavaScriptSerializer().Deserialize(datosMapa, typeof(List<Dictionary<string, object>>));
            }


            //var resultadoMapa = RequerimientoRules.Instance.CrearMapaCpc(dicDatos);
            //if (!resultadoMapa.Ok)
            //{
            //    result.Add("Error", resultadoMapa.ToDictionary());
            //    return JsonUtils.toJson(result);
            //}
            //result.Add("Url", resultadoMapa.Return);
            return JsonUtils.toJson(result);

        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_MarcadorGoogleMaps>> GetMarcadoresGoogleMapsPorIds(List<int> ids)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            return new RequerimientoRules(userLogeado).GetMarcadoresGoogleMaps(ids);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_MarcadorGoogleMaps>> GetMarcadorGoogleMapsPorId(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var ids = new List<int>() { id };
            return new RequerimientoRules(userLogeado).GetMarcadoresGoogleMaps(ids);
        }

        #endregion

        //[WebMethod(EnableSession = true)]
        //public Result<bool> InferirOrigenDeTodosLosRequerimientos()
        //{
        //    ValidarSesion(Session);
        //    var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
        //    return new RequerimientoRules(userLogeado).InferirOrigenDeTodosLosRequerimientos();
        //}

    }
}

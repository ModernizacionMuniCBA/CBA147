    using Model;
using Model.Comandos;
using Model.Consultas;
using Model.Resultados;
using Rules.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using UI.Resources;

namespace UI.Servicios
{
    /// <summary>
    /// Descripción breve de MovilService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class MovilService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Movil> Insertar(Comando_Movil comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insert(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarInformacionBasica(Comando_Movil comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EditarInformacionBasica(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarValuacion(Comando_Movil_Valuacion comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EditarValuacion(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarKilometraje(Comando_Movil_Kilometraje comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EditarKilometraje(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarITV(Comando_Movil_ITV comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EditarITV(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarTUV(Comando_Movil_TUV comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EditarTUV(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarCondicion(Comando_Movil_Condicion comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EditarCondicion(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarCaracteristicas(Comando_Movil_Caracteristicas comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).EditarCaracteristicas(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarEstado(Comando_CambioEstado comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).CambiarEstado(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarNota(Comando_Movil_Nota comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).AgregarNota(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> VisarNota(Comando_Movil_VisarNota comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).VisarNota(comando);
        }


        [WebMethod(EnableSession = true)]
        public Result<bool> AgregarReparacion(Comando_Movil_Reparacion comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).AgregarReparacion(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> BorrarReparacion(Comando_Movil_Reparacion comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).BorrarReparacion(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Movil> DarDeBaja(Comando_Movil comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Delete(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Movil> DarDeAlta(Comando_Movil comando)
        {
            ValidarSesion(Session);
            return new MovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeAlta(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Movil>> GetResultadoByFilters(Consulta_Movil consulta)
        {
            ValidarSesion(Session);
            return new MovilRules(GetUsuarioLogeado()).GetResultadoByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Movil>> GetResultadoTablaByFilters(Consulta_Movil consulta)
        {
            ValidarSesion(Session);
            return new MovilRules(GetUsuarioLogeado()).GetResultadoTablaByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Movil> GetDetalleById(int id)
        {
            ValidarSesion(Session);
            return new MovilRules(GetUsuarioLogeado()).GetResultadoById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Movil>> GetResultadoTablaByIds(List<int> ids)
        {
            ValidarSesion(Session);
            return new MovilRules(GetUsuarioLogeado()).GetResultadoTablaByIds(ids);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_Movil> GetResultadoTablaById(int id)
        {
            ValidarSesion(Session);
            return new MovilRules(GetUsuarioLogeado()).GetResultadoTablaById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Movil>> GetResultadoTablaByIdOrdenTrabajo(int idOrden)
        {
            ValidarSesion(Session);
            return new MovilRules(GetUsuarioLogeado()).GetResultadoTablaByIdOrdenTrabajo(idOrden);
        }

        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadParaAgregarAOT(int id)
        {
            ValidarSesion(Session);
            return new MovilRules(GetUsuarioLogeado()).GetCantidadParaAgregarAOT(id);
        }
    }
}

using Model;
using Model.Comandos;
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
    /// Descripción breve de TareaService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class TareaService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_TareaPorArea> Insertar(Comando_TareaPorArea comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new TareaPorAreaRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> DarDeBaja(int id)
        {
            ValidarSesion(Session);
            return new TareaPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> DarDeAlta(int id)
        {
            ValidarSesion(Session);
            return new TareaPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeAlta(id);
        }


        [WebMethod(EnableSession = true)]
        public Result<bool> EditarNombre(Comando_TareaPorArea_Editar comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new TareaPorAreaRules(userLogeado).EditarNombre(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> EditarDescripcion(Comando_TareaPorArea_Editar comando)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new TareaPorAreaRules(userLogeado).EditarDescripcion(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_TareaPorArea> GetDetalleById(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new TareaPorAreaRules(userLogeado).GetDetalleById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_TareaPorArea> GetResultadoTablaById(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new TareaPorAreaRules(userLogeado).GetResultadoTablaById(id);
        }


        [WebMethod(EnableSession = true)]
        public Result<List<ResultadoTabla_TareaPorArea>> GetByIdRequerimiento(int idRequerimiento)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new TareaPorAreaRules(userLogeado).GetByIdRequerimiento(idRequerimiento);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<ResultadoTabla_TareaPorArea>> GetByIdRequerimientoYArea(int idRequerimiento)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new TareaPorAreaRules(userLogeado).GetByIdRequerimientoYArea(idRequerimiento);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<ResultadoTabla_TareaPorArea>> GetByIdArea(int idArea)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new TareaPorAreaRules(userLogeado).GetByIdArea(idArea);
        }
    }
}

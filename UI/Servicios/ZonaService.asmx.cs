using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model;
using Model.Resultados;
using Model.Comandos;
using Model.Consultas;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class ZonaService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Result<Resultado_Zona> Insertar(Comando_Zona comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ZonaRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Zona> Actualizar(Comando_Zona comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ZonaRules(userLogueado).Actualizar(comando);
        }


        [WebMethod(EnableSession = true)]
        public Result<Resultado_Zona> DarDeBaja(int id)
        {
            ValidarSesion(Session);
            return new ZonaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Zona> DarDeAlta(int id)
        {
            ValidarSesion(Session);
            return new ZonaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeAlta(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Zona>> GetResultadoTabla(Consulta_Zona consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ZonaRules(userLogeado).GetResultadoTablaByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_Zona>> GetResultadoTablaByIds(List<int> ids)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ZonaRules(userLogeado).GetResultadoTablaByIds(ids);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_Zona> GetResultadoTablaById(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ZonaRules(userLogeado).GetResultadoTablaById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Zona>> GetByArea(Consulta_Zona consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ZonaRules(userLogeado).GetByFilters(consulta);
        }

    }
}

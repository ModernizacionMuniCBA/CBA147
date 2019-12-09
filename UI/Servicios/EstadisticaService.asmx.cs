using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model;
using Model.Entities;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model.Resultados;
using Model.Consultas;
using Model.Resultados.Estadisticas;
//using Model.Resultados.Estadisticas;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class EstadisticaService : _BaseService
    {       

        /*ESTADISTICAS V2*/
        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaPanel_Cpc>> GetDatosEstadistica_CPC(Consulta_EstadisticaCPC consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaCpc(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaOrigen>> GetDatosEstadisticaOrigen(Consulta_EstadisticaOrigen consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaOrigen(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaEficacia>> GetDatosEstadisticaEficacia(Consulta_EstadisticaEficacia consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaEficacia(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaResueltos>> GetDatosEstadisticaResueltos(Consulta_EstadisticaResueltos consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaResueltos(consulta);
        }
        
        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaServicios>> GetDatosEstadisticaServicios(Consulta_EstadisticaServicios consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaServicios(consulta);
        }
        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaUsuario>> GetDatosEstadisticaUsuario(Consulta_EstadisticaUsuario consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaUsuario(consulta);
        }
        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaArea>> GetDatosEstadisticaArea(Consulta_EstadisticaArea consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaArea(consulta);
        }
        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaArea>> GetDatosEstadisticaSubArea(Consulta_EstadisticaSubArea consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaSubArea(consulta);
        }
        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaZona>> GetDatosEstadisticaZona(Consulta_EstadisticaZona consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaZona(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadByIdArea(int idArea, int tipoCatalogo)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var catalogo = new CatalogoRules(userLogeado);
            
            Result<int> resultado = null;
            switch (tipoCatalogo)
            {
                case 1:
                    resultado = catalogo.GetCantidadUsuariosByIdArea(idArea);
                    break;
                case 2:
                    resultado = catalogo.GetCantidadMotivosByIdArea(idArea);
                    break;
                case 3:
                    resultado = catalogo.GetCantidadTareasByIdArea(idArea);
                    break;
            }

            return resultado;

         
        }
        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadMotivosByIdArea(int idArea)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new CatalogoRules(userLogeado).GetCantidadMotivosByIdArea(idArea);
        }
        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadTareasByIdArea(int idArea)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new CatalogoRules(userLogeado).GetCantidadTareasByIdArea(idArea);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaMotivos>> GetDatosEstadisticaMotivos(Consulta_EstadisticaMotivos consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaMotivos(consulta);
        }
        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_DatosEstadisticaRubros>> GetDatosEstadisticaRubros(Consulta_EstadisticaRubros consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new EstadisticaRules(userLogeado).GetDatosEstadisticaRubros(consulta);
        }
    }
}

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
    /// Descripción breve de FlotaService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class FlotaService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Result<Resultado_Flota> Insertar(Comando_Flota comando)
        {
            ValidarSesion(Session);
            return new FlotaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<int> Editar(Comando_Flota comando)
        {
            ValidarSesion(Session);
            return new FlotaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Editar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarEstado(Comando_CambioEstado comando)
        {
            ValidarSesion(Session);
            return new FlotaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).CambiarEstado(comando);
        }
        
        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Flota>> GetByFilters(Consulta_Flota consulta)
        {
            ValidarSesion(Session);
            return new FlotaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Flota>> GetPanel(Consulta_Flota consulta)
        {
            ValidarSesion(Session);
            return new FlotaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetPanel(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Flota> GetResultadoById(int id)
        {
            ValidarSesion(Session);
            return new FlotaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetResultadoById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Flota>> GetResultadoByIdOrdenTrabajo(int id)
        {
            ValidarSesion(Session);
            return new FlotaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetResultadoByIdOrdenTrabajo(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidadParaAgregarAOT(int id)
        {
            ValidarSesion(Session);
            return new FlotaRules(GetUsuarioLogeado()).GetCantidadParaAgregarAOT(id);
        }


        [WebMethod(EnableSession = true)]
        public Result<bool> TerminarTurnoTodasLasFlotas(List<int> idsFlotas)
        {
            ValidarSesion(Session);
            return new FlotaRules(GetUsuarioLogeado()).TerminarTurno(idsFlotas);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> TerminarTurno(int id)
        {
            ValidarSesion(Session);
            return new FlotaRules(GetUsuarioLogeado()).TerminarTurno(id);
        }

    }
}


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
    /// Descripción breve de RubroMotivoService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class RubroMotivoService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_RubroMotivo> Insertar(Comando_RubroMotivo comando)
        {
            ValidarSesion(Session);
            return new RubroMotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_RubroMotivo> Editar(Comando_RubroMotivo comando)
        {
            ValidarSesion(Session);
            return new RubroMotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Editar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_RubroMotivo> DarDeBaja(int id)
        {
            ValidarSesion(Session);
            return new RubroMotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_RubroMotivo> DarDeAlta(int id)
        {
            ValidarSesion(Session);
            return new RubroMotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeAlta(id);
        }
    }
}

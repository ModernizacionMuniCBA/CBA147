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
    /// Descripción breve de GrupoCategoriaMotivoService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class GrupoRubroMotivoService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_GrupoCategoriaMotivo> Insertar(Comando_GrupoRubroMotivo comando)
        {
            ValidarSesion(Session);
            return new GrupoRubroMotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insertar(comando);
        }


        [WebMethod(EnableSession = true)]
        public Result<Resultado_GrupoCategoriaMotivo> Editar(Comando_GrupoRubroMotivo comando)
        {
            ValidarSesion(Session);
            return new GrupoRubroMotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Update(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_GrupoCategoriaMotivo> DarDeBaja(Comando_GrupoRubroMotivo comando)
        {
            ValidarSesion(Session);
            return new GrupoRubroMotivoRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Delete(comando);
        }
    }
}

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
    /// Descripción breve de TipoMovilService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class TipoMovilService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_TipoMovil> Insert(Comando_TipoMovil comando)
        {
            ValidarSesion(Session);
            return new TipoMovilRules( SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insert(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_TipoMovil> Editar(Comando_TipoMovil comando)
        {
            ValidarSesion(Session);
            return new TipoMovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Update(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_TipoMovil> DarDeBaja(Comando_TipoMovil comando)
        {
            ValidarSesion(Session);
            return new TipoMovilRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Delete(comando);
        }
    }
}

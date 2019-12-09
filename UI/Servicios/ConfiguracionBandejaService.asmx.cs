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
    /// Descripción breve de ConfiguracionBandejaService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class ConfiguracionBandejaService : _BaseService
    {

        //[WebMethod(EnableSession = true)]
        //public Result<bool> Insertar(Comando_ConfiguracionBandeja comando)
        //{
        //    ValidarSesion(Session);
        //    var resultado = new ConfiguracionBandejaPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insertar(comando);
        //    new UsuarioService().ActualizarDatosCerrojo();
        //    return resultado;
        //}

        [WebMethod(EnableSession = true)]
        public Result<Resultado_ConfiguracionPorArea> GetConfiguraciones(int idArea)
        {
            ValidarSesion(Session);
            var resultado = new ConfiguracionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetConfiguraciones(idArea);
            return resultado;
        }


        [WebMethod(EnableSession = true)]
        public Result<bool> SetConfiguraciones(Comando_ConfiguracionPorArea comando)
        {
            ValidarSesion(Session);
            var resultado = new ConfiguracionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).SetConfiguraciones(comando);
            new UsuarioService().ActualizarDatosCerrojo();
            return resultado;
        }
    }
}

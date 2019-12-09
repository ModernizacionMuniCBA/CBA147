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
    /// Descripción breve de FuncionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class FuncionService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Funcion>> GetByIdArea(int idArea)
        {
            ValidarSesion(Session);
            return new FuncionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetByIdArea(idArea);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Funcion> Insert(Comando_Funcion comando)
        {
            ValidarSesion(Session);
            return new FuncionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insert(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Funcion> Update(Comando_Funcion comando)
        {
            ValidarSesion(Session);
            return new FuncionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Update(comando);
        }


        [WebMethod(EnableSession = true)]
        public Result<Resultado_Funcion> DarDeBaja(Comando_Funcion comando)
        {
            ValidarSesion(Session);
            return new FuncionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeBaja(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Funcion> DarDeAlta(Comando_Funcion comando)
        {
            ValidarSesion(Session);
            return new FuncionPorAreaRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeAlta(comando);
        }



    }
}

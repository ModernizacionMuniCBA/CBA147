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
    /// Descripción breve de SeccionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class SeccionService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Seccion> Insertar(Comando_Seccion comando)
        {
            ValidarSesion(Session);
            return new SeccionRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Seccion> Update(Comando_Seccion comando)
        {
            ValidarSesion(Session);
            return new SeccionRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Update(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Seccion> DarDeBaja(int id)
        {
            ValidarSesion(Session);
            return new SeccionRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Seccion> DarDeAlta(int id)
        {
            ValidarSesion(Session);
            return new SeccionRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).DarDeAlta(id);
        }


        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Seccion>> GetByFilters(Consulta_Seccion consulta)
        {
            ValidarSesion(Session);
            return new SeccionRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetByFilters(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_Seccion>> GetByIdArea(int idArea)
        {
            ValidarSesion(Session);
            var consulta = new Consulta_Seccion(null, false, idArea);
            return new SeccionRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).GetByFilters(consulta);
        }
    }
}


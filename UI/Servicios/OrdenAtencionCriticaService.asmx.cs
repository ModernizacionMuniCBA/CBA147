using Model;
using Model.Comandos;
using Model.Consultas;
using Model.Entities;
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
    /// Descripción breve de OrdenAtencionCriticaService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class OrdenAtencionCriticaService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenAtencionCritica> Insertar(Comando_OrdenAtencionCritica comando)
        {
            ValidarSesion(Session);
            return new OrdenEspecialRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Insert(comando);
        }


        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenAtencionCritica> Editar(Comando_OrdenAtencionCritica comando)
        {
            ValidarSesion(Session);
            return new OrdenEspecialRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Update(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrdenAtencionCritica> Completar(Comando_OrdenAtencionCritica comando)
        {
            ValidarSesion(Session);
            return new OrdenEspecialRules(SessionKey.getUsuarioLogueado(HttpContext.Current.Session)).Completar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla<ResultadoTabla_OrdenAtencionCritica>> GetResultadoTabla(Consulta_OrdenAtencionCritica consulta)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrdenEspecialRules(userLogeado).GetResultadoTabla(consulta);
        }

        [WebMethod(EnableSession = true)]
        public Result<ResultadoTabla_OrdenAtencionCritica> GetResultadoTablaById(int id)
        {
            ValidarSesion(Session);
            var userLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var list = new List<int>();
            list.Add(id);

            var result = new Result<ResultadoTabla_OrdenAtencionCritica>();

            var resultado = new OrdenEspecialRules(userLogeado).GetResultadoTabla(list);
            if (!resultado.Ok)
            {
                result.Copy(resultado.Errores);
                return result;
            }

            if (resultado.Return.Data == null || resultado.Return.Data.Count == 0)
            {
                result.Return = null;
            }
            else
            {
                result.Return = resultado.Return.Data[0];
            }

            return result;
        }
    }
}

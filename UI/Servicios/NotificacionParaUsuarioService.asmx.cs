using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Model.Comandos;
using Model.Resultados;
using Model;
using Model.Consultas;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class NotificacionParaUsuarioService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_NotificacionSistema> Insertar(Comando_NotificacionParaUsuario comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new NotificacionParaUsuarioRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_NotificacionSistema> Editar(Comando_NotificacionParaUsuario comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new NotificacionParaUsuarioRules(userLogueado).Editar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_NotificacionSistema>> Get()
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var resultado = new NotificacionParaUsuarioRules(userLogueado).GetResultadoByFilters(new Consulta_NotificacionParaUsuario()
            {
                Notificar = true,
                DadosDeBaja = false
            });

            if (!resultado.Ok)
            {
                return resultado;
            }

            resultado.Return = resultado.Return.OrderBy(x => x.Titulo).ToList();
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public Result<int> GetCantidad()
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new NotificacionParaUsuarioRules(userLogueado).GetCantidadByFilters(new Consulta_NotificacionParaUsuario()
            {
                Notificar = true,
                DadosDeBaja = false
            });
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_NotificacionSistema>> GetTodo()
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            var resultado = new NotificacionParaUsuarioRules(userLogueado).GetResultadoByFilters(new Consulta_NotificacionParaUsuario()
            {
                DadosDeBaja = false
            });

            if (!resultado.Ok)
            {
                return resultado;
            }

            resultado.Return = resultado.Return.OrderBy(x => x.Titulo).ToList();
            return resultado;
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_NotificacionSistema> DarDeBaja(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new NotificacionParaUsuarioRules(userLogueado).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_NotificacionSistema> DarDeAlta(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new NotificacionParaUsuarioRules(userLogueado).DarDeAlta(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_NotificacionSistema> SetNotificar(int id, bool notificar)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new NotificacionParaUsuarioRules(userLogueado).SetNotificar(id, notificar);
        }

        //[WebMethod(EnableSession = true)]
        //public Result<List<Resultado_NotificacionParaUsuario>> CambiarPosicion(Comando_NotificacionParaUsuario_CambioPosicion comando)
        //{
        //    ValidarSesion(Session);

        //    var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
        //    return new NotificacionParaUsuarioRules(userLogueado).CambiarPosicion(comando);
        //}
    }
}

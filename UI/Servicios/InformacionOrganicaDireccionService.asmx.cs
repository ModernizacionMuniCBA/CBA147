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

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class InformacionOrganicaDireccionService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaDireccion> Insertar(Comando_InformacionOrganicaDireccion comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaDireccion> Actualizar(Comando_InformacionOrganicaDireccion comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).Actualizar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaDireccion> DarDeBaja(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaDireccion> DarDeAlta(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).DarDeAlta(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_InformacionOrganicaDireccion>> GetByIdSecretaria(int idSecretaria)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).GetResultadoByIdSecretaria(idSecretaria);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaDireccion> GetById(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).GetResultadoById(id);
        }



        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarNombre(int id, string nombre)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).CambiarNombre(id, nombre);
        }



        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarResponsable(int id, string responsable)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).CambiarResponsable(id, responsable);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarTelefono(int id, string telefono)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).CambiarTelefono(id, telefono);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarDomicilio(int id, string domicilio)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).CambiarDomicilio(id, domicilio);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarEmail(int id, string email)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaDireccionRules(userLogueado).CambiarEmail(id, email);
        }
    }
}

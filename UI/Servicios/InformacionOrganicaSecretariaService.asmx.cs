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
    public class InformacionOrganicaSecretariaService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaSecretaria> Insertar(Comando_InformacionOrganicaSecretaria comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaSecretariaRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaSecretaria> Actualizar(Comando_InformacionOrganicaSecretaria comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaSecretariaRules(userLogueado).Actualizar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaSecretaria> DarDeBaja(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaSecretariaRules(userLogueado).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaSecretaria> DarDeAlta(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaSecretariaRules(userLogueado).DarDeAlta(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_InformacionOrganicaSecretaria> GetById(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaSecretariaRules(userLogueado).GetResultadoById(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<List<Resultado_InformacionOrganicaSecretaria>> GetAll()
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaSecretariaRules(userLogueado).GetResultadoAll();
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> CambiarNombre(int id, string nombre)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaSecretariaRules(userLogueado).CambiarNombre(id, nombre);
        }

        [WebMethod(EnableSession = true)]
        public Result<bool> QuitarDireccion(int id, int idDireccion)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new InformacionOrganicaSecretariaRules(userLogueado).QuitarDireccion(id, idDireccion);
        }


    }
}

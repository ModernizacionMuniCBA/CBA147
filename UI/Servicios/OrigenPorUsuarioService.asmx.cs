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
    public class OrigenPorUsuarioService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrigenPorUsuario> Insertar(Comando_OrigenPorUsuario comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenPorUsuarioRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrigenPorUsuario> DarDeBaja(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenPorUsuarioRules(userLogueado).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrigenPorUsuario> DarDeAlta(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenPorUsuarioRules(userLogueado).DarDeAlta(id);
        }
    }
}

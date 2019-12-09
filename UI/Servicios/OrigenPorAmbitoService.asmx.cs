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
    public class OrigenPorAmbitoService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrigenPorAmbito> Insertar(Comando_OrigenPorAmbito comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenPorAmbitoRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrigenPorAmbito> DarDeBaja(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenPorAmbitoRules(userLogueado).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_OrigenPorAmbito> DarDeAlta(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenPorAmbitoRules(userLogueado).DarDeAlta(id);
        }
    }
}

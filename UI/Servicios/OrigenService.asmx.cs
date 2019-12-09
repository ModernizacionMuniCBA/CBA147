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
    public class OrigenService : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Origen> Insertar(Comando_Origen comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Origen> Editar(Comando_Origen comando)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenRules(userLogueado).Editar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Origen> DarDeBaja(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenRules(userLogueado).DarDeBaja(id);
        }

        [WebMethod(EnableSession = true)]
        public Result<Resultado_Origen> DarDeAlta(int id)
        {
            ValidarSesion(Session);

            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new OrigenRules(userLogueado).DarDeAlta(id);
        }
    }
}

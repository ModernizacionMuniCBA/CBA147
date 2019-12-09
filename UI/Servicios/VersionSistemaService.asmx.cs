using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model;
using Model.Resultados;
using Model.Comandos;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class VersionSistemaService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Result<bool> Insertar(Comando_VersionSistema comando)
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new VersionSistemaRules(userLogueado).Insertar(comando);
        }

        [WebMethod(EnableSession = true)]
        public Result<string> GetVersion()
        {
            ValidarSesion(Session);
            var userLogueado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new VersionSistemaRules(userLogueado).GetVersion();
        }
    }
}

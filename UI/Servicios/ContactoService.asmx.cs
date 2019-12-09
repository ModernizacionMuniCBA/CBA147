using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model;
using Model.Entities;
using Newtonsoft.Json;
using Rules;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Rules.Rules.Mails;
using Intranet_UI.Utils;
using Model.Resultados;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class ContactoService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Result<bool> EnviarMailContacto(string mail, string telefono, string mensaje)
        {
            ValidarSesion(Session);

            var user = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);
            return new ContactoMailRules(user).EnviarMailContacto(mensaje, mail, telefono);
        }

    }
}

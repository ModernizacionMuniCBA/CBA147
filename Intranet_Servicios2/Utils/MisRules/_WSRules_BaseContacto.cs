using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Model;
using Model.Entities;
using Rules.Rules.Reportes;
using Model.Resultados;
using Intranet_Servicios2;
using Rules.Rules;
using Rules;
using Intranet_Servicios2.MisRules;
using Rules.Rules.Mails;

namespace Intranet_Servicios2.Utils.MisRules
{
    public class _WSRules_BaseContacto : _WSRules_Base<BaseEntity>
    {
        private readonly ContactoMailRules rules;

        public _WSRules_BaseContacto(UsuarioLogueado data)
            : base(data)
        {
            rules = new ContactoMailRules(data);
        }

        public ResultadoServicio<bool> EnviarMailContacto(string mail, string telefono, string mensaje)
        {
            var result = new ResultadoServicio<bool>();
            result.Return = false;

            var resultMail = new ContactoMailRules(getUsuarioLogueado()).EnviarMailContacto(mensaje, mail, telefono);
            if (!resultMail.Ok)
            {
                result.Error = resultMail.ToStringPublico();
                return result;
            }

            result.Return = true;
            return result;
        }

    }
}

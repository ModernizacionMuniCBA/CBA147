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

namespace Intranet_Servicios2.MisRules
{
    public class _WSRules_BaseMotivo : _WSRules_Base<Motivo>
    {
        private readonly MotivoRules rules;

        public _WSRules_BaseMotivo(UsuarioLogueado data)
            : base(data)
        {
            rules = new MotivoRules(data);
        }

        public ResultadoServicio<List<Motivo>> GetByIdServicio(int id)
        {
            var resultado = new ResultadoServicio<List<Motivo>>();

            var resultadoMotivos = rules.GetDeServicio(id, null, false, false);
            if (!resultadoMotivos.Ok)
            {
                resultado.Error = resultadoMotivos.ToStringPublico();
                return resultado;
            }

            resultado.Return = resultadoMotivos.Return;
            return resultado;
        }

    }
}

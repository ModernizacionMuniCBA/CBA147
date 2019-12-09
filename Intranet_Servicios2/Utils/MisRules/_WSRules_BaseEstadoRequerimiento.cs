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
    public class _WSRules_BaseEstadoRequerimiento : _WSRules_Base<BaseEntity>
    {
        private readonly EstadoRequerimientoRules rules;

        public _WSRules_BaseEstadoRequerimiento(UsuarioLogueado data)
            : base(data)
        {
            rules = new EstadoRequerimientoRules(data);
        }

        public ResultadoServicio<List<EstadoRequerimiento>> GetEstadosPublicos()
        {
            var resultado = new ResultadoServicio<List<EstadoRequerimiento>>();

            var resultadoEstados = rules.GetAll(false);
            if (!resultadoEstados.Ok || resultadoEstados.Return == null)
            {
                resultado.Error = resultadoEstados.MessagesPublicos.ToString();
                return resultado;
            }

            var estados = resultadoEstados.Return.Where(x => !x.KeyValuePublico.HasValue).ToList();
            resultado.Return = estados;
            return resultado;
        }


    }
}

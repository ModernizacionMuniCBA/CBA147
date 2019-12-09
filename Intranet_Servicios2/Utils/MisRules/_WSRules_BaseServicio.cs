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
    public class _WSRules_BaseServicio : _WSRules_Base<Motivo>
    {
        private readonly ServicioRules rules;

        public _WSRules_BaseServicio(UsuarioLogueado data)
            : base(data)
        {
            rules = new ServicioRules(data);
        }

        public ResultadoServicio<List<Servicio>> GetAll()
        {
            var resultado = new ResultadoServicio<List<Servicio>>();

            var tipos=new List<Enums.TipoMotivo>();
            tipos.Add(Enums.TipoMotivo.GENERAL);
            var resultadoMotivos = rules.GetByFilters(tipos, false);
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

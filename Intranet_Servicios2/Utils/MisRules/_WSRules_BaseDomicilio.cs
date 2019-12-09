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
    public class _WSRules_BaseDomicilio : _WSRules_Base<BaseEntity>
    {
        private readonly DomicilioRules rules;

        public _WSRules_BaseDomicilio(UsuarioLogueado data)
            : base(data)
        {
            rules = new DomicilioRules(data);
        }

        public ResultadoServicio<List<Domicilio>> Sugerir(string busqueda)
        {
            var resultado = new ResultadoServicio<List<Domicilio>>();

            var resultadoDomicilio = rules.Sugerir(busqueda);

            if (!resultadoDomicilio.Ok)
            {
                resultado.Error = resultadoDomicilio.Errores.ToStringPublico();
                return resultado;
            }

            if (resultadoDomicilio.Return == null)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            resultado.Return = resultadoDomicilio.Return;
            return resultado;
        }

        public ResultadoServicio<Domicilio> Buscar(double lat, double lng)
        {
            var resultado = new ResultadoServicio<Domicilio>();

            var resultadoDomicilio = rules.Buscar(lat, lng);

            if (!resultadoDomicilio.Ok)
            {
                resultado.Error = resultadoDomicilio.Errores.ToStringPublico();
                return resultado;
            }

            if (resultadoDomicilio.Return == null)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            //Agrego las observaciones
            resultado.Return = resultadoDomicilio.Return;
            return resultado;
        }
    }
}

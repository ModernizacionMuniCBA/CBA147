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
using Intranet_Servicios2.Utils.MisRules;
using Intranet_Servicios2.v1.Entities.Resultados;

namespace Intranet_Servicios2.v1.MisRules
{
    public class WSRules_Domicilio : _WSRules_Base<Domicilio>
    {

        private readonly DomicilioRules rules;
        private readonly _WSRules_BaseDomicilio rulesBase;

        public WSRules_Domicilio(UsuarioLogueado data)
            : base(data)
        {
            rules = new DomicilioRules(data);
            rulesBase = new _WSRules_BaseDomicilio(data);
        }

        public ResultadoServicio<List<ResultadoApp_Domicilio>> Sugerir(string busqueda)
        {
            var resultado = new ResultadoServicio<List<ResultadoApp_Domicilio>>();

            //Lamo al base
            var resultadoDomicilio = rulesBase.Sugerir(busqueda);
            if (!resultadoDomicilio.Ok)
            {
                resultado.Error = resultadoDomicilio.Error;
                return resultado;
            }

            if (resultadoDomicilio.Return == null)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            resultado.Return = ResultadoApp_Domicilio.ToList(resultadoDomicilio.Return);
            return resultado;
        }

        public ResultadoServicio<ResultadoApp_Domicilio> Buscar(double lat, double lng)
        {
            var resultado = new ResultadoServicio<ResultadoApp_Domicilio>();

            //Llamo al base
            var resultadoDomicilio = rulesBase.Buscar(lat, lng);
            if (!resultadoDomicilio.Ok)
            {
                resultado.Error = resultadoDomicilio.Error;
                return resultado;
            }

            if (resultadoDomicilio.Return == null)
            {
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }

            //Agrego las observaciones
            resultado.Return = new ResultadoApp_Domicilio(resultadoDomicilio.Return);
            return resultado;
        }

    }
}

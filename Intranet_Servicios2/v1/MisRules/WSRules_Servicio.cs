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

namespace Intranet_Servicios2.v1.MisRules
{
    public class WSRules_Servicio : _WSRules_Base<Servicio>
    {

        private readonly ServicioRules rules;
        private readonly _WSRules_BaseServicio rulesBase;

        public WSRules_Servicio(UsuarioLogueado data)
            : base(data)
        {
            rules = new ServicioRules(data);
            rulesBase = new _WSRules_BaseServicio(data);
        }

        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Servicio>> GetAll()
        {
            var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Servicio>>();

            //Llamo al base
            var resultadoServicios = rulesBase.GetAll();
            if (!resultadoServicios.Ok)
            {
                resultado.Error = resultadoServicios.Error;
                return resultado;
            }

            //Transformo
            var servicios = v1.Entities.Resultados.ResultadoApp_Servicio.ToList(resultadoServicios.Return);

            //Ordeno
            servicios = servicios.OrderBy(x => x.Nombre).ToList();

            //Devuelvo
            resultado.Return = servicios;
            return resultado;
        }
    }
}

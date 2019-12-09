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
    public class WSRules_Motivo : _WSRules_Base<Motivo>
    {
        private readonly MotivoRules rules;
        private readonly _WSRules_BaseMotivo rulesBase;

        public WSRules_Motivo(UsuarioLogueado data)
            : base(data)
        {
            rules = new MotivoRules(data);
            rulesBase = new _WSRules_BaseMotivo(data);
        }

        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Motivo>> GetByIdServicio(int id)
        {
            var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Motivo>>();

            //Invoco el base
            var resultadoMotivos = rulesBase.GetByIdServicio(id);
            if (!resultadoMotivos.Ok)
            {
                resultado.Error = resultadoMotivos.Error;
                return resultado;
            }

            //Transformo segun version
            var motivos = v1.Entities.Resultados.ResultadoApp_Motivo.ToList(resultadoMotivos.Return);
            motivos = motivos.OrderBy(x => x.Nombre).ToList();
            resultado.Return = motivos;
            return resultado;
        }

        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_ServicioMotivoParaBusqueda>> GetParaBusqueda()
        {
            var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_ServicioMotivoParaBusqueda>>();

            try
            {
                var resultadoConsulta = rules.ProcedimientoAlmacenado<v1.Entities.Resultados.ResultadoApp_ServicioMotivoParaBusqueda>("WS_App_MotivosParaConsulta_v1");
                if (!resultadoConsulta.Ok)
                {
                    resultado.Error = resultadoConsulta.Errores.ToStringPublico();
                    return resultado;
                }

                resultado.Return = resultadoConsulta.Return;
            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
            }
            return resultado;
        }
    }
}

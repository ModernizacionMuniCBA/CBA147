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
using Model.Resultados.Estadisticas;

namespace Intranet_Servicios2.Utils.MisRules
{
    public class _WSRules_BaseEstadistica : _WSRules_Base<BaseEntity>
    {
        private readonly EstadisticaRules rules;

        public _WSRules_BaseEstadistica(UsuarioLogueado data)
            : base(data)
        {
            rules = new EstadisticaRules(data);
        }

        public ResultadoServicio<Resultado_DatosEstadisticaPanel> GetDatosEstadisticaPanel(DateTime fechaDesde, DateTime fechaHasta)
        {
            var resultado = new ResultadoServicio<Resultado_DatosEstadisticaPanel>();

            //Result par las estadisticas
            var resultData = rules.GetDatosEstadistica(fechaDesde, fechaHasta);
            if (!resultData.Ok)
            {
                resultado.Error = resultData.Errores.ToStringPublico();
                return resultado;
            }
            resultado.Return = resultData.Return;
            return resultado;
        }

        //public ResultServicio<List<Resultado_DatosEstadisticaPanel_Cpc>> GetMapaCritico(DateTime fechaDesde, DateTime fechaHasta)
        //{
        //    var resultado = new ResultServicio<List<Resultado_DatosEstadisticaPanel_Cpc>>();

        //    //Result para las estadisticas
        //    var resultData = rules.GetMapaCritico(Enums.TipoRequerimiento.RECLAMO, fechaDesde, fechaHasta);
        //    if (!resultData.Ok)
        //    {
        //        resultado.Error = resultData.Errores.ToStringPublico();
        //        return resultado;
        //    }
        //    resultado.Return = resultData.Return;
        //    return resultado;
        //}

        public ResultadoServicio<bool?> ValidarUsuarioEstadisticasTV()
        {
            var resultado = new ResultadoServicio<bool?>();

            var resultadoPermiso =  new CerrojoUsuarioEstadisticaTVRules(getUsuarioLogueado()).TengoPermiso();
            if (!resultadoPermiso.Ok)
            {
                resultado.Error = resultadoPermiso.Errores.ToStringPublico();
                return resultado;
            }

            resultado.Return = resultadoPermiso.Return;
            return resultado;
        }
    }
}

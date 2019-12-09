using Intranet_Servicios2.Utils.Controllers;
using Model.Resultados;
using Model.Resultados.Estadisticas;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Intranet_Servicios2.v1.Controllers
{
    public class Estadistica_v1Controller : Control
    {
        const string routeBase = "v1/Estadistica";

        [HttpGet]
        [Route(routeBase + "/Panel")]
        public ResultadoServicio<Resultado_DatosEstadisticaPanel> GetDatosEstadisticaPanel(string token, DateTime fechaDesde, DateTime fechaHasta)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<Resultado_DatosEstadisticaPanel>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Estadisticas(resultadoUser.Return).GetDatosEstadisticaPanel(fechaDesde, fechaHasta);
        }

        //[HttpGet]
        //[Route(routeBase + "/MapaCritico")]
        //public ResultServicio<List<Resultado_DatosEstadisticaPanel_Cpc>> GetMapaCritica(string token, DateTime fechaDesde, DateTime fechaHasta)
        //{
        //    var resultadoUser = GetUser(token);
        //    if (!resultadoUser.Ok)
        //    {
        //        var resultado = new ResultServicio<List<Resultado_DatosEstadisticaPanel_Cpc>>();
        //        resultado.Error = resultadoUser.Error;
        //        return resultado;
        //    }

        //    return new v1.MisRules.WSRules_Estadisticas(resultadoUser.Return).GetMapaCritico(fechaDesde, fechaHasta);
        //}

        [HttpGet]
        [Route(routeBase + "/ValidarPermisoEstadisticasTV")]
        public ResultadoServicio<bool?> ValidarPermisoEstadisticasTV(string token)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool?>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Estadisticas(resultadoUser.Return).ValidarUsuarioEstadisticasTV();
        }

    }
}
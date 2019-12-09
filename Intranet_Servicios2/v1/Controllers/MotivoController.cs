using Intranet_Servicios2.Utils.Controllers;
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
    public class Motivo_v1Controller : Control
    {
        const string routeBase = "v1/Motivo";

        [HttpGet]
        [Route(routeBase + "/GetByIdServicio")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Motivo>> GetAllByIdServicio(string token, int id)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Motivo>>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Motivo(resultadoUser.Return).GetByIdServicio(id);
        }

        [HttpGet]
        [Route(routeBase + "/GetParaBusqueda")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_ServicioMotivoParaBusqueda>> GetParaBusqueda(string token)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_ServicioMotivoParaBusqueda>>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Motivo(resultadoUser.Return).GetParaBusqueda();
        }

    }
}
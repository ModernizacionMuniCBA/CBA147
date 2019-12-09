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
    public class Domicilio_v1Controller : Control
    {
        const string routeBase = "v1/Domicilio";

        [HttpGet]
        [Route(routeBase + "/Sugerir")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Domicilio>> Sugerir(string token, string busqueda)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Domicilio>>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Domicilio(resultadoUser.Return).Sugerir(busqueda);
        }

        [HttpGet]
        [Route(routeBase + "/Buscar")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Domicilio> GetParaBusqueda(string token, double latitud, double longitud)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Domicilio>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Domicilio(resultadoUser.Return).Buscar(latitud, longitud);
        }

    }
}
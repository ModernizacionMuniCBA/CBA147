using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Intranet_Servicios2.Utils.Controllers;
using Intranet_Servicios2.Utils.Controllers.ActionFilters;

namespace Intranet_Servicios2.v1.Controllers
{
    [RoutePrefix("v2/Domicilio")]
    public class Domicilio_v2Controller : Control
    {
        [HttpGet]
        [ConToken]
        [Route("Sugerir")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Domicilio>> Sugerir(string busqueda)
        {
            var resultadoUser = GetUsuarioLogeado();
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Domicilio>>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Domicilio(resultadoUser.Return).Sugerir(busqueda);
        }

        [HttpGet]
        [ConToken]
        [Route("Buscar")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_Domicilio> GetParaBusqueda(double latitud, double longitud)
        {
            var resultadoUser = GetUsuarioLogeado();
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
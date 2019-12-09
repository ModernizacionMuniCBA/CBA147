using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Internet_Servicios.Utils;
using Internet_Servicios.V1.Entities.Resultados;
using Internet_Servicios.V1.Entities.Comandos;

namespace Internet_Servicios.V1.Controllers
{
    [RoutePrefix("v1/Domicilio")]
    public class Domicilio_v1Controller : ApiController
    {
        [HttpGet]
        [Route("Sugerir")]
        public ResultServicio<List<ResultadoApp_Domicilio>> Sugerir(string token, string busqueda)
        {
            return RestCall.Call<List<ResultadoApp_Domicilio>>(Request);
        }

        [HttpGet]
        [Route("Buscar")]
        public ResultServicio<ResultadoApp_Domicilio> GetParaBusqueda(string token, double latitud, double longitud)
        {
            return RestCall.Call<ResultadoApp_Domicilio>(Request);
        }
    }
}
using Internet_Servicios.Utils;
using Internet_Servicios.V1.Entities.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Internet_Servicios.V1.Controllers
{
    [RoutePrefix("v1/Servicio")]
    public class Servicio_v1Controller : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public ResultServicio<List<ResultadoApp_Servicio>> GetAll(string token)
        {
            return RestCall.Call<List<ResultadoApp_Servicio>>(Request);
        }
    }
}
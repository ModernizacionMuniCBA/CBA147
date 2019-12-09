using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Internet_Servicios.Utils;
using Internet_Servicios.V1.Entities.Resultados;

namespace Internet_Servicios.V1.Controllers
{
    [RoutePrefix("v1/Motivo")]
    public class Motivo_v1Controller : ApiController
    {
        [HttpGet]
        [Route("GetByIdServicio")]
        public ResultServicio<List<ResultadoApp_Motivo>> GetByIdServicio(string token, int id)
        {
            return RestCall.Call<List<ResultadoApp_Motivo>>(Request);
        }

        [HttpGet]
        [Route("GetParaBusqueda")]
        public ResultServicio<List<ResultadoApp_ServicioMotivoParaBusqueda>> GetParaBusqueda(string token)
        {
            return RestCall.Call<List<ResultadoApp_ServicioMotivoParaBusqueda>>(Request);
        }
    }
}
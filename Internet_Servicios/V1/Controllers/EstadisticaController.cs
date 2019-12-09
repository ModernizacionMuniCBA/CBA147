using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Internet_Servicios.Utils;

namespace Internet_Servicios.V1.Controllers
{
    [RoutePrefix("v1/Estadistica")]
    public class Estadistica_v1Controller : ApiController
    {
        //[HttpGet]
        //[Route("Panel")]
        //public ResultServicio<Resultado_DatosEstadisticaPanel> GetDatosEstadisticaPanel(string token, DateTime fechaDesde, DateTime fechaHasta)
        //{
        //    return RestCall.Call<Resultado_DatosEstadisticaPanel>(Request);
        //}

        //[HttpGet]
        //[Route("MapaCritico")]
        //public ResultServicio<List<Resultado_DatosEstadisticaPanel_Cpc>> GetMapaCritica(string token, DateTime fechaDesde, DateTime fechaHasta)
        //{
        //    return RestCall.Call<List<Resultado_DatosEstadisticaPanel_Cpc>>(Request);
        //}

        //[HttpGet]
        //[Route("ValidarPermisoEstadisticasTV")]
        //public ResultServicio<bool?> ValidarPermisoEstadisticasTV(string token)
        //{
        //    return RestCall.Call<bool?>(Request);
        //}
    }
}
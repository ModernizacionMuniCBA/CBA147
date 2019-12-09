using Intranet_Servicios2.Utils.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Intranet_Servicios2.v1.Controllers
{
    public class Servicio_v1Controller : Control
    {
        const string routeBase = "v1/Servicio";


        [HttpGet]
        [Route(routeBase + "/GetAll")]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Servicio>> GetAll(string token)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_Servicio>>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Servicio(resultadoUser.Return).GetAll();
        }
    }
}
using System;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Internet_Servicios.Utils;

namespace Internet_Servicios.V1.Controllers
{
    /// <summary>Controlador para ajustes</summary>
    [RoutePrefix("v1/Ajustes")]
    public class Ajustes_v1Controller : ApiController
    {
        /// <summary>Devuelve los ajustes para la app</summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AppData")]
        public ResultServicio<JObject> GetAppData()
        {
            return RestCall.Call<JObject>(Request);
        }
    }
}
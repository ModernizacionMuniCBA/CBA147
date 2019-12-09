using System.Configuration;
using Intranet_Servicios2.Utils.Controllers;
using System;
using System.Linq;
using System.Web.Http;
using Intranet_Servicios2.v1.MisRules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Intranet_Servicios2.Utils.Controllers.ActionFilters;

namespace Intranet_Servicios2.v1.Controllers
{
    /// <summary>Controlador para ajustes</summary>
    [RoutePrefix("v1/Ajustes")]
    public class Ajustes_v1Controller : Control
    {
        /// <summary>Devuelve los ajustes para la app</summary>
        /// <param name="token">Token de Vecino Virtual</param>
        /// <returns></returns>
        [HttpGet]
        [Route("AppData")]
        [ValidarApp]
        public ResultadoServicio<JObject> GetAppData()
        {
            return new MisRules.WSRules_Ajustes(null).GetAppData();
        }
    }
}
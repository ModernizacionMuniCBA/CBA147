using Internet_Servicios;
using Internet_UI.Utils;
using System;
using System.Linq;
using System.Web.Services;
using Newtonsoft.Json.Linq;

namespace Internet_UI.Servicios
{
    [System.Web.Script.Services.ScriptService]
    public class ServicioAjustes : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Resultado<JObject> GetAppData()
        {
            var url = "/v1/Ajustes/AppData";
            return RestCall.Call<JObject>(url, RestSharp.Portable.Method.GET, null, true);
        }
    }
}
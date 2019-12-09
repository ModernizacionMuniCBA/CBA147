using Internet_Servicios;
using Internet_UI.Utils;
using InternetUI_Entities.Comandos;
using InternetUI_Entities.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace Internet_UI.Servicios
{
    public class ServicioEstadoCivil : _BaseService
    {
        [WebMethod]
        public Resultado<List<ResultadoApp_EstadoCivil>> Get()
        {
            
            return RestCall.Call<List<ResultadoApp_EstadoCivil>>(url, RestSharp.Portable.Method.GET, null, false, false);
        }
    }
}
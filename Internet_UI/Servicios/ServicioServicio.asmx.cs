using Internet_Servicios;
using Internet_UI.Utils;
using InternetUI_Entities.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace Internet_UI.Servicios
{
    public class ServicioServicio : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Resultado<List<ResultadoApp_Servicio>> Get()
        {
            var url = "v1/Servicio/GetAll?token=" + Session[Consts.TOKEN];
            return RestCall.Call<List<ResultadoApp_Servicio>>(url, RestSharp.Portable.Method.GET);
        }
    }
}
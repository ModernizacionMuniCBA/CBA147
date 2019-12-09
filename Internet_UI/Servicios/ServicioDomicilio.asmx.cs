using Internet_Servicios;
using Internet_UI.Utils;
using InternetUI_Entities.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace Internet_UI.Servicios
{
    public class ServicioDomicilio : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Resultado<List<ResultadoApp_Domicilio>> Sugerir(string consulta)
        {
            var url = "v1/Domicilio/Sugerir?token=" + Session[Consts.TOKEN] + "&busqueda=" + consulta;
            return RestCall.Call<List<ResultadoApp_Domicilio>>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<ResultadoApp_Domicilio> Buscar(double lat, double lng)
        {
            var url = "v1/Domicilio/Buscar?token=" + Session[Consts.TOKEN] + "&latitud=" + lat + "&longitud=" + lng;
            url = url.Replace(",", ".");
            return RestCall.Call<ResultadoApp_Domicilio>(url, RestSharp.Portable.Method.GET);
        }
    }
}
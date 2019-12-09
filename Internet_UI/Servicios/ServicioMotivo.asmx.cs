using Internet_Servicios;
using Internet_UI.Utils;
using InternetUI_Entities.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace Internet_UI.Servicios
{
    public class ServicioMotivo : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Resultado<List<ResultadoApp_ServicioMotivoParaBusqueda>> GetParaBusqueda()
        {
            var url = "v1/Motivo/GetParaBusqueda?token=" + Session[Consts.TOKEN];
            return RestCall.Call<List<ResultadoApp_ServicioMotivoParaBusqueda>>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<List<ResultadoApp_Motivo>> GetByIdServicio(int id)
        {
            var token = Session[Consts.TOKEN];
            var url = "v1/Motivo/GetByIdServicio?id=" + id + "&token=" + token;
            return RestCall.Call<List<ResultadoApp_Motivo>>(url, RestSharp.Portable.Method.GET);
        }
    }
}
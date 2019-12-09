using Internet_Servicios;
using Internet_UI.Utils;
using InternetUI_Entities.Comandos;
using InternetUI_Entities.Resultados;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Services;

namespace Internet_UI.Servicios
{
    public class ServicioRequerimiento : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Resultado<ResultadoApp_RequerimientoInsertado> Insertar(ComandoApp_Requerimiento comando)
        {
            var url = "v1/Requerimiento?token=" + Session[Consts.TOKEN];

            comando.Autenticacion = new ComandoApp_RequerimientoAutenticacion();
            comando.Autenticacion.OrigenAlias = ConfigurationManager.AppSettings["ORIGEN_WEB_KEY_ALIAS"];
            comando.Autenticacion.OrigenKey = ConfigurationManager.AppSettings["ORIGEN_WEB_KEY_SECRET"];

            return RestCall.Call<ResultadoApp_RequerimientoInsertado>(url, RestSharp.Portable.Method.POST, comando);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<bool> EnviarEmailComprobante(int id)
        {
            var url = "v1/Requerimiento/EnviarEmailComprobante?token=" + Session[Consts.TOKEN] + "&idRequerimiento=" + id;
            return RestCall.Call<bool>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<List<ResultadoApp_RequerimientoListado>> GetMisRequerimientos()
        {
            var url = "v1/Requerimiento?token=" + Session[Consts.TOKEN];
            return RestCall.Call<List<ResultadoApp_RequerimientoListado>>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<ResultadoApp_RequerimientoDetalle> GetDetalle(int id)
        {
            var url = "v1/Requerimiento/Detalle?id=" + id + "&token=" + Session[Consts.TOKEN];
            return RestCall.Call<ResultadoApp_RequerimientoDetalle>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<bool?> Cancelar(int id)
        {
            var url = "v1/Requerimiento/Cancelar?idRequerimiento=" + id + "&token=" + Session[Consts.TOKEN];
            return RestCall.Call<bool?>(url, RestSharp.Portable.Method.GET);
        }

    }
}
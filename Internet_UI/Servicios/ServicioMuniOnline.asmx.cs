using Internet_Servicios;
using Internet_UI.Utils;
using InternetUI_Entities.Resultados;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Internet_UI.Servicios
{
    /// <summary>
    /// Descripción breve de ServicioVecinoVirtual
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
     [System.Web.Script.Services.ScriptService]
    public class ServicioMuniOnline : _BaseService
    {

        [WebMethod(EnableSession = true)]
        public Resultado<List<ResultadoApp_AppMuniOnline>> GetAppsMuniOnline()
        {
            var url = ConfigurationManager.AppSettings["URL_VECINOVIRTUAL_PANEL"] + "/v2/AplicacionPanel";
            return RestCall.Call<List<ResultadoApp_AppMuniOnline>>(url, RestSharp.Portable.Method.GET,null, false, false);
        }
    }
}

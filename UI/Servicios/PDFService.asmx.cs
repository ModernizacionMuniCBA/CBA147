using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Model;
using Model.Entities;
using Rules.Rules;
using Intranet_UI.Utils;

namespace UI.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class PDFService : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public string GuardarPDF(string url)
        {
            ValidarSesion(Session);

            var resultado = new Dictionary<string, object>();

            try
            {
                // Create a new WebClient instance.
                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.Credentials = new NetworkCredential("sigo", "51g0");
                    myWebClient.DownloadFile(url, @"C:\Users\amura_f\Desktop\1.pdf");
                }

            }
            catch (Exception e)
            {
                var error = new Result<string>();
                error.AddErrorInterno(e);
                resultado.Add("Error", error);
            }
            
            return JsonUtils.toJson(resultado);
        }
    }
}

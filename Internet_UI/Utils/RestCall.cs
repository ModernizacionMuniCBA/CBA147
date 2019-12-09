using Internet_UI.Utils;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Configuration;
using System.Linq;

namespace Internet_Servicios
{
    public class RestCall
    {

        public static RestClient ApiClient(string url, bool baseUrl = true)
        {
            var urlServer = ConfigurationManager.AppSettings["URL_SERVER"];
            if (urlServer == null) return null;

            string urlFinal = url;
            if (baseUrl)
            {
                urlFinal = urlServer + "/" + urlFinal;
            }
            var client = new RestClient(urlFinal);
            return client;
        }

        public static RestRequest ApiRequest(Method method, bool addCustomHeader)
        {
            var request = new RestRequest(method);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");

            if (addCustomHeader)
            {
                request.AddHeader("Identificador", ConfigurationManager.AppSettings["DESDEAPP_IDENTIFICADOR"]);
                request.AddHeader("Key", ConfigurationManager.AppSettings["DESDEAPP_KEY"]);
            }

            return request;
        }

        public static Resultado<T> Call<T>(string url, Method metodo, object body = null, bool addCustomHeader = false, bool baseUrl = true)
        {
            try
            {
                var client = ApiClient(url, baseUrl);
                var request = ApiRequest(metodo, addCustomHeader);
                if (client == null || request == null)
                {
                    var resultado = new Resultado<T>();
                    resultado.Error = "Error procesando la solicitud";
                    return resultado;
                }

                if (body != null)
                {
                    request.AddBody(body);
                }

                IRestResponse response = client.Execute(request).Result;
                var json = JObject.Parse(response.Content);

                //Devuelvo
                return json.ToObject<Resultado<T>>();

            }
            catch (Exception e)
            {
                var resultado = new Resultado<T>();
                resultado.Error = "Error procesando la solicitud";
                return resultado;
            }
        }
    }
}
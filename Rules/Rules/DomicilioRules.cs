using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Comandos;
using Model.Resultados;
using Model.Utiles;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using RestSharp;
using Model.Consultas;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Microsoft.VisualBasic.Logging;

namespace Rules.Rules
{
    public class DomicilioRules : BaseRules<Domicilio>
    {

        private readonly DomicilioDAO dao;

        public DomicilioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = DomicilioDAO.Instance;
        }


        public Result<Domicilio> Buscar(double lat, double lng)
        {
            var resultado = new Result<Domicilio>();

            dao.Transaction(() =>
            {
                try
                {
                    MiLog.Info("Por buscar domicilio a traves de lat y lng");
                    MiLog.Info("Lat: " + lat);
                    MiLog.Info("Lng: " + lng);


                    //Debe mandar las 2 coordenadas, o ninguna
                    if (lat == 0 || lng == 0)
                    {
                        resultado.AddErrorPublico("Domicilio inválido");
                        return false;
                    }

                    string baseUrl = ConfigurationManager.AppSettings["URL_CORDOBA_GEO_API"];
                    if (baseUrl == null)
                    {
                        resultado.AddErrorInterno("Mal configurado las variables del webconfig: URL_CORDOBA_GEO_API");
                        return false;
                    }

                    string keyGoogleMaps = ConfigurationManager.AppSettings["KEY_GOOGLE_MAPS"];
                    if (keyGoogleMaps == null)
                    {
                        resultado.AddErrorInterno("Mal configurado las variables del webconfig: KEY_GOOGLE_MAPS");
                        return false;
                    }

                    string url = baseUrl + "/info/coordenada?lat=" + lat + "&lng=" + lng + "&googleMapsKey=" + keyGoogleMaps;
                    url = url.Replace(",", ".");

                    //Busco en el api
                    string response = null;

                    using (WebClient client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                        var responseStream = new GZipStream(client.OpenRead(url), CompressionMode.Decompress);
                        var reader = new StreamReader(responseStream);
                        response = reader.ReadToEnd();
                    }

                    MiLog.Info("Response " + response);

                    if (response == null)
                    {
                        MiLog.Info("Sin respuesta");
                        resultado.AddErrorInterno("Sin respuesta");
                        return false;
                    }


                    MiLog.Info("Parseando");
                    JObject obj = JsonConvert.DeserializeObject<JObject>(response);
                    if (obj.GetValue("estado").ToObject<string>() != "OK")
                    {
                        MiLog.Info("Estado distinto a OK");
                        resultado.AddErrorPublico(obj.GetValue("error").ToObject<string>());
                        return false;
                    }

                    MiLog.Info("Busco elementos");
                    JObject info = obj.GetValue("info").ToObject<JObject>();
                    JObject infoBarrio = info.GetValue("barrio").ToObject<JObject>();
                    JObject infoCpc = info.GetValue("cpc").ToObject<JObject>();
                    JObject infoDireccion = info.GetValue("direccion").ToObject<JObject>();

                    if (infoBarrio == null)
                    {
                        MiLog.Info("Info barrio es null");
                        resultado.AddErrorPublico("En la ubicación indicada no hay un barrio");
                        return false;
                    }

                    //Barrio
                    MiLog.Info("Por buscar barrio");
                    var barrio_idCatastro = infoBarrio.GetValue("id").ToObject<int>();
                    var resultadoBarrio = new BarrioRules(getUsuarioLogueado()).Buscar(barrio_idCatastro);
                    if (!resultadoBarrio.Ok)
                    {
                        MiLog.Info("Error buscando el barrio");
                        resultado.Copy(resultadoBarrio.Errores);
                        return false;
                    }

                    //Cpc
                    MiLog.Info("Por buscar cpc");
                    var cpc_idCatastro = infoCpc.GetValue("id").ToObject<int>();
                    var resultadoCpc = new CpcRules(getUsuarioLogueado()).Buscar(cpc_idCatastro);
                    if (!resultadoCpc.Ok)
                    {
                        MiLog.Info("Error buscando el cpc");
                        resultado.Copy(resultadoCpc.Errores);
                        return false;
                    }

                    //Direccion
                    MiLog.Info("Por buscar direccion");
                    string direccion_nombre = null;
                    int direccion_distancia = 0;

                    if (infoDireccion != null)
                    {
                        direccion_nombre = infoDireccion.GetValue("nombre").ToObject<string>();
                        direccion_distancia = infoDireccion.GetValue("distancia").ToObject<int>();
                    }

                    var domicilio = new Domicilio();
                    domicilio.Sugerido = true;
                    domicilio.Direccion = direccion_nombre;
                    domicilio.Distancia = direccion_distancia;
                    domicilio.Observaciones = null;
                    domicilio.Cpc = resultadoCpc.Return;
                    domicilio.Barrio = resultadoBarrio.Return;
                    domicilio.Latitud = ("" + lat).Replace(".", ",");
                    domicilio.Longitud = ("" + lng).Replace(".", ",");
                    resultado.Return = domicilio;

                    MiLog.Info("Domicilio encontrado");
                    return true;
                }
                catch (Exception e)
                {
                    resultado.AddErrorInterno(e);
                    return false;
                }
            });

            return resultado;
        }

        public Result<Resultado_Domicilio> BuscarResultado(double lat, double lng)
        {
            var resultado = new Result<Resultado_Domicilio>();

            var resultadoBuscar = Buscar(lat, lng);
            if (!resultadoBuscar.Ok)
            {
                resultado.Copy(resultadoBuscar.Errores);
                return resultado;
            }

            if (resultadoBuscar.Return != null)
            {
                resultado.Return = new Resultado_Domicilio(resultadoBuscar.Return);
            }

            return resultado;
        }

        public Result<List<Domicilio>> Sugerir(string busqueda)
        {
            var resultado = new Result<List<Domicilio>>();

            dao.Transaction(() =>
            {
                try
                {
                    //Debe mandar la busqueda
                    if (string.IsNullOrEmpty(busqueda))
                    {
                        resultado.AddErrorPublico("Domicilio inválido");
                        return false;
                    }

                    string baseUrl = ConfigurationManager.AppSettings["URL_CORDOBA_GEO_API"];
                    if (baseUrl == null)
                    {
                        resultado.AddErrorInterno("Mal configurado las variables del webconfig: URL_CORDOBA_GEO_API");
                        return false;
                    }

                    string keyGoogleMaps = ConfigurationManager.AppSettings["KEY_GOOGLE_MAPS"];
                    if (keyGoogleMaps == null)
                    {
                        resultado.AddErrorInterno("Mal configurado las variables del webconfig: KEY_GOOGLE_MAPS");
                        return false;
                    }

                    string url = baseUrl + "/info/buscar/" + busqueda + "?googleMapsKey=" + keyGoogleMaps;

                    //Busco en el api
                    string response = null;

                    using (WebClient client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                        var responseStream = new GZipStream(client.OpenRead(url), CompressionMode.Decompress);
                        var reader = new StreamReader(responseStream);
                        response = reader.ReadToEnd();
                    }

                    if (response == null)
                    {
                        return false;
                    }

                    JObject obj = JsonConvert.DeserializeObject<JObject>(response);
                    if (obj.GetValue("estado").ToObject<string>() != "OK")
                    {
                        resultado.AddErrorPublico(obj.GetValue("error").ToObject<string>());
                        return false;
                    }

                    var domicilios = new List<Domicilio>();

                    JArray infoArray = obj.GetValue("info").ToObject<JArray>();
                    if (infoArray.Count == 0)
                    {
                        resultado.AddErrorPublico("Sin resultados");
                        return false;
                    }

                    foreach (JObject info in infoArray)
                    {
                        JObject infoBarrio = info.GetValue("barrio").ToObject<JObject>();
                        JObject infoCpc = info.GetValue("cpc").ToObject<JObject>();
                        JObject infoDireccion = info.GetValue("direccion").ToObject<JObject>();
                        JObject latLang = info.GetValue("latLng").ToObject<JObject>();

                        if (infoBarrio == null)
                        {
                            resultado.AddErrorPublico("En la ubicación indicada no hay un barrio");
                            return false;
                        }

                        //Barrio
                        var barrio_idCatastro = infoBarrio.GetValue("id").ToObject<int>();
                        var resultadoBarrio = new BarrioRules(getUsuarioLogueado()).Buscar(barrio_idCatastro);
                        if (!resultadoBarrio.Ok)
                        {
                            resultado.Copy(resultadoBarrio.Errores);
                            return false;
                        }

                        //Cpc
                        var cpc_idCatastro = infoCpc.GetValue("id").ToObject<int>();
                        var resultadoCpc = new CpcRules(getUsuarioLogueado()).Buscar(cpc_idCatastro);
                        if (!resultadoCpc.Ok)
                        {
                            resultado.Copy(resultadoCpc.Errores);
                            return false;
                        }

                        //Direccion
                        string direccion_nombre = null;
                        int direccion_distancia = 0;

                        if (infoDireccion != null)
                        {
                            direccion_nombre = infoDireccion.GetValue("nombre").ToObject<string>();
                            direccion_distancia = infoDireccion.GetValue("distancia").ToObject<int>();
                        }

                        //LatLng
                        var lat = latLang.GetValue("lat").ToObject<double>();
                        var lng = latLang.GetValue("lng").ToObject<double>();

                        //Observacion
                        var observacion = info.GetValue("observacion").ToObject<string>();

                        var domicilio = new Domicilio();
                        domicilio.Sugerido = false;
                        domicilio.Direccion = direccion_nombre;
                        domicilio.Distancia = direccion_distancia;
                        domicilio.Observaciones = observacion;
                        domicilio.Cpc = resultadoCpc.Return;
                        domicilio.Barrio = resultadoBarrio.Return;
                        domicilio.Latitud = ("" + lat).Replace(".", ",");
                        domicilio.Longitud = ("" + lng).Replace(".", ",");
                        domicilios.Add(domicilio);
                    }

                    resultado.Return = domicilios;
                    return true;
                }
                catch (Exception e)
                {
                    resultado.AddErrorInterno(e);
                    return false;
                }
            });

            return resultado;
        }

        public Result<List<Resultado_Domicilio>> SugerirResultado(string busqueda)
        {
            var resultado = new Result<List<Resultado_Domicilio>>();

            var resultadoBuscar = Sugerir(busqueda);
            if (!resultadoBuscar.Ok)
            {
                resultado.Copy(resultadoBuscar.Errores);
                return resultado;
            }

            if (resultadoBuscar.Return != null)
            {
                resultado.Return = Resultado_Domicilio.ToList(resultadoBuscar.Return);
            }

            return resultado;
        }


        /* Mapa */

        //public Result<List<Resultado_MarcadorGoogleMaps>> GetMarcadoresGoogleMaps(List<int> ids)
        //{
        //    return dao.GetMarcadoresGoogleMaps(ids);
        //}

        public Result<bool> MigrarDomicilios()
        {
            return dao.MigrarDomicilios();
        }

    }
}

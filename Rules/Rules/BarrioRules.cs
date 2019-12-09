using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using Model.Resultados;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;

namespace Rules.Rules
{
    public class BarrioRules : BaseRules<Barrio>
    {

        private readonly BarrioDAO dao;

        public BarrioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = BarrioDAO.Instance;
        }

        public Result<Barrio> GetByIdCatastro(int idCatastro)
        {
            return dao.GetByIdCatastro(idCatastro);
        }
        public Result<Resultado_Barrio> GetResultadoByIdCatastro(int idCatastro)
        {
            var resultado = new Result<Resultado_Barrio>();

            var resultadoConsulta = GetByIdCatastro(idCatastro);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null)
            {
                resultado.Return = new Resultado_Barrio(resultadoConsulta.Return);
            }

            return resultado;
        }

        public Result<Barrio> Buscar(int id)
        {
            var resultado = new Result<Barrio>();

            try
            {
                string baseUrl = ConfigurationManager.AppSettings["URL_CORDOBA_GEO_API"];
                if (baseUrl == null)
                {
                    resultado.AddErrorInterno("Falta agregar la KEY de CordobaGeoApi");
                    return resultado;
                }
                string url = baseUrl + "/barrio/id/" + id;

                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "text/json");
                IRestResponse response = client.Execute(request).Result;
                JObject obj = JsonConvert.DeserializeObject<JObject>(response.Content);

                if (obj.GetValue("estado").ToObject<string>() == "SIN_RESULTADOS")
                {
                    resultado.Return = null;
                    return resultado;
                }

                if (obj.GetValue("estado").ToObject<string>() != "OK")
                {
                    resultado.AddErrorPublico(obj.GetValue("error").ToObject<string>());
                    return resultado;
                }

                JObject info = obj.GetValue("info").ToObject<JObject>();
                if (info == null)
                {
                    resultado.Return = null;
                    return resultado;
                }

                var barrio_idCatastro = info.GetValue("id").ToObject<int>();
                var barrio_nombre = info.GetValue("nombre").ToObject<string>();

                var resultadoBarrio = new BarrioRules(getUsuarioLogueado()).GetByIdCatastro(barrio_idCatastro);
                if (!resultadoBarrio.Ok)
                {
                    resultado.Copy(resultadoBarrio.Errores);
                    return resultado;
                }

                var barrio = resultadoBarrio.Return;
                if (barrio == null)
                {
                    barrio = new Barrio();
                    barrio.IdCatastro = barrio_idCatastro;
                    barrio.Nombre = barrio_nombre;

                    var resultInsert = new BarrioRules(getUsuarioLogueado()).Insert(barrio);
                    if (!resultInsert.Ok)
                    {
                        resultado.Copy(resultInsert.Errores);
                        return resultado;
                    }
                }
                else
                {
                    if (barrio_nombre != barrio.Nombre)
                    {
                        barrio.Nombre = barrio_nombre;
                        var resultadoUpdate = new BarrioRules(getUsuarioLogueado()).Update(barrio);
                        if (!resultadoUpdate.Ok)
                        {
                            resultado.Copy(resultadoUpdate.Errores);
                            return resultado;
                        }
                    }
                }

                resultado.Return = barrio;

            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }
        public Result<Resultado_Barrio> BuscarResultado(int id)
        {
            var resultado = new Result<Resultado_Barrio>();

            var resultadoConsulta = Buscar(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null)
            {
                resultado.Return = new Resultado_Barrio(resultadoConsulta.Return);
            }

            return resultado;
        }

        public Result<bool> InsertarDesdeCordobaGeoApi()
        {
            var resultado = new Result<bool>();

            try
            {
                string baseUrl = ConfigurationManager.AppSettings["URL_CORDOBA_GEO_API"];
                if (baseUrl == null)
                {
                    resultado.AddErrorInterno("Falta agregar la KEY de CordobaGeoApi");
                    return resultado;
                }
                string url = baseUrl + "/barrios?conPoligono=false";

                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "text/json");
                IRestResponse response = client.Execute(request).Result;
                JObject obj = JsonConvert.DeserializeObject<JObject>(response.Content);
                if (obj.GetValue("estado").ToObject<string>() != "OK")
                {
                    resultado.AddErrorPublico(obj.GetValue("error").ToObject<string>());
                    return resultado;
                }

                JArray barrios = obj.GetValue("info").ToObject<JArray>();
                foreach (JObject barrioData in barrios)
                {
                    var barrio_idCatastro = barrioData.GetValue("id").ToObject<int>();
                    var barrio_nombre = barrioData.GetValue("nombre").ToObject<string>();

                    var resultadoBarrio = new BarrioRules(getUsuarioLogueado()).GetByIdCatastro(barrio_idCatastro);
                    if (!resultadoBarrio.Ok)
                    {
                        resultado.Copy(resultadoBarrio.Errores);
                        return resultado;
                    }

                    var barrio = resultadoBarrio.Return;
                    if (barrio == null)
                    {
                        barrio = new Barrio();
                        barrio.IdCatastro = barrio_idCatastro;
                        barrio.Nombre = barrio_nombre;

                        var resultInsert = new BarrioRules(getUsuarioLogueado()).Insert(barrio);
                        if (!resultInsert.Ok)
                        {
                            resultado.Copy(resultInsert.Errores);
                            return resultado;
                        }
                    }
                    else
                    {
                        if (barrio_nombre != barrio.Nombre)
                        {
                            barrio.Nombre = barrio_nombre;
                            var resultadoUpdate = new BarrioRules(getUsuarioLogueado()).Update(barrio);
                            if (!resultadoUpdate.Ok)
                            {
                                resultado.Copy(resultadoUpdate.Errores);
                                return resultado;
                            }
                        }
                    }
                }

                resultado.Return = true;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
    }
}

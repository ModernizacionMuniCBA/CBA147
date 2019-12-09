using System;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;

namespace Rules.Rules
{
    public class CpcRules : BaseRules<Cpc>
    {
  
        private readonly CpcDAO dao;

        public CpcRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CpcDAO.Instance;
        }

        public Result<Cpc> GetByIdCatastro(int idCatastro) {
            return dao.GetByIdCatastro(idCatastro);
        }
        public Result<Resultado_Cpc> GetResultadoByIdCatastro(int idCatastro)
        {
            var resultado = new Result<Resultado_Cpc>();

            var resultadoConsulta = GetByIdCatastro(idCatastro);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null)
            {
                resultado.Return = new Resultado_Cpc(resultadoConsulta.Return);
            }

            return resultado;
        }


        public Result<Cpc> GetByKeyValue(int keyValue)
        {
            return dao.GetByKeyValue(keyValue);
        }
        public Result<Resultado_Cpc> GetResultadoByKeyValue(int keyValue)
        {
            var resultado = new Result<Resultado_Cpc>();

            var resultadoConsulta = GetByKeyValue(keyValue);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null)
            {
                resultado.Return = new Resultado_Cpc(resultadoConsulta.Return);
            }

            return resultado;
        }

        public Result<Cpc> Buscar(int id)
        {
            var resultado = new Result<Cpc>();

            try
            {
                string baseUrl = ConfigurationManager.AppSettings["URL_CORDOBA_GEO_API"];
                if (baseUrl == null)
                {
                    resultado.AddErrorInterno("Falta agregar la KEY de CordobaGeoApi");
                    return resultado;
                }
                string url = baseUrl + "/cpc/id/" + id;

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

                var idCatastro = info.GetValue("id").ToObject<int>();
                var nombre = info.GetValue("nombre").ToObject<string>();
                var numero = info.GetValue("numero").ToObject<int>();

                var resultadoConsulta = GetByIdCatastro(idCatastro);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Copy(resultadoConsulta.Errores);
                    return resultado;
                }

                var cpc = resultadoConsulta.Return;
                if (cpc == null)
                {
                    cpc = new Cpc();
                    cpc.IdCatastro = idCatastro;
                    cpc.Nombre = nombre;

                    var resultInsert = new CpcRules(getUsuarioLogueado()).Insert(cpc);
                    if (!resultInsert.Ok)
                    {
                        resultado.Copy(resultInsert.Errores);
                        return resultado;
                    }
                }
                else
                {
                    bool cambio = false;
                    if (nombre != cpc.Nombre)
                    {
                        cambio = true;
                        cpc.Nombre = nombre;
                    }

                    if (numero != cpc.Numero)
                    {
                        cambio = true;
                        cpc.Numero = numero;
                    }

                    if (cambio)
                    {
                        var resultadoUpdate = new CpcRules(getUsuarioLogueado()).Update(cpc);
                        if (!resultadoUpdate.Ok)
                        {
                            resultado.Copy(resultadoUpdate.Errores);
                            return resultado;
                        }
                    }
                }

                resultado.Return = cpc;

            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }
        public Result<Resultado_Cpc> BuscarResultado(int id)
        {
            var resultado = new Result<Resultado_Cpc>();

            var resultadoConsulta = Buscar(id);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return != null)
            {
                resultado.Return = new Resultado_Cpc(resultadoConsulta.Return);
            }

            return resultado;
        }
    }
}

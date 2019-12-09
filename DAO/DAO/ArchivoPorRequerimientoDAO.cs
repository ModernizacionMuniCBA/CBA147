using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace DAO.DAO
{
    public class ArchivoPorRequerimientoDAO : BaseDAO<ArchivoPorRequerimiento>
    {
        private static ArchivoPorRequerimientoDAO instance;

        public static ArchivoPorRequerimientoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ArchivoPorRequerimientoDAO();
                }
                return instance;
            }
        }


        private IQueryOver<ArchivoPorRequerimiento, ArchivoPorRequerimiento> GetQuery(int idRequerimiento, Enums.TipoArchivo? tipo, bool? dadosDeBaja)
        {
            var query = GetSession().QueryOver<ArchivoPorRequerimiento>();
            query.Where(x => x.Requerimiento.Id == idRequerimiento);

            //Tipo
            if (tipo.HasValue)
            {
                query.Where(x => x.Tipo == tipo);
            }

            //Dado de baja
            if (dadosDeBaja.HasValue)
            {
                if (dadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }
            }
            return query;
        }

        private IQueryOver<ArchivoPorRequerimiento, ArchivoPorRequerimiento> GetQuery(Model.Consultas.Consulta_ArchivoPorRequerimiento consulta)
        {
            var query = GetSession().QueryOver<ArchivoPorRequerimiento>();

            //RQ
            if (consulta.IdRequerimiento.HasValue)
            {
                query.Where(x => x.Requerimiento.Id == consulta.IdRequerimiento.Value);
            }

            //Tipo
            if (consulta.Tipo.HasValue)
            {
                query.Where(x => x.Tipo == consulta.Tipo.Value);
            }

            //Dado de baja
            if (consulta.DadosDeBaja.HasValue)
            {
                if (consulta.DadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }
            }
            return query;
        }

        //public Result<List<int>> GetIdsByFilters(Model.Consultas.Consulta_ArchivoPorRequerimiento consulta)
        //{
        //    var resultado = new Result<List<int>>();
        //    try
        //    {
        //        var query = GetQuery(consulta);
        //        resultado.Return = query.Select(x => x.Id).List<int>().ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        resultado.AddErrorInterno(e);
        //    }

        //    return resultado;
        //}

        public Result<List<ArchivoPorRequerimiento>> GetByFilters(Model.Consultas.Consulta_ArchivoPorRequerimiento consulta)
        {
            var resultado = new Result<List<ArchivoPorRequerimiento>>();
            try
            {
                var query = GetQuery(consulta);
                resultado.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<bool> Migrar()
        {
            var resultado = new Result<bool>();

            //using (var session = SessionManager.Instance.SessionFactory.OpenSession())
            //{
            //    int paso = 1;

            //    int cantidadRestante = session.QueryOver<ArchivoPorRequerimiento>().Where(x => x.FechaBaja == null && x.Data != null && (x.Identificador == null ) && x.Tipo == Enums.TipoArchivo.IMAGEN).RowCount();

            //    while (cantidadRestante != 0)
            //    {
            //        var entities = session.QueryOver<ArchivoPorRequerimiento>().Where(x => x.FechaBaja == null && x.Data != null && (x.Identificador == null) && x.Tipo == Enums.TipoArchivo.IMAGEN).Take(paso).List().ToList();

            //        for (int i = 0; i < entities.Count(); i++)
            //        {
            //            var e = entities[i];

            //            var comando = new
            //            {
            //                FileName = e.Nombre,
            //                Content = e.Data
            //            };


            //            var urlServidor = ConfigurationManager.AppSettings["URL_SERVIDOR_CORDOBA_FILES"];
            //            var apiKey = ConfigurationManager.AppSettings["API_KEY_CORDOBA_FILES"];
            //            var url = urlServidor + "/Archivo/v1/InsertarBase64?apiKey=" + apiKey;

            //            var client = new RestClient(url);
            //            var request = new RestRequest(Method.POST);
            //            request.AddBody(comando);
            //            request.AddHeader("Cache-Control", "no-cache");
            //            request.AddHeader("Content-Type", "application/json");
            //            IRestResponse response = client.Execute(request).Result;

            //            var respuesta = JObject.Parse(response.Content);
            //            if (respuesta.GetValue("status").ToObject<string>() == "ok")
            //            {
            //                var data = respuesta.GetValue("data").ToObject<JObject>();
            //                e.Identificador = data.GetValue("identificador").ToObject<string>();
            //                e.Width = data.GetValue("width").ToObject<int>();
            //                e.Height = data.GetValue("height").ToObject<int>();
            //                e.ContentType = data.GetValue("contentType").ToObject<string>();
            //                e.ContentLenght = data.GetValue("contentLength").ToObject<int>();
            //            }
            //            else
            //            {
            //                e.Identificador = "Error";
            //            }

            //            session.Update(e);
            //            session.Flush();
            //        }

            //        cantidadRestante = session.QueryOver<ArchivoPorRequerimiento>().Where(x => x.FechaBaja == null && x.Data != null && (x.Identificador == null) && x.Tipo == Enums.TipoArchivo.IMAGEN).RowCount();
            //    }

            //    resultado.Return = true;

            //}
            return resultado;
        }

    }
}

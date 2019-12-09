using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using Model.Consultas;
using NHibernate;
using Model.Resultados;
using Model.Comandos;


namespace DAO.DAO
{
    public class OrigenPorAmbitoDAO : BaseDAO<OrigenPorAmbito>
    {
        private static OrigenPorAmbitoDAO instance;

        public static OrigenPorAmbitoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrigenPorAmbitoDAO();
                }
                return instance;
            }
        }

        private IQueryOver<OrigenPorAmbito, OrigenPorAmbito> GetQuery(Consulta_OrigenPorAmbito consulta)
        {
            var query = GetSession().QueryOver<OrigenPorAmbito>();

            //Ambito Id
            if (consulta.AmbitoId.HasValue && consulta.AmbitoId.Value != -1)
            {
                query.JoinQueryOver<CerrojoAmbito>(x => x.Ambito).Where(x => x.Id == consulta.AmbitoId.Value);
            }

            //Origen Id
            if (consulta.OrigenId.HasValue && consulta.OrigenId.Value!=-1)
            {
                query.JoinQueryOver<Origen>(x => x.Origen).Where(x => x.Id == consulta.OrigenId.Value);
            }

            //Dados de baja
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

        public Result<List<Resultado_OrigenPorAmbito>> GetByFilters(Consulta_OrigenPorAmbito consulta)
        {
            var result = new Result<List<Resultado_OrigenPorAmbito>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = Resultado_OrigenPorAmbito.ToList(query.List().ToList());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }

            return result;
        } 
    }
}

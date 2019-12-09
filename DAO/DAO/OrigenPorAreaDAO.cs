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
    public class OrigenPorAreaDAO : BaseDAO<OrigenPorArea>
    {
        private static OrigenPorAreaDAO instance;

        public static OrigenPorAreaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrigenPorAreaDAO();
                }
                return instance;
            }
        }

        private IQueryOver<OrigenPorArea, OrigenPorArea> GetQuery(Consulta_OrigenPorArea consulta)
        {
            var query = GetSession().QueryOver<OrigenPorArea>();

            //Area Id
            if (consulta.AreaId.HasValue && consulta.AreaId.Value!=-1)
            {
                query.JoinQueryOver<CerrojoArea>(x=>x.Area).Where(x=>x.Id == consulta.AreaId.Value);
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

        public Result<List<Resultado_OrigenPorArea>> GetByFilters(Consulta_OrigenPorArea consulta)
        {
            var result = new Result<List<Resultado_OrigenPorArea>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = Resultado_OrigenPorArea.ToList(query.List().ToList());
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

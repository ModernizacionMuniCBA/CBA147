using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using NHibernate;
using Model.Consultas;
using Model.Resultados;

namespace DAO.DAO
{
    public class RequerimientoFavoritoPorUsuarioDAO : BaseDAO<RequerimientoFavoritoPorUsuario>
    {
        private static RequerimientoFavoritoPorUsuarioDAO instance;

        public static RequerimientoFavoritoPorUsuarioDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequerimientoFavoritoPorUsuarioDAO();
                }
                return instance;
            }
        }

        private IQueryOver<RequerimientoFavoritoPorUsuario, RequerimientoFavoritoPorUsuario> GetQuery(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            var query = GetSession().QueryOver<RequerimientoFavoritoPorUsuario>();

            if (consulta.IdRequerimiento.HasValue)
            {
                query.JoinQueryOver<Requerimiento>(x => x.Requerimiento).Where(x => x.Id == consulta.IdRequerimiento.Value && x.FechaBaja == null);
            }

            if (consulta.IdUser.HasValue)
            {
                query.JoinQueryOver<_VecinoVirtualUsuario>(x => x.User).Where(x => x.Id == consulta.IdUser.Value && x.FechaBaja == null);
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

        public Result<List<RequerimientoFavoritoPorUsuario>> GetByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            var resultado = new Result<List<RequerimientoFavoritoPorUsuario>>();
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

        public Result<List<Resultado_RequerimientoFavoritoPorUsuario>> GetResultadoByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            var resultado = new Result<List<Resultado_RequerimientoFavoritoPorUsuario>>();
            try
            {
                var query = GetQuery(consulta);
                resultado.Return = Resultado_RequerimientoFavoritoPorUsuario.ToList(query.List().ToList());
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<int>> GetIdsRequerimientoByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            var resultado = new Result<List<int>>();
            try
            {
                var query = GetQuery(consulta);
                resultado.Return = query.Select(x => x.Requerimiento.Id).List<int>().ToList();
                resultado.Return = resultado.Return.Distinct().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Requerimiento>> GetRequerimientoByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            var resultado = new Result<List<Requerimiento>>();
            try
            {
                var query = GetQuery(consulta);
                resultado.Return = query.Select(x => x.Requerimiento).List<Requerimiento>().ToList();
                resultado.Return = resultado.Return.GroupBy(x=>x.Id).Select(x=>x.FirstOrDefault()).ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<int> GetCantidadRequerimientosByFilters(Consulta_RequerimientoFavoritoPorUsuario consulta)
        {
            var resultado = new Result<int>();
            try
            {
                var query = GetQuery(consulta);
                resultado.Return = query.RowCount();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
    }
}

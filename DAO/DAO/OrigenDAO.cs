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
    public class OrigenDAO : BaseDAO<Origen>
    {
        private static OrigenDAO instance;

        public static OrigenDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrigenDAO();
                }
                return instance;
            }
        }

        private IQueryOver<Origen, Origen> GetQuery(Consulta_Origen consulta)
        {
            var query = GetSession().QueryOver<Origen>();

            //Nombre
            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                query.Where(x => x.Nombre.Contains(consulta.Nombre));
            }

            //Key
            if (!string.IsNullOrEmpty(consulta.KeyAlias))
            {
                query.Where(x => x.KeyAlias == consulta.KeyAlias);
            }

            //KeySecret
            if (!string.IsNullOrEmpty(consulta.KeySecret))
            {
                query.Where(x => x.KeySecret == consulta.KeySecret);
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

        public Result<List<Origen>> GetByFilters(Consulta_Origen consulta)
        {
            var result = new Result<List<Origen>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = query.List().ToList();
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

        public Result<List<Resultado_Origen>> GetResultadoByFilters(Consulta_Origen consulta)
        {
            var result = new Result<List<Resultado_Origen>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = Resultado_Origen.ToList(query.List().ToList());
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

        public Result<int> GetCantidadDuplicados(int? id, string nombre, string keyAlias, string keySecret)
        {
            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<Origen>();

                if (id.HasValue)
                {
                    query.Where(x => x.Id != id);
                }
                query.Where(x => (x.Nombre == nombre || x.KeyAlias == keyAlias || x.KeySecret == keySecret) && x.FechaBaja == null);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<bool> ExisteKeyAlias(string keyAlias)
        {
            var resultado = new Result<bool>();

            try
            {

                var query = GetSession().QueryOver<Origen>().Where(x => x.KeyAlias == keyAlias);
                resultado.Return = query.RowCount() != 0;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }

        public Result<bool> ExisteKeySecret(string keySecret)
        {
            var resultado = new Result<bool>();

            try
            {
                var query = GetSession().QueryOver<Origen>().Where(x => x.KeySecret == keySecret);
                resultado.Return = query.RowCount() != 0;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class TipoDAO : BaseDAO<TipoRequerimiento>
    {
        private static TipoDAO instance;

        public static TipoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TipoDAO();
                }
                return instance;
            }
        }

        public Result<List<TipoRequerimiento>> GetByFilters(string nombre, Enums.TipoRequerimiento tipo, bool? dadosDeBaja)
        {
            var result = new Result<List<TipoRequerimiento>>();

            try
            {
                var query = GetSession().QueryOver<TipoRequerimiento>();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query.Where(x => x.Nombre.IsLike(nombre, MatchMode.Anywhere));
                }

                if (tipo != null)
                {
                    query.Where(x => x.KeyValue == tipo);
                }

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
                result.Return = new List<TipoRequerimiento>(query.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
            }
            return result;
        }
    }
}

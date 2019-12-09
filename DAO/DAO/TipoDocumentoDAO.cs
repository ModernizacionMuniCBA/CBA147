using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class TipoDocumentoDAO : BaseDAO<TipoDocumento>
    {
        private static TipoDocumentoDAO instance;

        public static TipoDocumentoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TipoDocumentoDAO();
                }
                return instance;
            }
        }

        public Result<List<TipoDocumento>> GetByFilters(string nombre, bool? dadosDeBaja)
        {
            var result = new Result<List<TipoDocumento>>();

            try
            {
                var query = GetSession().QueryOver<TipoDocumento>();

                if (!string.IsNullOrEmpty(nombre))
                {
                    query.Where(x => x.Nombre.IsLike(nombre, MatchMode.Anywhere));
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
                result.Return = new List<TipoDocumento>(query.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class TipoMovilDAO : BaseDAO<TipoMovil>
    {
        private static TipoMovilDAO instance;

        public static TipoMovilDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TipoMovilDAO();
                }
                return instance;
            }
        }

        public Result<bool> Equals(TipoMovil obj)
        {
            try
            {
                var result = new Result<bool>();
                var query = GetSession().QueryOver<TipoMovil>();
                query.Where(x => x.Nombre.IsLike(obj.Nombre.ToUpper().Trim()));
                query.Where(x => x.FechaBaja==null);
                result.Return = query.List().Count != 0;
                return result;
            }
            catch (Exception e)
            {
                var result = new Result<bool>();
                result.AddErrorInterno(e.InnerException);
                return result;
            }
        }
    }
}

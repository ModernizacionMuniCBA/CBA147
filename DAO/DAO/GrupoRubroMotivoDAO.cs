using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using NHibernate.Criterion;
using Model.Entities;

namespace DAO.DAO
{
    public class GrupoRubroMotivoDAO : BaseDAO<GrupoRubroMotivo>
    {
        private static GrupoRubroMotivoDAO instance;

        public static GrupoRubroMotivoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GrupoRubroMotivoDAO();
                }
                return instance;
            }
        }


        public Result<bool> Equals(GrupoRubroMotivo obj)
        {
            try
            {
                var result = new Result<bool>();
                var query = GetSession().QueryOver<GrupoRubroMotivo>();
                query.Where(x => x.Nombre.IsLike(obj.Nombre.ToUpper().Trim()));
                query.Where(x => x.FechaBaja == null);
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

using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class CpcDAO : BaseDAO<Cpc>
    {
        private static CpcDAO instance;

        public static CpcDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CpcDAO();
                }
                return instance;
            }
        }

        public Result<Cpc> GetByIdCatastro(int idCatastro)
        {
            var result = new Result<Cpc>();
            try
            {
                var query = GetSession().QueryOver<Cpc>();
                query.Where(x => x.IdCatastro == idCatastro);

                var cpcs = query.List();
                if (cpcs.Count == 0)
                {
                    result.Return = null;
                }
                else
                {
                    result.Return = cpcs[0];
                }
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<Cpc> GetByKeyValue(int keyValue)
        {
            var result = new Result<Cpc>();
            try
            {
                var query = GetSession().QueryOver<Cpc>();
                query.Where(x => x.Numero == keyValue);

                var cpcs = query.List();
                if (cpcs.Count == 0)
                {
                    result.Return = null;
                }
                else
                {
                    result.Return = cpcs[0];
                }
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

    }
}

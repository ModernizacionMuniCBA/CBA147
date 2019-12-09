using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using NHibernate;

namespace DAO.DAO
{
    public class CategoriaMotivoAreaDAO : BaseDAO<CategoriaMotivoArea>
    {
        private static CategoriaMotivoAreaDAO instance;

        public static CategoriaMotivoAreaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoriaMotivoAreaDAO();
                }
                return instance;
            }
        }

        public Result<bool> Equals(CategoriaMotivoArea obj)
        {
            try
            {
                var result = new Result<bool>();
                var query = GetSession().QueryOver<CategoriaMotivoArea>();
                query.Where(x => x.Nombre.IsLike(obj.Nombre.ToUpper().Trim()));
                query.Where(x => x.Area.Id==obj.Area.Id);
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

        public Result<List<CategoriaMotivoArea>> GetByIdArea(int idArea)
        {
            var result = new Result<List<CategoriaMotivoArea>>();

            try
            {
                var query = GetSession().QueryOver<CategoriaMotivoArea>();
                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id == idArea);
                query.Where(x=>x.FechaBaja==null);
                result.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
       
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using Model.Resultados;

namespace DAO.DAO
{
    public class CampoPorMotivoDAO : BaseDAO<CampoPorMotivo>
    {
        private static CampoPorMotivoDAO instance;

        public static CampoPorMotivoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CampoPorMotivoDAO();
                }
                return instance;
            }
        }


        public Result<bool> Equals(CampoPorMotivo obj)
        {
            var result = new Result<bool>();
            
            try
            {
                var query = GetSession().QueryOver<CampoPorMotivo>();
                query.Where(x => x.Nombre.IsLike(obj.Nombre.ToUpper().Trim()));
                query.Where(x => x.Motivo == obj.Motivo);
                query.Where(x => x.FechaBaja == null);
                result.Return = query.List().Count != 0;
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.InnerException);
                return result;
            }
        }


        public Result<IList<CampoPorMotivo>> GetByIdMotivo(int idMotivo)
        {
            var result = new Result<IList<CampoPorMotivo>>();
            try
            {        
                var query = GetSession().QueryOver<CampoPorMotivo>();
                query.Where(x => x.Motivo.Id==idMotivo);
                query.Where(x => x.FechaBaja == null);
                result.Return = query.List();
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.InnerException);
                return result;
            }
        }
    }
}

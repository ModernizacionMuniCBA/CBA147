using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class NotaPorMovilDAO : BaseDAO<NotaPorMovil>
    {
        private static NotaPorMovilDAO instance;

        public static NotaPorMovilDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotaPorMovilDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<List<NotaPorMovil>> GetByFilters(int? idMovil, bool? dadoDeBaja)
        {
            var result = new Result<List<NotaPorMovil>>();

            try
            {
                var query = GetSession().QueryOver<NotaPorMovil>();

                if (idMovil.HasValue)
                {
                    query.JoinQueryOver<Movil>(x => x.Movil).Where(x => x.Id == idMovil.Value);                
                }

                if (dadoDeBaja.HasValue)
                {
                    if (dadoDeBaja.Value)
                    {
                        query.Where(x => x.FechaBaja != null);
                    }
                    else {
                        query.Where(x => x.FechaBaja == null);                    
                    }
                }
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

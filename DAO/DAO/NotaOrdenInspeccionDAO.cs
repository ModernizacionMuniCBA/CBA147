using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class NotaOrdenInspeccionDAO : BaseDAO<NotaPorOrdenInspeccion>
    {
        private static NotaOrdenInspeccionDAO instance;

        public static NotaOrdenInspeccionDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotaOrdenInspeccionDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<List<NotaPorOrdenInspeccion>> GetByFilters(int? idOrdenInspeccion, bool? dadoDeBaja)
        {
            var result = new Result<List<NotaPorOrdenInspeccion>>();

            try
            {
                var query = GetSession().QueryOver<NotaPorOrdenInspeccion>();

                if (idOrdenInspeccion.HasValue)
                {
                    query.JoinQueryOver<OrdenInspeccion>(x => x.OrdenInspeccion).Where(x => x.Id == idOrdenInspeccion.Value);                
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

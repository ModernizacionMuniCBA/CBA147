using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class NotaOrdenTrabajoDAO : BaseDAO<NotaPorOrdenTrabajo>
    {
        private static NotaOrdenTrabajoDAO instance;

        public static NotaOrdenTrabajoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotaOrdenTrabajoDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<List<NotaPorOrdenTrabajo>> GetByFilters(int? idOrdenTrabajo, bool? dadoDeBaja)
        {
            var result = new Result<List<NotaPorOrdenTrabajo>>();

            try
            {
                var query = GetSession().QueryOver<NotaPorOrdenTrabajo>();

                if (idOrdenTrabajo.HasValue) {
                    query.JoinQueryOver<OrdenTrabajo>(x => x.OrdenTrabajo).Where(x => x.Id == idOrdenTrabajo.Value);                
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

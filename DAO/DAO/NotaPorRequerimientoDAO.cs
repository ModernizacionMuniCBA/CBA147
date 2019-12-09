using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class NotaPorRequerimientoDAO : BaseDAO<NotaPorRequerimiento>
    {
        private static NotaPorRequerimientoDAO instance;

        public static NotaPorRequerimientoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotaPorRequerimientoDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<List<NotaPorRequerimiento>> GetByFilters(int? idOrdenTrabajo, int? idRequerimiento, bool? dadoDeBaja)
        {
            var result = new Result<List<NotaPorRequerimiento>>();

            try
            {
                var query = GetSession().QueryOver<NotaPorRequerimiento>();

                //Orden de trabajo
                if (idOrdenTrabajo.HasValue) {
                    query.JoinQueryOver<OrdenTrabajo>(x => x.OrdenTrabajo).Where(x => x.Id == idOrdenTrabajo.Value);                
                }

                //Requerimiento
                if (idRequerimiento.HasValue)
                {
                    query.JoinQueryOver<Requerimiento>(x => x.Requerimiento).Where(x => x.Id == idRequerimiento.Value);
                }

                //Dado de baja
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

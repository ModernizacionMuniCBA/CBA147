using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class RequerimientoPorOrdenTrabajoDAO : BaseDAO<RequerimientoPorOrdenTrabajo>
    {
        private static RequerimientoPorOrdenTrabajoDAO instance;

        public static RequerimientoPorOrdenTrabajoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequerimientoPorOrdenTrabajoDAO();
                }
                return instance;
            }
        }

        public Result<int> GetCantidadRequerimientosByIdOT(int idOt)
        {
            var query = GetSession().QueryOver<RequerimientoPorOrdenTrabajo>();
            query.Where(z => z.OrdenTrabajo.Id == idOt && z.FechaBaja == null);
            var result = new Result<int>();
            result.Return = query.RowCount();
            return result;
        }

    }
}
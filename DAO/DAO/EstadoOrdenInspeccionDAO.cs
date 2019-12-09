using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class EstadoOrdenInspeccionDAO : BaseDAO<EstadoOrdenInspeccion>
    {
        private static EstadoOrdenInspeccionDAO instance;

        public static EstadoOrdenInspeccionDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new EstadoOrdenInspeccionDAO();
                return instance;
            }
        }

        public Result<EstadoOrdenInspeccion> GetByKeyValue(Enums.EstadoOrdenInspeccion keyValue)
        {
            var result = new Result<EstadoOrdenInspeccion>();
            try
            {
                var query = GetSession().QueryOver<EstadoOrdenInspeccion>();
                query.Where(x => (int)x.KeyValue == (int)keyValue);

                result.Return = query.List().FirstOrDefault();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
    }
}

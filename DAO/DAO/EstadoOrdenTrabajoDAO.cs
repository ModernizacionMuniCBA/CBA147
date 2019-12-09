using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class EstadoOrdenTrabajoDAO : BaseDAO<EstadoOrdenTrabajo>
    {
        private static EstadoOrdenTrabajoDAO instance;

        public static EstadoOrdenTrabajoDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new EstadoOrdenTrabajoDAO();
                return instance;
            }
        }

        public Result<EstadoOrdenTrabajo> GetByKeyValue(Enums.EstadoOrdenTrabajo keyValue)
        {
            var result = new Result<EstadoOrdenTrabajo>();
            try
            {
                var query = GetSession().QueryOver<EstadoOrdenTrabajo>();
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

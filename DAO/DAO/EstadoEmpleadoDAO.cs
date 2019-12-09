using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class EstadoEmpleadoDAO : BaseDAO<EstadoEmpleado>
    {
        private static EstadoEmpleadoDAO instance;

        public static EstadoEmpleadoDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new EstadoEmpleadoDAO();
                return instance;
            }
        }

        public Result<EstadoEmpleado> GetByKeyValue(Enums.EstadoEmpleado keyValue)
        {
            var result = new Result<EstadoEmpleado>();
            try
            {
                var query = GetSession().QueryOver<EstadoEmpleado>();
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

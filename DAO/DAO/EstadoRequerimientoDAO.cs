using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class EstadoRequerimientoDAO : BaseDAO<EstadoRequerimiento>
    {
        private static EstadoRequerimientoDAO instance;

        public static EstadoRequerimientoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EstadoRequerimientoDAO();
                }
                return instance;
            }
        }

        public Result<EstadoRequerimiento> GetByKeyValue(Enums.EstadoRequerimiento keyValue)
        {
            var result = new Result<EstadoRequerimiento>();
            try
            {
                var query = GetSession().QueryOver<EstadoRequerimiento>();
                query.Where(x => x.KeyValue == keyValue);

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

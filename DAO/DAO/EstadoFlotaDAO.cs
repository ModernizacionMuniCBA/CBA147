using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class EstadoFlotaDAO : BaseDAO<EstadoFlota>
    {
        private static EstadoFlotaDAO instance;

        public static EstadoFlotaDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new EstadoFlotaDAO();
                return instance;
            }
        }

        public Result<EstadoFlota> GetByKeyValue(Enums.EstadoFlota keyValue)
        {
            var result = new Result<EstadoFlota>();
            try
            {
                var query = GetSession().QueryOver<EstadoFlota>();
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

using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class EstadoMovilDAO : BaseDAO<EstadoMovil>
    {
        private static EstadoMovilDAO instance;

        public static EstadoMovilDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new EstadoMovilDAO();
                return instance;
            }
        }

        public Result<EstadoMovil> GetByKeyValue(Enums.EstadoMovil keyValue)
        {
            var result = new Result<EstadoMovil>();
            try
            {
                var query = GetSession().QueryOver<EstadoMovil>();
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

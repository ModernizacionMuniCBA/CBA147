using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class EstadoOrdenEspecialDAO : BaseDAO<EstadoOrdenEspecial>
    {
        private static EstadoOrdenEspecialDAO instance;

        public static EstadoOrdenEspecialDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new EstadoOrdenEspecialDAO();
                return instance;
            }
        }

        public Result<EstadoOrdenEspecial> GetByKeyValue(Enums.EstadoOrdenEspecial keyValue)
        {
            var result = new Result<EstadoOrdenEspecial>();
            try
            {
                var query = GetSession().QueryOver<EstadoOrdenEspecial>();
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

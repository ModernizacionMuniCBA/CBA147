using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class AjustesDAO : BaseDAO<Ajustes>
    {
        private static AjustesDAO instance;

        public static AjustesDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AjustesDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<Ajustes> Get()
        {
            var result = new Result<Ajustes>();

            try
            {
                var query = GetSession().QueryOver<Ajustes>();
                result.Return = query.Where(x => x.FechaBaja == null).OrderBy(x => x.FechaAlta).Desc.List().First();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
    }
}

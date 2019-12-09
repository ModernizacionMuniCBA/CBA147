using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class VersionSistemaDAO : BaseDAO<VersionSistema>
    {
        private static VersionSistemaDAO instance;

        public static VersionSistemaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VersionSistemaDAO();
                }
                return instance;
            }
        }

        public Result<VersionSistema> Get()
        {
            var result = new Result<VersionSistema>();

            try
            {
                result.Return = GetSession().QueryOver<VersionSistema>().Where(x=>x.FechaBaja==null).OrderBy(x=>x.FechaAlta).Desc.List().First();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }

            return result;
        }
    }
}

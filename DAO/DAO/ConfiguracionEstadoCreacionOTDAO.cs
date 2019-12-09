using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class ConfiguracionEstadoCreacionOTDAO : BaseDAO<ConfiguracionEstadoCreacionOT>
    {
        private static ConfiguracionEstadoCreacionOTDAO instance;

        public static ConfiguracionEstadoCreacionOTDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfiguracionEstadoCreacionOTDAO();
                }
                return instance;
            }
        }

        public Result<ConfiguracionEstadoCreacionOT> GetByIdArea(int idArea)
        {
            var result = new Result<ConfiguracionEstadoCreacionOT>();

            try
            {
                var query = GetSession().QueryOver<ConfiguracionEstadoCreacionOT>();
                result.Return = query.Where(x => x.FechaBaja == null && x.Area.Id == idArea).List().FirstOrDefault();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

    }
}

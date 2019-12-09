using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class ConfiguracionBandejaPorAreaDAO : BaseDAO<ConfiguracionBandejaPorArea>
    {
        private static ConfiguracionBandejaPorAreaDAO instance;

        public static ConfiguracionBandejaPorAreaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfiguracionBandejaPorAreaDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<ConfiguracionBandejaPorArea> GetByIdArea(int idArea)
        {
            var result = new Result<ConfiguracionBandejaPorArea>();

            try
            {
                var query = GetSession().QueryOver<ConfiguracionBandejaPorArea>();
                result.Return = query.Where(x => x.FechaBaja == null && x.Area.Id == idArea && x.PorDefecto==true).List().FirstOrDefault();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

    }
}

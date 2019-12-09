using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class LimiteRequerimientosPorUsuarioDAO : BaseDAO<LimiteRequerimientosPorUsuario>
    {
        private static LimiteRequerimientosPorUsuarioDAO instance;

        public static LimiteRequerimientosPorUsuarioDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LimiteRequerimientosPorUsuarioDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<LimiteRequerimientosPorUsuario> ValidarLimiteRequerimientos(int idUsuario)
        {
            var result = new Result<LimiteRequerimientosPorUsuario>();

            try
            {
                var query = GetSession().QueryOver<LimiteRequerimientosPorUsuario>();
                    query.Where(x => x.IdUsuarioCreador == idUsuario);
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

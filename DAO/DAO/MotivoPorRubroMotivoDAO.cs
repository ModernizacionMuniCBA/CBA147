using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class MotivoPorRubroMotivoDAO : BaseDAO<MotivoPorRubroMotivo>
    {
        private static MotivoPorRubroMotivoDAO instance;

        public static MotivoPorRubroMotivoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MotivoPorRubroMotivoDAO();
                }
                return instance;
            }
        }


        public Result<List<MotivoPorRubroMotivo>> GetByIdGrupo(int idGrupo)
        {
            var result = new Result<List<MotivoPorRubroMotivo>>();
            
            try
            {
                var query = GetSession().QueryOver<MotivoPorRubroMotivo>();
                query.JoinQueryOver<RubroMotivo>(x => x.CategoriaMotivo).Where(x=>x.Grupo.Id==idGrupo && x.FechaBaja==null);
                query.Where(x => x.FechaBaja == null);
                result.Return = query.List().ToList();
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.InnerException);
                return result;
            }
        
        }
    }
}

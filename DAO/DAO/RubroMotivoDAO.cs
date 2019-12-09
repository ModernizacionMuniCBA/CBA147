using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class RubroMotivoDAO : BaseDAO<RubroMotivo>
    {
        private static RubroMotivoDAO instance;

        public static RubroMotivoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RubroMotivoDAO();
                }
                return instance;
            }
        }

        public Result<List<RubroMotivo>> GetRubrosByIdGrupo(int idGrupo)
        {
            var result = new Result<List<RubroMotivo>>();

            try
            {
                var query = GetSession().QueryOver<RubroMotivo>();
                query.JoinQueryOver<GrupoRubroMotivo>(x => x.Grupo).Where(x => x.Id == idGrupo && x.FechaBaja == null);
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

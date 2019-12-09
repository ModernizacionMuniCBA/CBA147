using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class MotivoPorRubroMotivoRules  : BaseRules<MotivoPorRubroMotivo>
    {

        private readonly MotivoPorRubroMotivoDAO dao;

        public MotivoPorRubroMotivoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = MotivoPorRubroMotivoDAO .Instance;
        }


        public Result<List<MotivoPorRubroMotivo>> GetByIdGrupo(int idGrupo)
        {
            return dao.GetByIdGrupo(idGrupo);
        }
    }
}

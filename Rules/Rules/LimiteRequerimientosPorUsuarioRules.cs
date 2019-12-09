using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;

namespace Rules.Rules
{
    public class LimiteRequerimientosPorUsuarioRules : BaseRules<LimiteRequerimientosPorUsuario>
    {
    
        private readonly LimiteRequerimientosPorUsuarioDAO dao;

        public LimiteRequerimientosPorUsuarioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = LimiteRequerimientosPorUsuarioDAO.Instance;
        }

        public Result<LimiteRequerimientosPorUsuario> ValidarLimiteRequerimientos(){
            return dao.ValidarLimiteRequerimientos(getUsuarioLogueado().Usuario.Id);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class AjustesRules : BaseRules<Ajustes>
    {

        private readonly AjustesDAO dao;

        public AjustesRules(UsuarioLogueado data)
            : base(data)
        {
            dao = AjustesDAO.Instance;
        }

        public Result<Ajustes> Get()
        {
            return dao.Get();
        }
    }
}

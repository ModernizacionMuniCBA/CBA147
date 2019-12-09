using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class TipoCampoPorMotivoRules : BaseRules<TipoCampo>
    {

        private readonly TipoCampoPorMotivoDAO dao;

        public TipoCampoPorMotivoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = TipoCampoPorMotivoDAO.Instance;
        }
    }
}

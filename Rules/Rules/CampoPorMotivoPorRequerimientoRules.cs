using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class CampoPorMotivoPorRequerimientoRules : BaseRules<CampoPorMotivoPorRequerimiento>
    {

        private readonly CampoPorMotivoPorRequerimientoDAO dao;

        public CampoPorMotivoPorRequerimientoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = CampoPorMotivoPorRequerimientoDAO.Instance;
        }

    }
}

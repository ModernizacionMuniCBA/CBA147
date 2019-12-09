using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Consultas;
using Model.Comandos;

namespace Rules.Rules
{
    public class ReferenteProvisorioRules : BaseRules<ReferenteProvisorio>
    {
        private readonly ReferenteProvisorioDAO dao;

        public ReferenteProvisorioRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ReferenteProvisorioDAO.Instance;
        }

    }
}

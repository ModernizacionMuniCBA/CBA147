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
    public class KilometrajePorMovilRules : BaseRules<KilometrajePorMovil>
    {
        private readonly KilometrajePorMovilDAO dao;

        public KilometrajePorMovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = KilometrajePorMovilDAO.Instance;
        }


    }
}

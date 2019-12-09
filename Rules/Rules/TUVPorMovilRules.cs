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
    public class TUVPorMovilRules : BaseRules<TUVPorMovil>
    {
        private readonly TUVPorMovilDAO dao;

        public TUVPorMovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = TUVPorMovilDAO .Instance;
        }


    }
}

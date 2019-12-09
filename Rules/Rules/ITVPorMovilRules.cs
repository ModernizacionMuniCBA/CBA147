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
    public class ITVPorMovilRules : BaseRules<ITVPorMovil>
    {
        private readonly ITVPorMovilDAO dao;

        public ITVPorMovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ITVPorMovilDAO .Instance;
        }


    }
}

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
    public class ValuacionPorMovilRules : BaseRules<ValuacionPorMovil>
    {
        private readonly ValuacionPorMovilDAO dao;

        public ValuacionPorMovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ValuacionPorMovilDAO.Instance;
        }


    }
}

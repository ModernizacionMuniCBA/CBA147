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
    public class ReparacionPorMovilRules : BaseRules<ReparacionPorMovil>
    {
        private readonly ReparacionPorMovilDAO dao;

        public ReparacionPorMovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ReparacionPorMovilDAO.Instance;
        }


    }
}

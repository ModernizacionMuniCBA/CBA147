using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;
using Model.Resultados;
using Model.Comandos;

namespace Rules.Rules
{
    public class TareaPorAreaPorRequerimientoRules : BaseRules<TareaPorAreaPorRequerimiento>
    {

        private readonly TareaPorAreaPorRequerimientoDAO dao;

        public TareaPorAreaPorRequerimientoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = TareaPorAreaPorRequerimientoDAO.Instance;
        }


     }
}

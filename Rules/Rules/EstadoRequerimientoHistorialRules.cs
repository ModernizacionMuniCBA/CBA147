using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;

namespace Rules.Rules
{
    public class EstadoRequerimientoHistorialRules : BaseRules<EstadoRequerimientoHistorial>
    {

        private readonly EstadoRequerimientoHistorialDAO dao;

        public EstadoRequerimientoHistorialRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoRequerimientoHistorialDAO.Instance;
        }

        public Result<List<EstadoRequerimientoHistorial>> GetByFilters(int? idRQ, bool? dadosDeBaja)
        {
            return dao.GetByFilters(idRQ, dadosDeBaja);
        }
    }
}

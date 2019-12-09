using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;

namespace Rules.Rules
{
    public class EstadoMovilHistorialRules : BaseRules<EstadoMovilHistorial>
    {

        private readonly EstadoMovilHistorialDAO dao;

        public EstadoMovilHistorialRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoMovilHistorialDAO.Instance;
        }

        //public Result<List<EstadoRequerimientoHistorial>> GetByFilters(int? idRQ, bool? dadosDeBaja)
        //{
        //    return dao.GetByFilters(idRQ, dadosDeBaja);
        //}
    }
}

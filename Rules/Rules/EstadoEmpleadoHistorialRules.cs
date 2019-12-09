using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;

namespace Rules.Rules
{
    public class EstadoEmpleadoHistorialRules : BaseRules<EstadoEmpleadoHistorial>
    {

        private readonly EstadoEmpleadoHistorialDAO dao;

        public EstadoEmpleadoHistorialRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoEmpleadoHistorialDAO.Instance;
        }

        //public Result<List<EstadoRequerimientoHistorial>> GetByFilters(int? idRQ, bool? dadosDeBaja)
        //{
        //    return dao.GetByFilters(idRQ, dadosDeBaja);
        //}
    }
}

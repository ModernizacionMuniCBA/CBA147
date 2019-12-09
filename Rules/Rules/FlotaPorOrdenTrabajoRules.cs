using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;

namespace Rules.Rules
{
    public class FlotaPorOrdenTrabajoRules : BaseRules<FlotaPorOrdenTrabajo>
    {

        private readonly FlotaPorOrdenTrabajoDAO dao;

        public FlotaPorOrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = FlotaPorOrdenTrabajoDAO.Instance;
        }

        //public Result<List<EstadoRequerimientoHistorial>> GetByFilters(int? idRQ, bool? dadosDeBaja)
        //{
        //    return dao.GetByFilters(idRQ, dadosDeBaja);
        //}
    }
}

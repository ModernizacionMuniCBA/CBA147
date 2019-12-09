using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;

namespace Rules.Rules
{
    public class EstadoOrdenEspecialHistorialRules : BaseRules<EstadoOrdenEspecialHistorial>
    {

        private readonly EstadoOrdenEspecialHistorialDAO dao;

        public EstadoOrdenEspecialHistorialRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoOrdenEspecialHistorialDAO.Instance;
        }

        //public Result<List<EstadoRequerimientoHistorial>> GetByFilters(int? idRQ, bool? dadosDeBaja)
        //{
        //    return dao.GetByFilters(idRQ, dadosDeBaja);
        //}
    }
}

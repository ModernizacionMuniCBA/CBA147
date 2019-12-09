using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;

namespace Rules.Rules
{
    public class EstadoFlotaHistorialRules : BaseRules<EstadoFlotaHistorial>
    {

        private readonly EstadoFlotaHistorialDAO dao;

        public EstadoFlotaHistorialRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadoFlotaHistorialDAO.Instance;
        }

        //public Result<List<EstadoRequerimientoHistorial>> GetByFilters(int? idRQ, bool? dadosDeBaja)
        //{
        //    return dao.GetByFilters(idRQ, dadosDeBaja);
        //}
    }
}

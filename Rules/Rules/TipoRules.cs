using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class TipoRules : BaseRules<TipoRequerimiento>
    {

        private readonly TipoDAO dao;

        public TipoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = TipoDAO.Instance;
        }

        public Result<List<TipoRequerimiento>> GetByFilters(string nombre, Enums.TipoRequerimiento tipo, bool? dadosDeBaja)
        {
            return dao.GetByFilters(nombre, tipo, dadosDeBaja);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class TemaRules : BaseRules<Tema>
    {
            private readonly TemaDAO dao;

            public TemaRules(UsuarioLogueado data)
                : base(data)
        {
            dao = TemaDAO.Instance;
        }

        public Result<List<Tema>> GetByFilters(int? idServicio, string nombre, bool? dadosDeBaja)
        {
            return dao.GetByFilters(idServicio, nombre, dadosDeBaja);
        }

      
    }
}

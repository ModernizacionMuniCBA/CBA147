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
    public class MovilPorOrdenTrabajoRules : BaseRules<MovilPorOrdenTrabajo>
    {
        private readonly MovilXOrdenTrabajoDAO dao;

        public MovilPorOrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = MovilXOrdenTrabajoDAO.Instance;
        }


        public virtual Result<List<MovilPorOrdenTrabajo>> GetByIdOrdenTrabajo(int idOrdenTrabajo, bool dadosDeBaja)
        {
            return dao.GetByIdOrdenTrabajo(idOrdenTrabajo, dadosDeBaja);
        }

        public virtual Result<List<int>> GetIdsByIdOrdenTrabajo(int idOrdenTrabajo, bool dadosDeBaja)
        {
            return dao.GetIdsByIdOrdenTrabajo(idOrdenTrabajo, dadosDeBaja);
        }
    }
}

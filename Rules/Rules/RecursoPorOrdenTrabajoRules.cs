using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;

namespace Rules.Rules
{
    public class RecursoPorOrdenTrabajoRules : BaseRules<RecursoPorOrdenTrabajo>
    {
      
        private readonly RecursoPorOrdenTrabajoDAO dao;

        public RecursoPorOrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = RecursoPorOrdenTrabajoDAO.Instance;
        }

        public override Result<RecursoPorOrdenTrabajo> ValidateDatosNecesarios(RecursoPorOrdenTrabajo entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            if (entity.OrdenTrabajo == null)
            {
                result.AddErrorPublico("Debe indicar la orden de trabajo");
            }

            return result;
        }

        public Result<bool> DeleteByOrdenTrabajo(int idOT)
        {
            return dao.DeleteByIdOrdenTrabajo(idOT);
        }

        public Result<List<RecursoPorOrdenTrabajo>> GetByIdOrdenTrabajo(int idOT) {
            return dao.GetByIdOrdenTrabajo(idOT);
        }
    }
}

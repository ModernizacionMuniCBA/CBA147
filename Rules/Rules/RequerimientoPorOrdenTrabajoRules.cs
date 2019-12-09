using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;

namespace Rules.Rules
{
    public class RequerimientoPorOrdenTrabajoRules : BaseRules<RequerimientoPorOrdenTrabajo>
    {
    
        private readonly RequerimientoPorOrdenTrabajoDAO dao;

        public RequerimientoPorOrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = RequerimientoPorOrdenTrabajoDAO.Instance;
        }

        public Result<int> GetCantidadRequerimientosByIdOT(int idOt)
        {
            return dao.GetCantidadRequerimientosByIdOT(idOt);
        }

        public override Result<RequerimientoPorOrdenTrabajo> ValidateDatosNecesarios(RequerimientoPorOrdenTrabajo entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            if (entity.Requerimiento == null)
            {
                result.AddErrorPublico("Debe indicar el requerimiento");
            }

            if (entity.OrdenTrabajo == null)
            {
                result.AddErrorPublico("Debe indicar la orden de trabajo");
            }

            return result;
        }
    }
}

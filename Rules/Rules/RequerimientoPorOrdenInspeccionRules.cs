using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;

namespace Rules.Rules
{
    public class RequerimientoPorOrdenInspeccionRules : BaseRules<RequerimientoPorOrdenInspeccion>
    {
    
        private readonly RequerimientoPorOrdenInspeccionDAO dao;

        public RequerimientoPorOrdenInspeccionRules(UsuarioLogueado data)
            : base(data)
        {
            dao = RequerimientoPorOrdenInspeccionDAO.Instance;
        }

        public override Result<RequerimientoPorOrdenInspeccion> ValidateDatosNecesarios(RequerimientoPorOrdenInspeccion entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            if (entity.Requerimiento == null)
            {
                result.AddErrorPublico("Debe indicar el requerimiento");
            }

            if (entity.OrdenInspeccion == null)
            {
                result.AddErrorPublico("Debe indicar la orden de inspección");
            }

            return result;
        }
    }
}

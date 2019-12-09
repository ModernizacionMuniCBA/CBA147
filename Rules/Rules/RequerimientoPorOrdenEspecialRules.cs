using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;

namespace Rules.Rules
{
    public class RequerimientoPorOrdenEspecialRules : BaseRules<RequerimientoPorOrdenEspecial>
    {
    
        private readonly RequerimientoPorOrdenEspecialDAO dao;

        public RequerimientoPorOrdenEspecialRules(UsuarioLogueado data)
            : base(data)
        {
            dao = RequerimientoPorOrdenEspecialDAO.Instance;
        }

        public override Result<RequerimientoPorOrdenEspecial> ValidateDatosNecesarios(RequerimientoPorOrdenEspecial entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            if (entity.Requerimiento == null)
            {
                result.AddErrorPublico("Debe indicar el requerimiento");
            }

            if (entity.OrdenEspecial == null)
            {
                result.AddErrorPublico("Debe indicar la orden de trabajo");
            }

            return result;
        }

        //public Result<Object> DeleteByOrdenTrabajo(int idOT)
        //{
        //    return dao.DeleteByIdOrdenTrabajo(idOT);
        //}

        //public Result<List<RequerimientoXOrdenTrabajo>> GetByIdOrdenTrabajo(int idOT) {
        //    return dao.GetByIdOrdenTrabajo(idOT);
        //}

        //public Result<List<RequerimientoXOrdenTrabajo>> GetByIdRequerimiento(int idRq)
        //{
        //    return dao.GetByIdRequerimiento(idRq);
        //}

    }
}

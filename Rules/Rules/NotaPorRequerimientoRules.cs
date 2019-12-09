using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class NotaPorRequerimientoRules : BaseRules<NotaPorRequerimiento>
    {
      
        private readonly NotaPorRequerimientoDAO dao;

        public NotaPorRequerimientoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = NotaPorRequerimientoDAO.Instance;
        }

        /* Validaciones */

        public override Result<NotaPorRequerimiento> ValidateDatosNecesarios(NotaPorRequerimiento entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Requerimiento
            if(entity.Requerimiento==null){
                result.AddErrorPublico("Debe ingresar el requerimiento");
                return result;
            }

            //Obeservaciones
            if (string.IsNullOrEmpty(entity.Observaciones))
            {
                result.AddErrorPublico("Debe ingresar el contenido de la nota");
                return result;
            }

            return result;
        }

        /* Busqueda */

        public Result<List<NotaPorRequerimiento>> GetByFilters(int? idOrdenTrabajo, int? idRequerimiento, bool? dadosDeBaja)
        {
            return dao.GetByFilters(idOrdenTrabajo, idRequerimiento, dadosDeBaja);
        }
    }
}

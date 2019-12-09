using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class NotaPorMovilRules : BaseRules<NotaPorMovil>
    {

        private readonly NotaPorMovilDAO dao;

        public NotaPorMovilRules(UsuarioLogueado data)
            : base(data)
        {
            dao = NotaPorMovilDAO.Instance;
        }

        /* Validaciones */

        public override Result<NotaPorMovil> ValidateDatosNecesarios(NotaPorMovil entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Movil
            if(entity.Movil==null){
                result.AddErrorPublico("Debe ingresar el móvil");
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

        public Result<List<NotaPorMovil>> GetByFilters(int? idMovil, bool? dadosDeBaja)
        {
            return dao.GetByFilters(idMovil,  dadosDeBaja);
        }
    }
}

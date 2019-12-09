using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class NotaPorOrdenTrabajoRules : BaseRules<NotaPorOrdenTrabajo>
    {

        private readonly NotaOrdenTrabajoDAO dao;

        public NotaPorOrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = NotaOrdenTrabajoDAO.Instance;
        }

        /* Validaciones */

        public override Result<NotaPorOrdenTrabajo> ValidateDatosNecesarios(NotaPorOrdenTrabajo entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Obeservaciones
            if (string.IsNullOrEmpty(entity.Observaciones))
            {
                result.AddErrorPublico("Debe ingresar el contenido de la nota");
            }

            return result;
        }

        /* Busqueda */

        public Result<List<NotaPorOrdenTrabajo>> GetByFilters(int? idOrdenTrabajo, bool? dadosDeBaja)
        {
            return dao.GetByFilters(idOrdenTrabajo, dadosDeBaja);
        }

    

        //public Result<NotaXOrdenTrabajo> DarDeBajaNota(int idNota)
        //{
        //    var result = new Result<NotaXOrdenTrabajo>();

        //    var resultQueryNota = GetById(idNota);
        //    if (!resultQueryNota.Ok) {
        //        result.Copy(resultQueryNota);
        //        return result;
        //    }

        //    var nota = resultQueryNota.Return;
        //    if (nota == null) {
        //        result.AddErrorInterno("Nota no encontrada");
        //        return result;
        //    }


        //    var resultDelete = ValidateDelete(nota);
        //    if (!resultDelete.Ok) {
        //        result.Copy(resultDelete);
        //        return result;
        //    }

        //    //Actualizo la orden
        //    var resultOrden = OrdenTrabajoRules.Instance.Update(nota.OrdenTrabajo);
        //    if (!resultOrden.Ok)
        //    {
        //        result.Copy(resultOrden);
        //        return result;
        //    }

        //    result.Return = nota;
        //    return result;
        //}

    }
}

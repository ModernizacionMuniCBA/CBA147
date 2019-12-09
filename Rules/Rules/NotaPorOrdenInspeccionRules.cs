using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;

namespace Rules.Rules
{
    public class NotaPorOrdenInspeccionRules : BaseRules<NotaPorOrdenInspeccion>
    {

        private readonly NotaOrdenInspeccionDAO dao;

        public NotaPorOrdenInspeccionRules(UsuarioLogueado data)
            : base(data)
        {
            dao = NotaOrdenInspeccionDAO.Instance;
        }

        /* Validaciones */

        public override Result<NotaPorOrdenInspeccion> ValidateDatosNecesarios(NotaPorOrdenInspeccion entity)
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

        public Result<List<NotaPorOrdenInspeccion>> GetByFilters(int? idOrdenInspeccion, bool? dadosDeBaja)
        {
            return dao.GetByFilters(idOrdenInspeccion, dadosDeBaja);
        }

    

        //public Result<NotaXOrdenInspeccion> DarDeBajaNota(int idNota)
        //{
        //    var result = new Result<NotaXOrdenInspeccion>();

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
        //    var resultOrden = OrdenInspeccionRules.Instance.Update(nota.OrdenInspeccion);
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

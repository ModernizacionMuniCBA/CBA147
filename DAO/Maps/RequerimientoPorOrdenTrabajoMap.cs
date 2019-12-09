using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class RequerimientoPorOrdenTrabajoMap : BaseEntityMap<RequerimientoPorOrdenTrabajo>
    {
        public RequerimientoPorOrdenTrabajoMap()
        {
            //Tabla
            Table("RequerimientoPorOrdenTrabajo");

            //Requerimiento
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();

            //Orden Trabajo
            References(x => x.OrdenTrabajo, "IdOrdenTrabajo").Not.Nullable();
        }
    }
}

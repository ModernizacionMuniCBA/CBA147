using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class FlotaPorOrdenTrabajoMap : BaseEntityMap<FlotaPorOrdenTrabajo>
    {
        public FlotaPorOrdenTrabajoMap()
        {
            //Tabla
            Table("FlotaPorOrdenTrabajo");

            //Empleado
            References(x => x.Flota, "IdFlota").Not.Nullable();

            //Orden Trabajo
            References(x => x.OrdenTrabajo, "IdOrdenTrabajo").Not.Nullable();

        }
    }
}

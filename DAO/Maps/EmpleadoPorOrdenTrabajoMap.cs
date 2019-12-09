using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EmpleadoPorOrdenTrabajoMap : BaseEntityMap<EmpleadoPorOrdenTrabajo>
    {
        public EmpleadoPorOrdenTrabajoMap()
        {
            //Tabla
            Table("EmpleadoPorOrdenTrabajo");

            //Empleado
            References(x => x.Empleado, "IdEmpleado").Not.Nullable();

            //Orden Trabajo
            References(x => x.OrdenTrabajo, "IdOrdenTrabajo").Not.Nullable();

            //Seccion
            References(x => x.Seccion, "IdSeccion").Nullable();
        }
    }
}

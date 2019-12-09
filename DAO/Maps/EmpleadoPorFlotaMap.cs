using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EmpleadoPorFlotaMap : BaseEntityMap<EmpleadoPorFlota>
    {
        public EmpleadoPorFlotaMap()
        {
            //Tabla
            Table("EmpleadoPorFlota");

            //Empleado
            References(x => x.Empleado, "IdEmpleado").Not.Nullable();

            //Orden Trabajo
            References(x => x.Flota, "IdFlota").Not.Nullable();

            //Seccion
            //References(x => x.Seccion, "IdSeccion").Nullable();
        }
    }
}

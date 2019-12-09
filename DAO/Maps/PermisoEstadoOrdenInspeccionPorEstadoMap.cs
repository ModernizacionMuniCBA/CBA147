using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class PermisoEstadoOrdenInspeccionPorEstadoMap : BaseEntityMap<PermisoEstadoOrdenInspeccionPorEstado>
    {
        public PermisoEstadoOrdenInspeccionPorEstadoMap()
        {
            //Tabla
            Table("PermisoEstadoOrdenInspeccionPorEstado");

            References(x => x.EstadoOrdenInspeccion, "IdEstado").Not.Nullable();
            References(x => x.Permiso, "IdPermiso").Not.Nullable();
        }
    }
}

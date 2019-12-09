using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class PermisoEstadoOrdenTrabajoPorEstadoMap : BaseEntityMap<PermisoEstadoOrdenTrabajoPorEstado>
    {
        public PermisoEstadoOrdenTrabajoPorEstadoMap()
        {
            //Tabla
            Table("PermisoEstadoOrdenTrabajoPorEstado");

            References(x => x.EstadoOrdenTrabajo, "IdEstado").Not.Nullable();
            References(x => x.Permiso, "IdPermiso").Not.Nullable();
        }
    }
}

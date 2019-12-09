using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class PermisoEstadoRequerimientoPorEstadoMap : BaseEntityMap<PermisoEstadoRequerimientoPorEstado>
    {
        public PermisoEstadoRequerimientoPorEstadoMap()
        {
            //Tabla
            Table("PermisoEstadoRequerimientoPorEstado");

            References(x => x.EstadoRequerimiento, "IdEstado").Not.Nullable();
            References(x => x.Permiso, "IdPermiso").Not.Nullable();
        }
    }
}

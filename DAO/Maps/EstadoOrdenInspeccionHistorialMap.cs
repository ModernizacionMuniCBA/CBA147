using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoOrdenInspeccionHistorialMap : BaseEntityMap<EstadoOrdenInspeccionHistorial>
    {
        public EstadoOrdenInspeccionHistorialMap()
        {
            //Tabla
            Table("EstadoOrdenInspeccionHistorial");

            //Requerimiento
            References(x => x.OrdenInspeccion, "IdOrdenInspeccion").Not.Nullable();

            //Estado
            References(x => x.Estado, "IdEstado").Not.Nullable();

            //Fecha
            Map(x => x.Fecha, "Fecha").Not.Nullable();

            //Utimo
            Map(x => x.Ultimo, "Ultimo").Nullable();
        }
    }
}

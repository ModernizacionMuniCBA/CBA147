using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoOrdenTrabajoHistorialMap : BaseEntityMap<EstadoOrdenTrabajoHistorial>
    {
        public EstadoOrdenTrabajoHistorialMap()
        {
            //Tabla
            Table("EstadoOrdenTrabajoHistorial");

            //Requerimiento
            References(x => x.OrdenTrabajo, "IdOrdenTrabajo").Not.Nullable();

            //Estado
            References(x => x.Estado, "IdEstado").Not.Nullable();

            //Fecha
            Map(x => x.Fecha, "Fecha").Not.Nullable();

            //Utimo
            Map(x => x.Ultimo, "Ultimo").Nullable();
        }
    }
}

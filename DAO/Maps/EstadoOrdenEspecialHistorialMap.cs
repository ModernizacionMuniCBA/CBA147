using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoOrdenEspecialHistorialMap : BaseEntityMap<EstadoOrdenEspecialHistorial>
    {
        public EstadoOrdenEspecialHistorialMap()
        {
            //Tabla
            Table("EstadoOrdenEspecialHistorial");

            //Requerimiento
            References(x => x.OrdenEspecial, "IdOrdenEspecial").Not.Nullable();

            //Estado
            References(x => x.Estado, "IdEstado").Not.Nullable();

            //Fecha
            Map(x => x.Fecha, "Fecha").Not.Nullable();

            //Utimo
            Map(x => x.Ultimo, "Ultimo").Nullable();
        }
    }
}

using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoRequerimientoHistorialMap : BaseEntityMap<EstadoRequerimientoHistorial>
    {
        public EstadoRequerimientoHistorialMap()
        {
            //Tabla
            Table("EstadoRequerimientoHistorial");

            //Requerimiento
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();

            //Estado
            References(x => x.Estado, "IdEstado").Not.Nullable();

            //Fecha
            Map(x => x.Fecha, "Fecha").Not.Nullable();

            //Utimo
            Map(x => x.Ultimo, "Ultimo").Nullable();
        }
    }
}

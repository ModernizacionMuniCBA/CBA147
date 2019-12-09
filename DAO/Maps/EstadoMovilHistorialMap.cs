using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoMovilHistorialMap : BaseEntityMap<EstadoMovilHistorial>
    {
        public EstadoMovilHistorialMap()
        {
            //Tabla
            Table("EstadoMovilHistorial");

            //Movil
            References(x => x.Movil, "IdMovil").Not.Nullable();

            //Estado
            References(x => x.Estado, "IdEstado").Not.Nullable();

            //Estado
            Map(x => x.Ultimo, "Ultimo").Not.Nullable();

            //Fecha
            Map(x => x.Fecha, "Fecha").Not.Nullable();

        }
    }
}

using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoFlotaHistorialMap : BaseEntityMap<EstadoFlotaHistorial>
    {
        public EstadoFlotaHistorialMap()
        {
            //Tabla
            Table("EstadoFlotaHistorial");

            //Flota
            References(x => x.Flota, "IdFlota").Not.Nullable();

            //Estado
            References(x => x.Estado, "IdEstado").Not.Nullable();

            //Estado
            Map(x => x.Ultimo, "Ultimo").Not.Nullable();

            //Fecha
            Map(x => x.Fecha, "Fecha").Not.Nullable();

        }
    }
}

using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class ITVPorMovilMap : BaseEntityMap<ITVPorMovil>
    {
        public ITVPorMovilMap()
        {
            //Tabla
            Table("ITVPorMovil");

            //Movil
            References(x => x.Movil, "IdMovil").Not.Nullable();

            //Fecha Ultimo ITV
            Map(x => x.FechaUltimoITV, "FechaUltimoITV").Nullable();

            //Fecha Vencimiento ITV
            Map(x => x.FechaVencimientoITV, "FechaVencimientoITV").Not.Nullable();
        }
    }
}

using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class TUVPorMovilMap : BaseEntityMap<TUVPorMovil>
    {
        public TUVPorMovilMap()
        {
            //Tabla
            Table("TUVPorMovil");

            //Movil
            References(x => x.Movil, "IdMovil").Not.Nullable();

            //Fecha ultimo TUV
            Map(x => x.FechaUltimoTUV, "FechaUltimoTUV").Nullable();

            //Fecha vencimiento TUV
            Map(x => x.FechaVencimientoTUV, "FechaVencimientoTUV").Not.Nullable();
        }
    }
}

using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class ValuacionPorMovilMap : BaseEntityMap<ValuacionPorMovil>
    {
        public ValuacionPorMovilMap()
        {
            //Tabla
            Table("ValuacionPorMovil");

            //Movil
            References(x => x.Movil, "IdMovil").Not.Nullable();

            //Fecha Valuacion
            Map(x => x.FechaValuacion, "FechaValuacion").Not.Nullable();

            //Valor
            Map(x => x.Valor, "Valor").Not.Nullable();
        }
    }
}

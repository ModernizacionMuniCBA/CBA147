using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class ReparacionPorMovilMap : BaseEntityMap<ReparacionPorMovil>
    {
        public ReparacionPorMovilMap()
        {
            //Tabla
            Table("ReparacionPorMovil");

            //Movil
            References(x => x.Movil, "IdMovil").Not.Nullable();

            //Fecha Valuacion
            Map(x => x.FechaReparacion, "FechaReparacion").Not.Nullable();

            //Valor
            Map(x => x.MontoReparacion, "MontoReparacion").Nullable();

            //Falla
            Map(x => x.Motivo, "Motivo").Nullable();

            //Taller
            Map(x => x.Taller, "Taller").Nullable();  
        }
    }
}

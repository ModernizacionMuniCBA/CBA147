using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class ZonaMap : BaseEntityMap<Zona>
    {
        public ZonaMap()
        {
            //Tabla
            Table("Zona");

            //Nombre
            Map(x => x.Nombre, "Nombre").Nullable().Length(100);

            //Color
            References(x => x.Color, "IdColor").Not.Nullable();

            //Area
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();

            //Barrios
            HasMany(x => x.BarriosPorZona).Table("BarrioPorZona").KeyColumn("IdZona").Cascade.All();
        }
    }
}

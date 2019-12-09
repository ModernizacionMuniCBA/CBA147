using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class TareaPorAreaMap : BaseEntityMap<TareaPorArea>
    {
        public TareaPorAreaMap()
        {
            //Tabla
            Table("TareaPorArea");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //Area
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();
        }
    }
}

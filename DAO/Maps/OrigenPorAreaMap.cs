using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class OrigenPorAreaMap : BaseEntityMap<OrigenPorArea>
    {
        public OrigenPorAreaMap()
        {
            //Tabla
            Table("OrigenPorArea");

            References(x => x.Origen, "IdOrigen").Not.Nullable();

            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();
        }
    }
}

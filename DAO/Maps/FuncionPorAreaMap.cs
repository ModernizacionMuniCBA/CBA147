using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class FuncionPorAreaMap : BaseEntityMap<FuncionPorArea>
    {
        public FuncionPorAreaMap()
        {
            //Tabla
            Table("FuncionPorArea");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //Area
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable() ;
        }
    }
}

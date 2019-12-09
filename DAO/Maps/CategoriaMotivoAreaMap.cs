using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class CategoriaMotivoAreaMap : BaseEntityMap<CategoriaMotivoArea>
    {
        public CategoriaMotivoAreaMap()
        {
            //Tabla
            Table("CategoriaMotivoArea");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //Area
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();

        }
    }
}

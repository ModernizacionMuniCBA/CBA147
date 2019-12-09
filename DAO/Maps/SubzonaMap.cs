using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class SubzonaMap : BaseEntityMap<Subzona>
    {
              public SubzonaMap()
        {
            //Tabla
            Table("Subzona");

            //Nombre
            Map(x => x.Nombre, "Nombre").Nullable().Length(100);

            //Zona
            References(x => x.Zona, "IdZona").Not.Nullable();
        }
    }
}

using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class OrigenMap : BaseEntityMap<Origen>
    {
        public OrigenMap()
        {
            //Tabla
            Table("Origen");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable().Length(200);
            Map(x => x.KeyAlias, "KeyAlias").Not.Nullable().Length(200);
            Map(x => x.KeySecret, "KeySecret").Not.Nullable().Length(200);
        }
    }
}

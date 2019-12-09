using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class CpcMap : BaseEntityMap<Cpc>
    {
        public CpcMap()
        {
            //Tabla
            Table("Cpc");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //IdCtastro
            Map(x => x.IdCatastro, "IdCatastro").Nullable();

            //Numero
            Map(x => x.Numero, "Numero").Nullable();
        }
    }
}

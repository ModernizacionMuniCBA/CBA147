using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class BarrioMap : BaseEntityMap<Barrio>
    {
        public BarrioMap()
        {
            //Tabla
            Table("Barrio");

            //Nombre
            Map(x => x.Nombre, "Nombre").Nullable().Length(100);

            //IdCatastro
            Map(x => x.IdCatastro, "IdCatastro");
        }
    }
}

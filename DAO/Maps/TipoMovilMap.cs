using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class TipoMovilMap : BaseEntityMap<TipoMovil>
    {
        public TipoMovilMap() {
            
            //Tabla
            Table("TipoMovil");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable().Length(100);
        }
    }
}

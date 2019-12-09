using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class TipoRequerimientoMap : BaseEntityMap<TipoRequerimiento>
    {
        public TipoRequerimientoMap() {
            
            //Tabla
            Table("TipoRequerimiento");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable().Length(100);

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.TipoRequerimiento)).Not.Nullable();
        }
    }
}

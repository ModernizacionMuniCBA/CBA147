using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class TipoDocumentoMap : BaseEntityMap<TipoDocumento>
    {

        public TipoDocumentoMap()
        {
            //Tabla
            Table("TipoDocumento");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.TipoDocumento)).Not.Nullable();
        }
    }
}

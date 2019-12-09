using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class TipoCampoMap : BaseEntityMap<TipoCampo>
    {
        public TipoCampoMap()
        {
            //Tabla
            Table("TipoCampo");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.TipoCampoPorMotivo)).Not.Nullable();
        }
    }
}

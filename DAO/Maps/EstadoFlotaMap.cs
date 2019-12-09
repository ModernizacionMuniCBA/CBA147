using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoFlotaMap : BaseEntityMap<EstadoFlota>
    {
        public EstadoFlotaMap()
        {
            //Tabla
            Table("EstadoFlota");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.EstadoFlota)).Not.Nullable();

            //Color
            Map(x => x.Color, "Color").Not.Nullable();
        }
    }
}

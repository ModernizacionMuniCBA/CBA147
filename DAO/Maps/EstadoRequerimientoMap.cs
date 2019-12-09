using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoRequerimientoMap : BaseEntityMap<EstadoRequerimiento>
    {
        public EstadoRequerimientoMap()
        {
            //Tabla
            Table("EstadoRequerimiento");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.EstadoRequerimiento)).Not.Nullable();

            //KeyValue Publico
            Map(x => x.KeyValuePublico, "KeyValuePublico").CustomType(typeof(Enums.EstadoRequerimiento)).Nullable();

            //Color
            Map(x => x.Color, "Color").Not.Nullable();

            //Orden
            Map(x => x.Orden, "Orden");
        }
    }
}

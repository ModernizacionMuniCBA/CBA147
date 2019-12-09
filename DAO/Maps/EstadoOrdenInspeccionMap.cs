using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoOrdenInspeccionMap : BaseEntityMap<EstadoOrdenInspeccion>
    {
        public EstadoOrdenInspeccionMap()
        {
            //Tabla
            Table("EstadoOrdenInspeccion");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.EstadoOrdenInspeccion)).Not.Nullable();
        
            //Color
            Map(x => x.Color, "Color").Not.Nullable();
        }
    }
}

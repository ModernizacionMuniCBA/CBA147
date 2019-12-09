using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoMovilMap : BaseEntityMap<EstadoMovil>
    {
        public EstadoMovilMap()
        {
            //Tabla
            Table("EstadoMovil");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.EstadoMovil)).Not.Nullable();

            //Color
            Map(x => x.Color, "Color").Not.Nullable();
        }
    }
}

using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class EstadoOrdenEspecialMap : BaseEntityMap<EstadoOrdenEspecial>
    {
        public EstadoOrdenEspecialMap()
        {
            //Tabla
            Table("EstadoOrdenEspecial");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //KeyValue
            Map(x => x.KeyValue, "KeyValue").CustomType(typeof(Enums.EstadoOrdenEspecial)).Not.Nullable();

            //Color
            Map(x => x.Color, "Color").Not.Nullable();

        }
    }
}

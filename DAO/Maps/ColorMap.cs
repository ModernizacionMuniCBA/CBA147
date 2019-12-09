using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class ColorMap : BaseEntityMap<Color>
    {
        public ColorMap()
        {
            //Tabla
            Table("Color");

            //Color
            Map(x => x.Valor, "Valor").Not.Nullable();

        }
    }
}

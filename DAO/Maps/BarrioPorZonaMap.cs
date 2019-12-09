using System;
using System.Collections.Generic;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class BarrioPorZonaMap : BaseEntityMap<BarrioPorZona>
    {
        public BarrioPorZonaMap()
        {
            //Tabla
            Table("BarrioPorZona");

            //Barrio
            References(x => x.Barrio, "IdBarrio").Not.Nullable();

            //Zona
            References(x => x.Zona, "IdZona").Nullable();
        }
    }
}

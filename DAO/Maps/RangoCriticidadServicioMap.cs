using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class RangoCriticidadServicioMap : BaseEntityMap<RangoCriticidadServicio>
    {
        public RangoCriticidadServicioMap()
        {
            //Tabla
            Table("RangoCriticidadServicio");

            //Servicio
            Map(x => x.IdServicio, "IdServicio");
            //References(x => x.Servicio, "IdServicio").Not.Nullable();

            
            Map(x => x.Desde, "Desde").Not.Nullable();
       
            Map(x => x.Color, "Color").Not.Nullable();


        }
    }
}

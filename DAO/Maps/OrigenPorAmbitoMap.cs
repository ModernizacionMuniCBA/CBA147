using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class OrigenPorAmbitoMap : BaseEntityMap<OrigenPorAmbito>
    {
        public OrigenPorAmbitoMap()
        {
            //Tabla
            Table("OrigenPorAmbito");

            References(x => x.Origen, "IdOrigen").Not.Nullable();
            References(x => x.Ambito, "IdAmbitoCerrojo").Not.Nullable();
        }
    }
}

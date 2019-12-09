using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class CampoPorMotivoPorRequerimientoMap : BaseEntityMap<CampoPorMotivoPorRequerimiento>
    {
        public CampoPorMotivoPorRequerimientoMap()
        {
            //Tabla
            Table("CampoPorMotivoPorRequerimiento");

            //Requerimiento
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();

            //Campo
            References(x => x.CampoPorMotivo, "IdCampoPorMotivo").Not.Nullable();

            //Valor
            Map(x => x.Valor, "Valor");
        }
    }
}

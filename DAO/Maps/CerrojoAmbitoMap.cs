using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class CerrojoAmbitoMap : BaseEntityMap<CerrojoAmbito>
    {
        public CerrojoAmbitoMap()
        {
            //Tabla
            Table("CerrojoAmbito");
            ReadOnly();

            //Nombre
            Map(x => x.Nombre, "Nombre");

            //KeyValue
            Map(x => x.KeyValue);
        }
    }
}

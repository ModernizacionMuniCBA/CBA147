using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class KilometrajePorMovilMap : BaseEntityMap<KilometrajePorMovil>
    {
        public KilometrajePorMovilMap()
        {
            //Tabla
            Table("KilometrajePorMovil");

            //Movil
            References(x => x.Movil, "IdMovil").Not.Nullable();

            //Estado
            Map(x => x.Kilometraje, "Kilometraje").Not.Nullable();

            //Fecha
            Map(x => x.FechaKilometraje, "FechaKilometraje").Not.Nullable();
        }
    }
}

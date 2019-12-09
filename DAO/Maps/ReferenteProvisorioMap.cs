using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Maps
{
     class ReferenteProvisorioMap: BaseEntityMap<ReferenteProvisorio>
    {
        public ReferenteProvisorioMap() {

            //Tabla
            Table("ReferenteProvisorio");

            Map(z => z.Apellido, "Apellido");
            Map(z => z.Nombre, "Nombre");
            Map(z => z.DNI, "DNI");
            Map(z => z.GeneroMasculino, "SexoMasculino");
            Map(z => z.Telefono, "Telefono");
        }

    }
}

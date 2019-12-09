using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class InformacionOrganicaSecreatariaMap : BaseEntityMap<InformacionOrganicaSecretaria>
    {
        public InformacionOrganicaSecreatariaMap()
        {
            //Tabla
            Table("InformacionOrganicaSecretaria");
            Map(x => x.Nombre, "Nombre").Not.Nullable();
        }
    }
}

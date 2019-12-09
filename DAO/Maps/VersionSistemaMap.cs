using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class VersionSistemaMap : BaseEntityMap<VersionSistema>
    {
        public VersionSistemaMap()
        {
            //Tabla
            Table("VersionSistema");

            //Nombre
            Map(x => x.Version, "Version");
        }
    }
}

using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class UsuarioReferentePorRequerimientoMap : BaseEntityMap<UsuarioReferentePorRequerimiento>
    {
        public UsuarioReferentePorRequerimientoMap()
        {
            //Tabla
            Table("UsuarioReferentePorRequerimiento");
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();
            References(x => x.UsuarioReferente, "IdUsuarioReferente").Nullable();
        }
    }
}

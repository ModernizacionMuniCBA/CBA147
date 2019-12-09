using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class OrigenPorUsuarioMap : BaseEntityMap<OrigenPorUsuario>
    {
        public OrigenPorUsuarioMap()
        {
            //Tabla
            Table("OrigenPorUsuario");

            References(x => x.Origen, "IdOrigen").Not.Nullable();
            References(x => x.UsuarioOrigen, "IdUsuarioCerrojoOrigen").Not.Nullable();
        }
    }
}

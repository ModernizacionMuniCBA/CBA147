using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class RequerimientoFavoritoPorUsuarioMap : BaseEntityMap<RequerimientoFavoritoPorUsuario>
    {
        public RequerimientoFavoritoPorUsuarioMap()
        {
            //Tabla
            Table("RequerimientoFavoritoPorUsuario");
            References(x => x.User, "IdUsuarioCerrojoFavorito").Not.Nullable();
            References(x => x.Requerimiento, "IdRequerimiento").Not.Nullable();
        }
    }
}

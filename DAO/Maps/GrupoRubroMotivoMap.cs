using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class GrupoRubroMotivoMap : BaseEntityMap<GrupoRubroMotivo>
    {
        public GrupoRubroMotivoMap()
        {
            Table("GrupoRubroMotivo");

            Map(x => x.Nombre, "Nombre");
            
        }
    }
}
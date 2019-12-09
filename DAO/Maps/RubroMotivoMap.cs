using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class RubroMotivoMap : BaseEntityMap<RubroMotivo>
    {
        public RubroMotivoMap()
        {
            Table("RubroMotivo");

            Map(x => x.Nombre, "Nombre");
            
            References(x => x.Grupo, "IdGrupoRubroMotivo");

            //Motivos
            HasMany(x => x.Motivos).Table("MotivoPorRubroMotivo").KeyColumn("IdRubroMotivo");
        }
    }
}
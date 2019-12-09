using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class CerrojoAreaMap : BaseEntityMap<CerrojoArea>
    {
        public CerrojoAreaMap()
        {
            //Tabla
            Table("CerrojoArea");

            //Nombre
            Map(x => x.Nombre, "Nombre").Nullable().Length(100);

            //Motivos
            HasMany(x => x.Motivos).Table("Motivo").KeyColumn("IdAreaCerrojo");

            //CodigoMunicipal
            Map(x => x.CodigoMunicipal, "CodigoMunicipal");

            //Orden Especial
            Map(x => x.CrearOrdenEspecial, "CrearOrdenEspecial");

            //Hijas
            HasMany(x => x.Subareas).Table("CerrojoArea").KeyColumn("IdPadre");

            //Padre
            References(x => x.AreaPadre, "IdPadre");

            //Padre
            References(x => x.TerritorioIncumbencia, "IdTerritorioIncumbencia");

            //Tipos de motivo por defecto
            HasMany(x => x.TiposMotivoPorDefecto).Table("ConfiguracionBandejaPorArea").KeyColumn("IdArea");
        }
    }
}

using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class TerritorioIncumbenciaMap : BaseEntityMap<TerritorioIncumbencia>
    {
        public TerritorioIncumbenciaMap()
        {
            Table("TerritorioIncumbencia");

            Map(x => x.Nombre, "Nombre");
            Map(x => x.Poligono, "Poligono");
            References(x => x.Area, "IdAreaCerrojo");
        }
    }
}
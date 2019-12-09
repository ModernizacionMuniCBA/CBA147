using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class CategoriaEdificioMunicipalMap : BaseEntityMap<CategoriaEdificioMunicipal>
    {
        public CategoriaEdificioMunicipalMap()
        {
            //Tabla
            Table("CategoriaEdificioMunicipal");

            //Nombre
            Map(x => x.Nombre, "Nombre").Not.Nullable();

            //Edificios
            HasMany(x => x.EdificiosMunicipales).Table("EdificioMunicipal").KeyColumn("IdCategoriaEdificioMunicipal")
                .Cascade.All();

        }
    }
}

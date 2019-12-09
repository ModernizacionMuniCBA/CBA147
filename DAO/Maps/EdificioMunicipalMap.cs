using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class EdificioMunicipalMap : BaseEntityMap<EdificioMunicipal>
    {
        public EdificioMunicipalMap()
        {
            //Tabla
            Table("EdificioMunicipal");

            //Categoria
            References(x => x.Categoria, "IdCategoriaEdificioMunicipal").Not.Nullable();

            //Nombre
            Map(x => x.Nombre, "Nombre");

            //Domicilio
            References(x => x.Domicilio, "IdDomicilio");

        }
    }
}

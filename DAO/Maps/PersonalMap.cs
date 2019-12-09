using System;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class PersonalMap : BaseEntityMap<Personal>
    {
        public PersonalMap()
        {
            //Tabla
            Table("Personal");

            //Persona
            References(x=>x.PersonaFisica, "IdPersonaFisica").Not.Nullable();

            //Area
            References(x=>x.Area, "IdAreaCerrojo").Not.Nullable();

        }
    }
}

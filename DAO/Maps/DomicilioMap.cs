using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class DomicilioMap : BaseEntityMap<Domicilio>
    {
        public DomicilioMap()
        {
            //Tabla
            Table("Domicilio");

            ////Calle
            //References(x => x.Calle, "IdCalle").Cascade.All().Nullable();

            //Barrio
            References(x => x.Barrio, "IdBarrio").Cascade.All().Not.Nullable();

            //Cpc
            References(x => x.Cpc, "IdCpc").Cascade.All().Nullable();

            Map(x => x.Direccion, "Direccion").Nullable();
            Map(x => x.Latitud, "Latitud").Nullable();
            Map(x => x.Longitud, "Longitud").Nullable();
            Map(x => x.Sugerido, "Sugerido");
            Map(x => x.Distancia, "Distancia");

            ////Altura
            //Map(x => x.Altura, "Altura").Nullable();

            ////Torre
            //Map(x => x.Torre, "Torre").Nullable();

            ////Departamento
            //Map(x => x.Departamento, "Departamento").Nullable();

            ////Piso
            //Map(x => x.Piso, "Piso").Nullable();

            ////X Catastro 
            //Map(x => x.Xcatastro, "Xcatastro");

            ////Y Catastro 
            //Map(x => x.Ycatastro, "Ycatastro");

            ////Manual
            //Map(x => x.PorBarrio, "Manual").Not.Nullable();

            ////Direccion GoogleMaps
            //Map(x => x.DireccionGoogleMaps, "DireccionGoogleMaps");

        }
    }
}

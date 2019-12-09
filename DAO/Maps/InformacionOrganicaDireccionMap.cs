using System;
using System.Linq;
using Model.Entities;

namespace DAO.Maps
{
    class InformacionOrganicaDireccionMap : BaseEntityMap<InformacionOrganicaDireccion>
    {
        public InformacionOrganicaDireccionMap()
        {
            //Tabla
            Table("InformacionOrganicaDireccion");
            References(x => x.Secretaria, "IdInformacionOrganicaSecretraria").Not.Nullable();
            References(x => x.UsuarioCreador, "IdUsuarioCerrojoCreador").Not.Nullable();

            Map(x => x.Nombre, "Nombre").Not.Nullable();

            Map(x => x.Responsable, "Responsable").Not.Nullable();
            Map(x => x.Domicilio, "Domicilio").Not.Nullable();
            Map(x => x.Email, "Email").Not.Nullable();
            Map(x => x.Telefono, "Telefono").Not.Nullable();

        }
    }
}

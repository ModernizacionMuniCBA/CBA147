using System;
using System.Linq;
using Model.Entities;
using Model;

namespace DAO.Maps
{
    class MovilMap : BaseEntityMap<Movil>
    {
        public MovilMap()
        {
            //Tabla
            Table("Movil");
            Map(x => x.Dominio).Not.Nullable();
            Map(x => x.FechaIncorporacion).Nullable();
            Map(x => x.Modelo).Not.Nullable();
            Map(x => x.Marca).Not.Nullable();
            Map(x => x.NumeroInterno).Nullable();
            Map(x => x.Carga).Nullable();
            Map(x => x.Asientos).Nullable();
            Map(x => x.Condicion, "Condicion").CustomType(typeof(Enums.CondicionMovil)).Nullable();
            References(x => x.Tipo, "IdTipoMovil").Not.Nullable();
            References(x => x.Area, "IdAreaCerrojo").Not.Nullable();
            Map(x => x.Año).Nullable();
            Map(x => x.Caracteristicas).Nullable();
            Map(x => x.TipoCombustible, "TipoCombustible").CustomType(typeof(Enums.TipoCombustible)).Nullable();

            //Estados
            HasMany(x => x.Estados).Table("EstadoMovil").KeyColumn("IdMovil")
                .Cascade.All();

            //Valuaciones
            HasMany(x => x.ValuacionesPorMovil).Table("ValuacionPorMovil")
                .KeyColumn("IdMovil");

            //Kilometraje
            HasMany(x => x.KilometrajesPorMovil).Table("KilometrajePorMovil")
                .KeyColumn("IdMovil");

            //Vencimiento ITV
            HasMany(x => x.ITVsPorMovil).Table("ITVPorMovil")
                .KeyColumn("IdMovil");
          
            //Vencimiento TUV
            HasMany(x => x.TUVsPorMovil).Table("TUVPorMovil")
                .KeyColumn("IdMovil");

            //Ordenes de trabajo
            HasMany(x => x.OrdenesTrabajo).Table("MovilPorOrdenTrabajo")
                .KeyColumn("IdMovil")
                .Cascade.All();

            //Flotas
            HasMany(x => x.Flotas).Table("Flota")
                .KeyColumn("IdMovil")
                .Cascade.All();

            References(x => x.FlotaActiva, "IdFlotaActiva").Nullable();
        }
    }
}
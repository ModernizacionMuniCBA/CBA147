using System.Linq;
using Model;
using Model.Entities;

namespace DAO.Maps
{
    class RequerimientoMap : BaseEntityMap<Requerimiento>
    {

        public RequerimientoMap()
        {
            //Tabla
            Table("Requerimiento");

            //Tipo
            References(x => x.Tipo, "IdTipo").Not.Nullable();

            //Motivo
            References(x => x.Motivo, "IdMotivo").Not.Nullable();

            //Persona Fisica
            References(x => x.PersonaFisica, "IdPersonaFisica").Nullable();

            ////Descripcion
            //Map(x => x.Descripcion, "Descripcion")
            //    .CustomType("StringClob")
            //    .CustomSqlType("varchar(MAX)")
            //    .Nullable();

            //Descripciones
            HasMany(x => x.Descripciones).Table("DescripcionPorRequerimiento")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            //Año
            Map(x => x.Año, "Anio").Not.Nullable();

            //Numero
            Map(x => x.Numero, "Numero").Not.Nullable();

            //Estados
            HasMany(x => x.Estados).Table("EstadoRequerimiento").KeyColumn("IdRequerimiento")
                .Cascade.All();

            //Domicilio
            References(x => x.Domicilio, "IdDomicilio")
                .Nullable()
                .Cascade.All();

            //Domicilio manual
            Map(x => x.DomicilioManual, "DomicilioManual").Nullable();

            //Marcado
            Map(x => x.Marcado, "Marcado").Nullable();

            //Prioridad
            Map(x => x.Prioridad, "Prioridad").CustomType(typeof(Enums.PrioridadRequerimiento)).Not.Nullable();

            //Relevamiento Interno
            Map(x => x.RelevamientoInterno, "RelevamientoInterno").Not.Nullable();

            //Mail
            Map(x => x.Mail, "Mail");

            //Ordenes Trabajo
            HasMany(x => x.OrdenesTrabajo).Table("RequerimientoPorOrdenTrabajo")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            //Ordenes Orden Especial
            HasMany(x => x.OrdenesEspeciales).Table("RequerimientoPorOrdenEspecial")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            //Ordenes Inspeccion
            HasMany(x => x.OrdenesInspeccion).Table("RequerimientoPorOrdenInspeccion")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            //Tareas
            HasMany(x => x.Tareas).Table("TareaPorAreaPorRequerimiento")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            //Campos
            HasMany(x => x.CamposDinamicos).Table("CampoPorMotivoPorRequerimiento")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            //User Agent
            Map(x => x.UserAgent, "UserAgent").Not.Nullable();

            //Usuario cerrojo referente
            HasMany(x => x.UsuariosReferentes).Table("UsuarioReferentePorRequerimiento")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            References(x => x.UsuarioCreador, "IdUsuarioCerrojoCreador")
                .Nullable();

            //Referente Provisorio
            References(x => x.ReferenteProvisorio, "IdReferenteProvisorio");

            //TipoDispositivo
            Map(x => x.TipoDispositivo, "TipoDispositivo").CustomType(typeof(Enums.TipoDispositivo)).Not.Nullable();

            //Orden de trabajo activa
            References(x => x.OrdenTrabajoActiva, "IdOrdenTrabajo").Cascade.All().Nullable();

            //Orden de inspeccion activa
            References(x => x.OrdenInspeccionActiva, "IdOrdenInspeccion").Cascade.All().Nullable();

            ////Cpc
            Map(x => x.NumeroCpc, "NumeroCpc").Nullable();

            //Nota
            HasMany(x => x.Notas)
                .Table("NotaPorRequerimiento")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            //Subarea responsable
            References(x => x.AreaResponsable, "IdAreaCerrojoResponsable").Nullable();

            //Archivos
            HasMany(x => x.Archivos)
                .Table("ArchivoPorRequerimiento")
                .KeyColumn("IdRequerimiento")
                .Cascade.All();

            //Origen
            References(x => x.Origen, "IdOrigen").Nullable();
        }
    }
}

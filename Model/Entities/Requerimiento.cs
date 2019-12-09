using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class Requerimiento : BaseEntity
    {
        public virtual TipoRequerimiento Tipo { get; set; }
        public virtual Motivo Motivo { get; set; }
        public virtual PersonaFisica PersonaFisica { get; set; }
        //public virtual string Descripcion { get; set; }
        public virtual IList< DescripcionPorRequerimiento> Descripciones { get; set; }
        public virtual int Año { get; set; }
        public virtual string Numero { get; set; }
        public virtual IList<EstadoRequerimientoHistorial> Estados { get; set; }
        public virtual Domicilio Domicilio { get; set; }
        public virtual string DomicilioManual { get; set; }
        public virtual Enums.PrioridadRequerimiento Prioridad { get; set; }
        public virtual bool RelevamientoInterno { get; set; }
        public virtual string Mail { get; set; }
        public virtual string UserAgent { get; set; }
        public virtual Enums.TipoDispositivo TipoDispositivo { get; set; }
        public virtual IList<RequerimientoPorOrdenTrabajo> OrdenesTrabajo { get; set; }
        public virtual IList<RequerimientoPorOrdenInspeccion> OrdenesInspeccion { get; set; }
        public virtual IList<NotaPorRequerimiento> Notas { get; set; }
        public virtual IList<TareaPorAreaPorRequerimiento> Tareas { get; set; }
        public virtual IList<CampoPorMotivoPorRequerimiento> CamposDinamicos { get; set; }
        public virtual IList<TareaPorAreaPorRequerimiento> getTareas()
        {
            if (Tareas == null || Tareas.Count == 0) return new List<TareaPorAreaPorRequerimiento>();
            return Tareas.Where(x => x.FechaBaja == null).ToList();
        }
        public virtual OrdenTrabajo OrdenTrabajoActiva { get; set; }
        public virtual OrdenAtencionCritica getOrdenAtencionCritica()
        {
            if (OrdenesEspeciales == null || OrdenesEspeciales.Count == 0) return null;
            return OrdenesEspeciales.Where(x => x.FechaBaja == null).ToList().FirstOrDefault().OrdenEspecial;
        }

        public virtual OrdenInspeccion OrdenInspeccionActiva { get; set; }

        public virtual IList<RequerimientoPorOrdenEspecial> OrdenesEspeciales { get; set; }
        public virtual CerrojoArea AreaResponsable { get; set; }
        public virtual IList<ArchivoPorRequerimiento> Archivos { get; set; }
        public virtual ReferenteProvisorio ReferenteProvisorio { get; set; }
        public virtual IList<UsuarioReferentePorRequerimiento> UsuariosReferentes { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioCreador { get; set; }
        public virtual bool Marcado { get; set; }
        public virtual int? NumeroCpc { get; set; }
        public virtual Origen Origen { get; set; }
        public virtual string GetNumero()
        {
            return Numero + " / " + Año;
        }

        //public virtual string getDomicilioString(bool mostrarBarrio, bool mostrarCPC, bool mostrarObservaciones)
        //{
        //    //Ubicacion
        //    string ubicacion = "Sin datos";
        //    if (Domicilio == null)
        //    {
        //        if (!string.IsNullOrEmpty(DomicilioManual))
        //        {
        //            ubicacion = Utils.toTitleCase(DomicilioManual);
        //        }

        //        return ubicacion;
        //    }

        //    return Domicilio.getDomicilioString(mostrarBarrio, mostrarCPC, mostrarObservaciones);
        //}

        //public virtual string getDomicilioString()
        //{
        //    //Ubicacion
        //    string ubicacion = null;
        //    if (Domicilio != null)
        //    {
        //        if (Domicilio.PorBarrio == true)
        //        {
        //            ubicacion = Utils.toTitleCase(Domicilio.Barrio.Nombre)+ ", Obs: " + Domicilio.Observaciones;
        //        }
        //        else
        //        {
        //            ubicacion = Utils.toTitleCase(Domicilio.Calle.Nombre) + " " + Domicilio.Altura + ", Barrio: " + Utils.toTitleCase(Domicilio.Barrio.Nombre) + ", CPC: " + Utils.toTitleCase(Domicilio.Cpc.Nombre);
        //        }
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(DomicilioManual))
        //        {
        //            ubicacion = Utils.toTitleCase(DomicilioManual);
        //        }
        //        else
        //        {
        //            ubicacion = "Sin datos";
        //        }
        //    }
        //    return ubicacion;
        //}

        public virtual EstadoRequerimientoHistorial GetUltimoEstado()
        {
            if (Estados == null || Estados.Count == 0)
            {
                var estado = new EstadoRequerimientoHistorial()
                {
                    Estado = new EstadoRequerimiento()
                    {
                        Nombre = "Sin datos",
                        Color = "FFFFFF",
                        FechaAlta = DateTime.Now,
                        Id = 1,
                        KeyValue = Enums.EstadoRequerimiento.SINDATOS
                    }
                };
                return estado;
            }
            return Estados.Where(x => x.Ultimo == true && x.FechaBaja == null).FirstOrDefault();
        }

        public virtual List<EstadoRequerimientoHistorial> GetEstados()
        {
            if (Estados == null) return null;
            return Estados.Where(x => x.FechaBaja == null).ToList();
        }

        public virtual List<RequerimientoPorOrdenTrabajo> GetOrdenesTrabajo()
        {
            if (OrdenesTrabajo == null) return null;
            return OrdenesTrabajo.Where(x => x.FechaBaja == null).ToList();
        }
    }
}

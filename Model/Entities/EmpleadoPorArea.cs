using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class EmpleadoPorArea : BaseEntity
    {
        public virtual _VecinoVirtualUsuario UsuarioEmpleado { get; set; }
        public virtual CerrojoArea Area { get; set; }
        public virtual IList<FuncionPorEmpleado> FuncionesPorEmpleado { get; set; }
        public virtual IList<FuncionPorEmpleado> GetFunciones()
        {
            if (FuncionesPorEmpleado == null) return null;
            return FuncionesPorEmpleado.Where(x => x.FechaBaja == null).ToList();
        }
        public virtual Seccion Seccion { get; set; }
        public virtual IList<EmpleadoPorFlota> Flotas { get; set; }
        public virtual IList<EmpleadoPorFlota> GetFlotas()
        {
            if (Flotas == null) return null;
            return Flotas.Where(x => x.FechaBaja == null).ToList();
        }

        public virtual Flota FlotaActiva { get; set; }

        public virtual IList<EmpleadoPorOrdenTrabajo> OrdenesTrabajo { get; set; }
        public virtual IList<EmpleadoPorOrdenTrabajo> GetOrdenesTrabajo()
        {
            if (OrdenesTrabajo == null) return null;
            return OrdenesTrabajo.Where(x => x.FechaBaja == null).ToList();
        }

        public virtual EmpleadoPorOrdenTrabajo GetOrdenTrabajoActiva()
        {
            var list = new List<Enums.EstadoOrdenTrabajo>();
            list.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);
            if (OrdenesTrabajo == null) return null;
            return OrdenesTrabajo.Where(x => x.FechaBaja == null && list.Contains(x.OrdenTrabajo.GetUltimoEstado().Estado.KeyValue)).FirstOrDefault();
        }

        public virtual IList<EstadoEmpleadoHistorial> Estados { get; set; }
        public virtual EstadoEmpleado UltimoEstado()
        {
            if (Estados == null || Estados.Count == 0) return null;
            return Estados.Where(x => x.FechaBaja == null).OrderByDescending(x => x.FechaAlta).FirstOrDefault().Estado;
        }

        public EmpleadoPorArea()
            : base()
        {

        }
    }
}

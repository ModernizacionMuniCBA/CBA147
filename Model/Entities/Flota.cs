using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Flota : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual Movil Movil { get; set; }
        public virtual CerrojoArea Area { get; set; }
        public virtual IList<EmpleadoPorFlota> Empleados { get; set; }
        public virtual List<EmpleadoPorFlota> GetEmpleados()
        {
            if (Empleados == null) return null;
            return Empleados.Where(x => x.FechaBaja == null).ToList();
        }
        public virtual IList<EstadoFlotaHistorial> Estados { get; set; }
        public virtual EstadoFlota GetUltimoEstado()
        {
            if (Estados == null || Estados.Count == 0) return null;
            return Estados.Where(x => x.FechaBaja == null).OrderByDescending(x => x.FechaAlta).FirstOrDefault().Estado;
        }
        public virtual IList<FlotaPorOrdenTrabajo> OrdenesTrabajo { get; set; }
        public virtual IList<FlotaPorOrdenTrabajo> GetOrdenesTrabajo()
        {
            if (OrdenesTrabajo == null) return null;
            return OrdenesTrabajo.Where(x => x.FechaBaja == null).ToList();
        }

        public virtual FlotaPorOrdenTrabajo GetOrdenTrabajoActiva()
        {
            var list = new List<Enums.EstadoOrdenTrabajo>();
            list.Add(Enums.EstadoOrdenTrabajo.ENPROCESO);
            if (OrdenesTrabajo == null) return null;
            return OrdenesTrabajo.Where(x => x.FechaBaja == null && list.Contains(x.OrdenTrabajo.GetUltimoEstado().Estado.KeyValue)).FirstOrDefault();
        }


    }
}

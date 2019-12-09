using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class OrdenTrabajo : BaseEntity
    {
        public virtual string Numero { get; set; }

        public virtual int Año { get; set; }

        public virtual string Descripcion { get; set; }

        public virtual CerrojoArea Area { get; set; }
        public virtual CerrojoAmbito Ambito { get; set; }

        public virtual IList<EstadoOrdenTrabajoHistorial> Estados { get; set; }

        public virtual IList<RequerimientoPorOrdenTrabajo> RequerimientosPorOrdenTrabajo { get; set; }
        public virtual IList<RequerimientoPorOrdenTrabajo> RequerimientosPorOrdenTrabajoActivos()
        {
            return RequerimientosPorOrdenTrabajo.Where(x => x.FechaBaja == null).ToList();
        }
        public virtual IList<EmpleadoPorOrdenTrabajo> EmpleadosPorOrdenTrabajo { get; set; }
        public virtual IList<EmpleadoPorOrdenTrabajo> EmpleadosPorOrdenTrabajoActivos()
        {
            return EmpleadosPorOrdenTrabajo.Where(x => x.FechaBaja == null).ToList();
        }
        public virtual IList<FlotaPorOrdenTrabajo> FlotasPorOrdenTrabajo { get; set; }
        public virtual IList<FlotaPorOrdenTrabajo> FlotasPorOrdenTrabajoActivos()
        {
            return FlotasPorOrdenTrabajo.Where(x => x.FechaBaja == null).ToList();
        }

        public virtual IList<NotaPorOrdenTrabajo> Notas { get; set; }

        public virtual string UserAgent { get; set; }

        public virtual Enums.TipoDispositivo TipoDispositivo { get; set; }

        public virtual Seccion Seccion { get; set; }

        public virtual List<RecursoPorOrdenTrabajo> Recursos { get; set; }

        public virtual Zona Zona { get; set; }
        public virtual IList<MovilPorOrdenTrabajo> MovilesPorOrdenTrabajo
        {
            get;
            set;
        }

        public virtual IList<MovilPorOrdenTrabajo> MovilesPorOrdenTrabajoActivos()
        {
            return MovilesPorOrdenTrabajo.Where(x => x.FechaBaja == null).ToList();

        }

        public virtual _VecinoVirtualUsuario UsuarioCreador { get; set; }
        public virtual DateTime? FechaCreacion { get; set; }

        public virtual string GetNumero()
        {
            return Numero + " / " + Año;
        }

        public virtual EstadoOrdenTrabajoHistorial GetUltimoEstado()
        {
            if (Estados == null) return null;
            return Estados.Where(x => x.Ultimo == true && x.FechaBaja == null).FirstOrDefault();
        }

        public virtual List<EstadoOrdenTrabajoHistorial> GetEstados()
        {
            if (Estados == null) return null;
            return Estados.Where(x => x.FechaBaja == null).ToList();
        }

        public virtual List<Requerimiento> Requerimientos()
        {
            var list = new List<Requerimiento>();
            RequerimientosPorOrdenTrabajo.Where(x => x.FechaBaja == null).ToList().ForEach(x => list.Add(x.Requerimiento));
            return list;
        }

        public virtual RequerimientoPorOrdenTrabajo GetRequerimientoPorOrdenTrabajo(int idRq)
        {
            if (RequerimientosPorOrdenTrabajo == null || RequerimientosPorOrdenTrabajo.Count == 0) return null;
            foreach (var requerimientoOT in RequerimientosPorOrdenTrabajo)
            {
                if (requerimientoOT.Requerimiento.Id == idRq)
                {
                    return requerimientoOT;
                }
            }
            return null;
        }

        public virtual bool ContieneRequerimiento(int idRq)
        {
            if (Requerimientos() == null || Requerimientos().Count == 0) return false;
            foreach (var requerimientoOT in Requerimientos())
            {
                if (requerimientoOT.Id == idRq)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

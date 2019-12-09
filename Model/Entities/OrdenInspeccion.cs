using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class OrdenInspeccion : BaseEntity
    {
        public virtual string Numero { get; set; }
        public virtual bool Marcado { get; set; }

        public virtual int Año { get; set; }

        public virtual string Descripcion { get; set; }

        public virtual IList<EstadoOrdenInspeccionHistorial> Estados { get; set; }

        public virtual IList<RequerimientoPorOrdenInspeccion> RequerimientosPorOrdenInspeccion { get; set; }
        public virtual IList<RequerimientoPorOrdenInspeccion> RequerimientosPorOrdenInspeccionActivos()
        {
            return RequerimientosPorOrdenInspeccion.Where(x => x.FechaBaja == null).ToList();
        }

        //public virtual IList<NotaPorOrdenTrabajo> Notas { get; set; }

        public virtual string UserAgent { get; set; }

        public virtual Enums.TipoDispositivo TipoDispositivo { get; set; }

        public virtual _VecinoVirtualUsuario UsuarioCreador { get; set; }
        public virtual DateTime? FechaCreacion { get; set; }

        public virtual string GetNumero()
        {
            return Numero + " / " + Año;
        }

        public virtual EstadoOrdenInspeccionHistorial GetUltimoEstado()
        {
            if (Estados == null) return null;
            return Estados.Where(x => x.Ultimo == true && x.FechaBaja == null).FirstOrDefault();
        }

        public virtual List<EstadoOrdenInspeccionHistorial> GetEstados()
        {
            if (Estados == null) return null;
            return Estados.Where(x => x.FechaBaja == null).ToList();
        }

        public virtual List<Requerimiento> Requerimientos()
        {
            var list = new List<Requerimiento>();
            RequerimientosPorOrdenInspeccion.Where(x => x.FechaBaja == null).ToList().ForEach(x => list.Add(x.Requerimiento));
            return list;
        }

        public virtual RequerimientoPorOrdenInspeccion GetRequerimientoPorOrdenInspeccion(int idRq)
        {
            if (RequerimientosPorOrdenInspeccion == null || RequerimientosPorOrdenInspeccion.Count == 0) return null;
            foreach (var requerimientoOI in RequerimientosPorOrdenInspeccion)
            {
                if (requerimientoOI.Requerimiento.Id == idRq)
                {
                    return requerimientoOI;
                }
            }
            return null;
        }

        public virtual bool ContieneRequerimiento(int idRq)
        {
            if (Requerimientos() == null || Requerimientos().Count == 0) return false;
            foreach (var requerimientoOI in Requerimientos())
            {
                if (requerimientoOI.Id == idRq)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

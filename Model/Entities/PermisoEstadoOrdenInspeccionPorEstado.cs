using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class PermisoEstadoOrdenInspeccionPorEstado : BaseEntity
    {
        public virtual EstadoOrdenInspeccion  EstadoOrdenInspeccion { get; set; }
        public virtual PermisoEstadoOrdenInspeccion Permiso { get; set; }

        public PermisoEstadoOrdenInspeccionPorEstado()
        {

        }
    }
}

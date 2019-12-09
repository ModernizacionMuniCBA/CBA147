using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class PermisoEstadoOrdenTrabajoPorEstado : BaseEntity
    {
        public virtual EstadoOrdenTrabajo EstadoOrdenTrabajo { get; set; }
        public virtual PermisoEstadoOrdenTrabajo Permiso { get; set; }

        public PermisoEstadoOrdenTrabajoPorEstado()
        {

        }
    }
}

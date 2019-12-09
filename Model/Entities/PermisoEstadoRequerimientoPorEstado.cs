using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class PermisoEstadoRequerimientoPorEstado : BaseEntity
    {
        public virtual EstadoRequerimiento EstadoRequerimiento { get; set; }
        public virtual PermisoEstadoRequerimiento Permiso { get; set; }

        public PermisoEstadoRequerimientoPorEstado()
        {

        }
    }
}

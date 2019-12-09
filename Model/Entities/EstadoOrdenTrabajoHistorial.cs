using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class EstadoOrdenTrabajoHistorial : BaseEntity
    {
        public virtual OrdenTrabajo OrdenTrabajo { get; set; }
        public virtual EstadoOrdenTrabajo Estado { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual Boolean Ultimo { get; set; }


    }
}

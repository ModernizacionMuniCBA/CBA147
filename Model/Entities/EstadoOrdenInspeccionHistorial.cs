using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class EstadoOrdenInspeccionHistorial : BaseEntity
    {
        public virtual OrdenInspeccion OrdenInspeccion { get; set; }
        public virtual EstadoOrdenInspeccion Estado { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual Boolean Ultimo { get; set; }


    }
}

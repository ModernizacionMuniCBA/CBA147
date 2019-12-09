using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class EstadoRequerimientoHistorial : BaseEntity
    {
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual EstadoRequerimiento Estado { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual Boolean Ultimo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class ReparacionPorMovil : BaseEntity
    {
        public virtual DateTime FechaReparacion { get; set; }
        public virtual int MontoReparacion { get; set; }
        public virtual Movil Movil { get; set; }
        public virtual string Motivo { get; set; }
        public virtual string Taller { get; set; }

        public ReparacionPorMovil()
        {

        }

    }
}

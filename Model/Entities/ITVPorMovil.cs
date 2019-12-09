using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class ITVPorMovil : BaseEntity
    {
        public virtual Movil Movil { get; set; }
        public virtual DateTime? FechaUltimoITV { get; set; }
        public virtual DateTime FechaVencimientoITV { get; set; }

        public ITVPorMovil()
        {

        }

    }
}

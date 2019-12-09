using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class TUVPorMovil : BaseEntity
    {
        public virtual Movil Movil { get; set; }
        public virtual DateTime? FechaUltimoTUV  { get; set; }
        public virtual DateTime FechaVencimientoTUV  { get; set; }

        public TUVPorMovil()
        {

        }

    }
}

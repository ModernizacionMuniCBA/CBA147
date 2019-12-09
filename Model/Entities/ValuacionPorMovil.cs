using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class ValuacionPorMovil : BaseEntity
    {
        public virtual DateTime FechaValuacion { get; set; }
        public virtual int Valor { get; set; }
        public virtual Movil Movil { get; set; }

        public ValuacionPorMovil()
        {

        }

    }
}

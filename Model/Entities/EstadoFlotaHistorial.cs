using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class EstadoFlotaHistorial : BaseEntity
    {
        public virtual Flota Flota{ get; set; }
        public virtual EstadoFlota Estado { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual bool Ultimo { get; set; }

        public EstadoFlotaHistorial()
        {

        }

    }
}

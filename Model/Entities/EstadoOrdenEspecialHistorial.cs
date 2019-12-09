using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class EstadoOrdenEspecialHistorial : BaseEntity
    {
        public virtual OrdenAtencionCritica OrdenEspecial { get; set; }
        public virtual EstadoOrdenEspecial Estado { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual bool Ultimo{ get; set; }

        public EstadoOrdenEspecialHistorial()
        {

        }

    }
}

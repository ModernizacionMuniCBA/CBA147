using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class KilometrajePorMovil : BaseEntity
    {
        public virtual Movil Movil { get; set; }
        public virtual DateTime FechaKilometraje { get; set; }
        public virtual int Kilometraje { get; set; }

        public KilometrajePorMovil()
        {

        }

    }
}

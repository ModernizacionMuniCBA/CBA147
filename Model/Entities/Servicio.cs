using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Servicio : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string Icono { get; set; }
        public virtual string UrlIcono { get; set; }
        public virtual string Color { get; set; }
        public virtual bool Principal { get; set; }

        public virtual IList<Tema> Temas { get; set; }

    }
}

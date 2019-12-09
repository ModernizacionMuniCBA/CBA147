using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Tema : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual Servicio Servicio { get; set; }
        public virtual IList<Motivo> Motivos { get; set; }



    }
}
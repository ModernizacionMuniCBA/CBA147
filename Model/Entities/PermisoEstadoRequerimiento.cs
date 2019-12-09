using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class PermisoEstadoRequerimiento : BaseEntity
    {
        public virtual Enums.PermisoEstadoRequerimiento KeyValue { get; set; }
        public virtual string Nombre { get; set; }
        public virtual int Posicion { get; set; }

        public PermisoEstadoRequerimiento()
        {

        }
    }
}

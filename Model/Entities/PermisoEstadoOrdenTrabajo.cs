using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class PermisoEstadoOrdenTrabajo : BaseEntity
    {
        public virtual Enums.PermisoEstadoOrdenTrabajo KeyValue { get; set; }
        public virtual string Nombre { get; set; }
        public virtual int Posicion { get; set; }

        public PermisoEstadoOrdenTrabajo()
        {

        }
    }
}

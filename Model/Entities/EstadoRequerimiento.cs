using System;
using System.Linq;

namespace Model.Entities
{
    public class EstadoRequerimiento : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual Enums.EstadoRequerimiento KeyValue { get; set; }
        public virtual Enums.EstadoRequerimiento? KeyValuePublico { get; set; }
        public virtual String Color { get; set; }
        public virtual int Orden { get; set; }


    }
}

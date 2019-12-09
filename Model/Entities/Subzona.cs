using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class Subzona : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual Zona Zona { get; set; }

    }
}

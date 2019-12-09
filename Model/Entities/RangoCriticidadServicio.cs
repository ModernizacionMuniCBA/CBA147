using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class RangoCriticidadServicio : BaseEntity
    {
        public virtual int IdServicio { get; set; }
        public virtual int Desde { get; set; }
        public virtual string Color { get; set; }


    }
}

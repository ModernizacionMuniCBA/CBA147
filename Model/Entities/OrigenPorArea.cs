﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class OrigenPorArea : BaseEntity
    {
        public virtual Origen Origen{ get; set; }
        public virtual CerrojoArea Area { get; set; }

    }
}

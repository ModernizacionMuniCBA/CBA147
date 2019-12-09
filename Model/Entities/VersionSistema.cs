using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class VersionSistema : BaseEntity
    {
        public virtual string Version { get; set; }

        public VersionSistema()
        {

        }
    }
}

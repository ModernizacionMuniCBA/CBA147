using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class TipoDocumento : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual Enums.TipoDocumento KeyValue { get; set; }

    }
}

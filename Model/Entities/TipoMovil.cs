using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class TipoMovil : BaseEntity
    {
        public virtual string Nombre { get; set; }

    }
}

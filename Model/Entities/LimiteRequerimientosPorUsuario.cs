using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class LimiteRequerimientosPorUsuario : BaseEntity
    {
        public virtual int Contador { get; set; }
        public virtual int IdUsuarioCreador { get; set; }
        public virtual DateTime Fecha { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class MensajePorRequerimiento : BaseEntity
    {
        public virtual string EmailReceptor { get; set; }
        public virtual string Texto { get; set; }
        public virtual Requerimiento RequerimientoAsociado { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioEmisor { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioReceptor { get; set; }
        public virtual bool Enviado { get; set; }
    }
}

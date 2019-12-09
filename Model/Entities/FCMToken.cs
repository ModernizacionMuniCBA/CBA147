using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class FCMToken : BaseEntity
    {
        public virtual _VecinoVirtualUsuario UsuarioToken { get; set; }

    }
}

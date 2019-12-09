using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class InformacionOrganica : BaseEntity
    {
        public virtual InformacionOrganicaDireccion Direccion { get; set; }
        public virtual CerrojoArea Area { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioCreador { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class InformacionOrganicaDireccion : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string Responsable { get; set; }
        public virtual string Domicilio { get; set; }
        public virtual string Telefono { get; set; }
        public virtual string Email { get; set; }
        public virtual InformacionOrganicaSecretaria Secretaria { get; set; }

        public virtual _VecinoVirtualUsuario UsuarioCreador { get; set; }
    }
}

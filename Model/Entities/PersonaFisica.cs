using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class PersonaFisica : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual string NroDoc { get; set; }
        public virtual string Cuil { get; set; }
        public virtual DateTime? FechaNacimiento { get; set; }
        public virtual Enums.Sexo Sexo { get; set; }
        public virtual string Mail { get; set; }
        public virtual string Telefono { get; set; }
        public virtual Domicilio Domicilio { get; set; }
        public virtual string DomicilioManual { get; set;
        }

    }
}

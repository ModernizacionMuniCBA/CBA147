using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Entities
{
    public class ArchivoPorRequerimiento : BaseEntity
    {
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual string Nombre { get; set; }
        public virtual Enums.TipoArchivo Tipo { get; set; }
        public virtual string Identificador { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual int ContentLength { get; set; }
        public virtual string ContentType { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioReferente { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class NotificacionSistema : BaseEntity
    {
        public virtual string Titulo { get; set; }
        public virtual string Contenido { get; set; }
        public virtual bool Notificar { get; set; }
        public NotificacionSistema()
            : base()
        {

        }

        public NotificacionSistema(Comandos.Comando_NotificacionParaUsuario comando)
            : base()
        {
            this.Titulo = comando.Titulo;
            this.Contenido = comando.Contenido;
            this.Notificar = comando.Notificar;
        }
    }
}

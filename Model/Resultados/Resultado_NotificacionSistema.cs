using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_NotificacionSistema : Resultado_Base<NotificacionSistema>
    {
        public virtual string Titulo { get; set; }
        public virtual string Contenido { get; set; }
        public virtual bool Notificar { get; set; }

        public Resultado_NotificacionSistema()
            : base()
        {

        }

        public Resultado_NotificacionSistema(NotificacionSistema entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Titulo = entity.Titulo;
            Contenido = entity.Contenido;
            Notificar = entity.Notificar;
        }

        public static List<Resultado_NotificacionSistema> ToList(List<NotificacionSistema> list)
        {
            return list.Select(x => new Resultado_NotificacionSistema(x)).ToList();
        }
    }
}

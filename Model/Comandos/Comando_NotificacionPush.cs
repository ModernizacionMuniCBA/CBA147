using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_NotificacionPush
    {
        public int IdUsuario { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public Comando_NotificacionPush()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_RequerimientoComentario
    {
        public string Comentario { get; set; }
        public int IdRequerimiento { get; set; }
        public int? IdOrdenTrabajo { get; set; }

        public Comando_RequerimientoComentario()
        {

        }
    }
}

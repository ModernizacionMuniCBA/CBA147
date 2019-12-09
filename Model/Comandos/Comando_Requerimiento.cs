using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_Requerimiento
    {
        public int IdMotivo { get; set; }
        public string Descripcion { get; set; }
        public Comando_Domicilio Domicilio { get; set; }
        public string Imagen { get; set; }

        public string OrigenAlias { get; set; }
        public string OrigenKey { get; set; }

        public Comando_Requerimiento()
        {

        }
    }
}

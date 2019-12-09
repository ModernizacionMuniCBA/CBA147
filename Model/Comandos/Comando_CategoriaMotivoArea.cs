using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_CategoriaMotivoArea
    {
        public int? Id { get; set; }
        public int IdArea { get; set; }
        public string Nombre { get; set; }
        public Comando_CategoriaMotivoArea()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Secretaria
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Responsable { get; set; }
        public string Domicilio { get; set; }

        public Comando_Secretaria()
        {

        }
    }
}

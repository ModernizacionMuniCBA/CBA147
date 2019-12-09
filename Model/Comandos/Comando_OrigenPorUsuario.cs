using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_OrigenPorUsuario
    {
        public int? Id { get; set; }
        public int UsuarioId { get; set; }
        public int OrigenId { get; set; }

        public Comando_OrigenPorUsuario()
        {

        }
    }
}

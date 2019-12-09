using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Empleado
    {
        public int? Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdArea { get; set; }
        public int? IdSeccion { get; set; }
        public List<int> IdFunciones { get; set; }
        public bool? DadosDeBaja { get; set; }

        public Comando_Empleado()
        {
            DadosDeBaja = false;
            IdFunciones=new List<int>();
        }
    }
}

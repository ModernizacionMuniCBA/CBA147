using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_OrdenAtencionCritica
    {
        public int? Id{get;set;}
        public string Descripcion { get; set; }
        
        public int IdRequerimiento { get; set; }
        public Comando_OrdenAtencionCritica()
        {

        }
    }
}

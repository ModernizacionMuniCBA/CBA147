using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_EdificioMunicipal
    {
        public int? Id { get; set; }
        public int IdCategoria{ get; set; }
        public string Nombre {get; set; }
        public Comando_Domicilio Domicilio { get; set; }
        public bool? DadosDeBaja { get; set; }

        public Comando_EdificioMunicipal()
        {
            DadosDeBaja = false;
        }
    }
}

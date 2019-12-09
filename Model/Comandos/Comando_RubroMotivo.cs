using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_RubroMotivo
    {
        public int? Id { get; set; }
        public int IdGrupo{get;set;}
        public string Nombre { get; set; }
        public List<int> IdsMotivos{get;set;}
        public string Observaciones { get; set; }
        public Comando_RubroMotivo()
        {

        }
    }

}

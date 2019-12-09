using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_VersionSistema
    {

        public string Version { get; set; }
        public string Descripcion { get; set; }

        public Comando_VersionSistema()
        {

        }
    }
}

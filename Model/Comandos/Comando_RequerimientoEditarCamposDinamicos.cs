using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_RequerimientoEditarCamposDinamicos
    {
        public int IdRequerimiento { get; set; }
        public List<Model.Comandos.Comando_RequerimientoIntranet.Comando_CampoDinamico> CamposDinamicos { get; set; }

        public Comando_RequerimientoEditarCamposDinamicos()
        {

        }


    }
}

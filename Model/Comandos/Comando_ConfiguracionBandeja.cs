using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_ConfiguracionBandeja
    {
        public IList<Comando_ConfiguracionBandejaPorArea> ConfiguracionesBandejas { get; set; }

        public Comando_ConfiguracionBandeja()
        {

        }

            [Serializable]
        public class Comando_ConfiguracionBandejaPorArea
        {
            public int IdArea { get; set; }
            public int IdTipoMotivo { get; set; }
            public bool PorDefecto{ get; set; }

            public Comando_ConfiguracionBandejaPorArea()
            {

            }
        }
    }
}

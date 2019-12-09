using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.WSVecinoVirtual.Resultados
{
    [Serializable]
    public class VVResultado_Menu
    {
        public int IdCerrojo { get; set; }

        public int IdObjetoCerrojo { get; set; }
        public string Titulo { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Codigo { get; set; }

        public int Posicion { get; set; }
        public int? IdPadre { get; set; }
        public List<VVResultado_Menu> Hijos { get; set; }

    }
}

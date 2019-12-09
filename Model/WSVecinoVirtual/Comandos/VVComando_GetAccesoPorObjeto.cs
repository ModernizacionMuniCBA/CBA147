using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
        [Serializable]
    public class VVComando_GetAccesoPorObjeto
    {
        public int? IdUser { get; set; }
        public int? IdArea { get; set; }
        public int IdRol { get; set; }
        public int IdObjeto { get; set; }
    }
}
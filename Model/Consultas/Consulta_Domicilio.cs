using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Domicilio
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Consulta { get; set; }
        public bool PorLatitud { get { return Lat.HasValue && Lng.HasValue; } }

        public Consulta_Domicilio()
        {

        }
    }
}

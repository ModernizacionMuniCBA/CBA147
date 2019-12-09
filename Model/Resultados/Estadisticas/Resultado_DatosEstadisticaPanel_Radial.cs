using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados.Estadisticas
{
    [Serializable]
    public class Resultado_DatosEstadisticaPanel_Radial
    {
        public string Servicio{ get; set; }
        public double Dias { get; set; }

        public Resultado_DatosEstadisticaPanel_Radial() 
        {

        }
    }
}

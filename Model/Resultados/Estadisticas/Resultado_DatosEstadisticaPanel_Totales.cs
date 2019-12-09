using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados.Estadisticas
{
    [Serializable]
    public class Resultado_DatosEstadisticaPanel_Totales 
    {
        public int Total{ get; set; }
        public double Porcentaje { get; set; }

        public Resultado_DatosEstadisticaPanel_Totales() 
        {

        }
    }
}

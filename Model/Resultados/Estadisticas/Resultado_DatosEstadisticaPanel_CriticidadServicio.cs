using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados.Estadisticas
{
        [Serializable]
    public class Resultado_DatosEstadisticaPanel_CriticidadServicio
    {
        public int IdArea { get; set; }
        public string Area { get; set; }
        public int Total { get; set; }
        public int Atendidos { get; set; }
        public float Porcentaje { get; set; }
        public string Color { get; set; }

        public Resultado_DatosEstadisticaPanel_CriticidadServicio()
        {

        }
    }
}

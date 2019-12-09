using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados.Estadisticas
{
        [Serializable]
    public class Resultado_DatosEstadisticaPanel
    {
        public List<Resultado_DatosEstadisticaPanel_Motivos> Ranking_Motivos { get; set; }
        public Resultado_DatosEstadisticaPanel_Totales Totales { get; set; }
        public List<Resultado_DatosEstadisticaPanel_Radial> Radial { get; set; }
        public int PromedioAtencion { get; set; }
        public List<Resultado_DatosEstadisticaPanel_CriticidadServicio> CriticidadServicios { get; set; }
        public string UrlMapa { get; set; }
        public List<Resultado_DatosEstadisticaPanel_Cpc> ArrayDatosMapa { get; set; }
     
        public Resultado_DatosEstadisticaPanel()
        {

        }


    }
}

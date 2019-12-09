using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados.Estadisticas
{
    [Serializable]
    public class Resultado_DatosEstadisticaPanel_Cpc
    {
        public virtual List<int> IdsRequerimientos { get; set; }
        public virtual int CantidadRequerimientos { get; set; }
        public virtual double Porcentaje { get; set; }

        public virtual Resultado_Cpc Cpc { get; set; }

        public virtual string Color { get; set; }
        public virtual string Criticidad { get; set; }
        public Resultado_DatosEstadisticaPanel_Cpc()
        {

        }

        public Resultado_DatosEstadisticaPanel_Cpc(Resultado_Cpc entity, List<int> idsRequerimientos, int cantidadRequerimientos, double porcentaje, string color, string criticidad)
        {
            Cpc = entity;
            IdsRequerimientos = idsRequerimientos;
            CantidadRequerimientos = cantidadRequerimientos;
            Porcentaje = porcentaje;
            Color = color;
            Criticidad = criticidad;
        }
    }
}

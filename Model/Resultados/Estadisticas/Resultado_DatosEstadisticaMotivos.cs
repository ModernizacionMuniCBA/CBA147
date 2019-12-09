using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados.Estadisticas
{
    [Serializable]
    public class Resultado_DatosEstadisticaMotivos
    {
        public virtual string Motivo { get; set; }
        public virtual int Cantidad { get; set; }
        public virtual double Porcentaje { get; set; }
        public virtual int IdMotivo { get; set; }
        public virtual List<int> IdsRequerimientos { get; set; }
        
        public Resultado_DatosEstadisticaMotivos() 
        {

        }
    }
}

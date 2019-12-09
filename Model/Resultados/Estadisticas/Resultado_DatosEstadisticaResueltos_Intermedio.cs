using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados.Estadisticas
{
    [Serializable]
    public class Resultado_DatosEstadisticaResueltos_Intermedio
    {
        public virtual int Año{ get; set; }
        public virtual  int Mes { get; set; }        
        public virtual int IdRequerimiento { get; set; }
  
        public Resultado_DatosEstadisticaResueltos_Intermedio() 
        {

        }
    }
}

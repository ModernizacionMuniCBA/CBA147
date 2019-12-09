using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenInspeccionInit
    {
        public List<Resultado_Requerimiento> Requerimientos { get; set; }

        public Resultado_OrdenInspeccionInit()
        {

        }
    }
}

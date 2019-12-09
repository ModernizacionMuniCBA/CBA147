using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenTrabajoInit
    {
        public List<Resultado_Requerimiento> Requerimientos { get; set; }
        public Resultado_Area Area { get; set; }

        public List<Resultado_Seccion> Secciones { get; set; }

        public int CantidadMoviles { get; set; }

        public Resultado_OrdenTrabajoInit()
        {

        }
    }
}

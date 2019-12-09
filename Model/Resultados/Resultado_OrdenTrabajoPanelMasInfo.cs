using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenTrabajoPanelMasInfo
    {
        public IList<Resultado_OrdenTrabajoDetalle_Movil> Moviles { get; set; }
        public  IList<Resultado_OrdenTrabajoDetalle_Zona> Zonas { get; set; }
        public int CantidadRequerimientos { get; set; }

        public Resultado_OrdenTrabajoPanelMasInfo() { 
        Moviles=new  List<Resultado_OrdenTrabajoDetalle_Movil>();
            Zonas=new List<Resultado_OrdenTrabajoDetalle_Zona>();
        }

    }

}

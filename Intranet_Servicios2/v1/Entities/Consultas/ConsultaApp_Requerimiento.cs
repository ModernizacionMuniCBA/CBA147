
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet_Servicios2.v1.Entities.Consultas
{
    public class ConsultaApp_Requerimiento
    {
        public List<int> IdsMotivo { get; set; }
        public List<int> IdsArea { get; set; }
        public List<int> IdsServicio { get; set; }
        public List<Enums.EstadoRequerimiento> KeyValuesEstado { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }

        public ConsultaApp_Requerimiento()
        {

        }
    }

}
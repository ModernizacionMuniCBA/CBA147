using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_RequerimientoExportar
    {
        public DateTime FechaAlta { get; set; }
        public string Numero { get; set; }

        public int Año { get; set; }
        public string MotivoNombre { get; set; }
        public string Descripcion { get; set; }   
        public string DomicilioObservaciones { get; set; }
        public string DomicilioDireccion { get; set; }
        public string BarrioNombre { get; set; }
        public ResultadoTabla_RequerimientoExportar():base() { }

        public ResultadoTabla_RequerimientoExportar(Requerimiento requerimiento)
        {
            
        }

    }
}

using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_OrdenAtencionCritica
    {
        private Requerimiento requerimiento;

        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }

        
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }

        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }

         public int IdRequerimiento{ get; set; }
         public string RequerimientoNumero { get; set; }

        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public int EstadoKeyValue { get; set; }
       
        public string Descripcion { get; set; }
 
        public ResultadoTabla_OrdenAtencionCritica():base() { }

        public ResultadoTabla_OrdenAtencionCritica(Requerimiento requerimiento)
        {
            
        }

    }
}

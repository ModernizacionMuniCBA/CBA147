using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_OrdenInspeccion
    {

        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Numero { get; set; }
       
        public string Descripcion{ get; set; }
   
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public Enums.EstadoOrdenInspeccion EstadoKeyValue { get; set; }
        public int Dias { get; set; }

        public ResultadoTabla_OrdenInspeccion() : base() { }



    }
}

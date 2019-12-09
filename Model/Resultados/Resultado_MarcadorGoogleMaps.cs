using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    public class Resultado_MarcadorGoogleMaps
    {

        public int Id { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public int BarrioId { get; set; }
        public string BarrioNombre { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public int EstadoKeyValue { get; set; }
        public int DomicilioId { get; set; }
        public int AreaId { get; set; }
        public string AreaNombre { get; set; }
    }
}

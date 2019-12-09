using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    [Serializable]
    public class Consulta_RequerimientoCercano
    {
        public int? IdMotivo { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int Distancia { get; set; }
        public List<Enums.EstadoRequerimiento> EstadosKeyValue { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? DadosDeBaja { get; set; }

        public bool? Default { get; set; }

        public Consulta_RequerimientoCercano()
        {
            DadosDeBaja = false;
        }


    }
}

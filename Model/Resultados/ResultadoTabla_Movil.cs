using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_Movil
    {
        public int Id { get; set; }
        public string NumeroInterno { get; set; }
        public int? TipoMovilId { get; set; }
        public string TipoMovilNombre { get; set; }
        public string Dominio { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? VencimientoITV { get; set; }
        public DateTime? VencimientoTUV { get; set; }
        public int? DiasITV { get; set; }
        public int? DiasTUV { get; set; }
        public string Observaciones { get; set; }
        public string NombreEstado { get; set; }
        public int IdEstado { get; set; }
        public ResultadoTabla_Movil() : base() { }

    }
}

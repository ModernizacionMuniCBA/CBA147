using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Flota
    {
        public int? IdArea { get; set; }
        public bool? DadosDeBaja { get; set; }
        public int? IdOT { get; set; }
        public int? IdMovil { get; set; }
        public bool? Hoy { get; set; }
        public List<Enums.EstadoFlota> Estados { get; set; }

        public Consulta_Flota()
        {
            DadosDeBaja = false;
        }
    }
}

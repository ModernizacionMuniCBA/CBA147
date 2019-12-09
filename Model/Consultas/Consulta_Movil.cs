using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Movil
    {
        public string Filtro { get; set; }
        public int? IdArea { get; set; }
        public int? IdOT { get; set; }
        public int? IdTipo{ get; set; }
        public List<Enums.EstadoMovil> Estados { get; set; }
        public bool? DadosDeBaja { get; set; }
        public bool? Flota { get; set; }

        public Consulta_Movil()
        {
            DadosDeBaja = null;
        }
    }
}

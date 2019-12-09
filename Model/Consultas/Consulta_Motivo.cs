using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Motivo
    {
        public int? IdArea { get; set; }
        public int? IdServicio { get; set; }
        public List<Enums.TipoMotivo> Tipos { get; set; }
        public bool? Urgente { get; set; }
        public bool? DadosDeBaja { get; set; }


                public Consulta_Motivo()
        {
            DadosDeBaja = false;
        }
    }
}

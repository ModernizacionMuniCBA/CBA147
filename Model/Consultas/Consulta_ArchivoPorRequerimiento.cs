using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_ArchivoPorRequerimiento
    {
        public int? IdRequerimiento { get; set; }
        public Enums.TipoArchivo? Tipo { get; set; }
        public bool? DadosDeBaja { get; set; }
        public Consulta_ArchivoPorRequerimiento()
        {
            DadosDeBaja = false;
        }
    }
}

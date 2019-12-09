using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_RequerimientoFavoritoPorUsuario
    {
        public int? IdUser { get; set; }
        public int? IdRequerimiento { get; set; }

        public bool? DadosDeBaja { get; set; }

        public Consulta_RequerimientoFavoritoPorUsuario()
        {

        }
    }
}

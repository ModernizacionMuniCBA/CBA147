using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_Zona
    {

        public int Id { get; set; }
        public int AreaId { get; set; }
        public string AreaNombre { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaBaja { get; set; }

        public ResultadoTabla_Zona() : base() { }


    }
}

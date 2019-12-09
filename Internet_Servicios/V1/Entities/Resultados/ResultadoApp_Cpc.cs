using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Resultados
{
    public class ResultadoApp_Cpc
    {
        public int Id { get; set; }
        public int IdCatastro { get; set; }
        public int Numero { get; set; }
        public string Nombre { get; set; }
    }
}
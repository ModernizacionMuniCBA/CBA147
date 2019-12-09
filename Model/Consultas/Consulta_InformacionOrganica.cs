using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_InformacionOrganica
    {
        public int? Id { get; set; }
        public int? IdDireccion {get;set;}
        public int? IdSecretaria{get;set;}
        public int? IdArea { get; set; }
        public bool? DadosDeBaja { get; set; }

        public Consulta_InformacionOrganica()
        {
            DadosDeBaja = false;
        }
    }
}

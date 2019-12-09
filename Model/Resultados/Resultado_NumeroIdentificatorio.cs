using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_NumeroIdentificatorio {
        public string Numero { get; set; }
        public int Año { get; set; }

        public Resultado_NumeroIdentificatorio(string numero, int año){
            this.Numero=numero;
            this.Año=año;
        }
    }
}

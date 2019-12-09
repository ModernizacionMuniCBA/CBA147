using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_BarrioPorZona
    {
        public List<Resultado_Barrio> Barrios{get;set;}
        public int IdArea{get;set;}
        public int IdZona{get;set;}
        public int? IdSubZona{get;set;}

        public Comando_BarrioPorZona()
        {

        }
    }
}

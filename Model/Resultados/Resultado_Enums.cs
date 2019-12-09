using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Enums
    {
        public virtual string Nombre { get; set; }
        public virtual int KeyValue { get; set; }
        
        public Resultado_Enums()
            : base()
        {

        }

        public Resultado_Enums(int k, string n)
        {

            KeyValue = k;
            Nombre = n;
        }

    }
}

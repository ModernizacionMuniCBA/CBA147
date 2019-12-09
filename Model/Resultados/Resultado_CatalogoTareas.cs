using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_CatalogoTareas
    {
        public virtual string Nombre { get; set; }
        public virtual string Observaciones { get; set; }
        
        public Resultado_CatalogoTareas() 

        {

        }
    }
}

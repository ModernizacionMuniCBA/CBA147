using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_AreaConSubarea
    {
        public int? Id { get; set; }
        public List<int?> IdsHijos { get; set; }
        
        public Consulta_AreaConSubarea()
        {

        }
    }
}

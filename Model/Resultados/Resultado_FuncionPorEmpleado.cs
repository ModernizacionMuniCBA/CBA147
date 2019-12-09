using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_FuncionPorEmpleado : Resultado_Base<FuncionPorEmpleado>
    {
        public int IdFuncion { get; set; }
        public int IdEmpleado { get; set; }

        public Resultado_FuncionPorEmpleado():base()
        {

        }
    }
}

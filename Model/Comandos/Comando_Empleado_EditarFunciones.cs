using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Empleado_EditarFunciones
    {
        public virtual int IdEmpleado { get; set; }
        public virtual List<int> IdFunciones { get; set; }



        public Comando_Empleado_EditarFunciones()
        {
       
        }

    }
}

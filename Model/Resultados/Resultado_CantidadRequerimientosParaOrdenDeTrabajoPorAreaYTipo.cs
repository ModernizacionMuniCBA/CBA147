using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo
    {
        public int IdTipo;
        public int Cantidad;

        public Resultado_CantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo()
            : base()
        {

        }
    }
}

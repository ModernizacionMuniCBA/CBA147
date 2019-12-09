using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla<T>
    {
        public int CantidadMaxima { get; set; }
        public int Cantidad { get; set; }

        public bool SuperaElLimite
        {
            get
            {
                return Cantidad > CantidadMaxima;
            }
        }

        public List<T> Data { get; set; }

        public ResultadoTabla() : base() { }

    }
}

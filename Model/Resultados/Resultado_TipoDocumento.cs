using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_TipoDocumento : Resultado_Base<TipoDocumento>
    {
        public string Nombre { get; set; }
        public int KeyValue { get; set; }

        public Resultado_TipoDocumento()
            : base()
        {

        }

        public Resultado_TipoDocumento(TipoDocumento entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            KeyValue = (int)entity.KeyValue;
        }
    }
}

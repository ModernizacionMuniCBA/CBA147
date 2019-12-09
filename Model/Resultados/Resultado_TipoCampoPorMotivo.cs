using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_TipoCampoPorMotivo : Resultado_Base<TipoCampo>
    {
        public virtual string Nombre { get; set; }
                public virtual Enums.TipoCampoPorMotivo KeyValue{ get; set; }

        public Resultado_TipoCampoPorMotivo()
            : base()
        {

        }

        public Resultado_TipoCampoPorMotivo(TipoCampo entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }
            KeyValue = entity.KeyValue;
            Nombre = entity.Nombre;
        }

        public static List<Resultado_TipoCampoPorMotivo> ToList(List<TipoCampo> list)
        {
            return list.Select(x => new Resultado_TipoCampoPorMotivo(x)).ToList();
        }
    }
}

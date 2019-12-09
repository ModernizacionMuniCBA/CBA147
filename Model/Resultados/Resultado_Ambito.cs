using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Ambito : Resultado_Base<CerrojoAmbito>
    {
        public virtual string Nombre { get; set; }
        public virtual int KeyValue { get; set; }

        public Resultado_Ambito()
            : base()
        {

        }

        public Resultado_Ambito(CerrojoAmbito entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            KeyValue = entity.KeyValue;
        }

        public static List<Resultado_Ambito> ToList(List<CerrojoAmbito> list)
        {
            return list.Select(x => new Resultado_Ambito(x)).ToList();
        }
    }
}

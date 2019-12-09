using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrigenPorAmbito : Resultado_Base<OrigenPorAmbito>
    {
        public virtual int AmbitoId { get; set; }
        public virtual string AmbitoNombre { get; set; }
        public virtual int AmbitoKeyValue { get; set; }
        public virtual int OrigenId { get; set; }
        public virtual string OrigenNombre { get; set; }

        public Resultado_OrigenPorAmbito()
            : base()
        {

        }

        public Resultado_OrigenPorAmbito(OrigenPorAmbito entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            if (entity.Ambito != null)
            {
                AmbitoId = entity.Ambito.Id;
                AmbitoNombre = entity.Ambito.Nombre;
                AmbitoKeyValue = entity.Ambito.KeyValue;
            }
            if (entity.Origen != null)
            {
                OrigenId = entity.Origen.Id;
                OrigenNombre = entity.Origen.Nombre;
            }
        }

        public static List<Resultado_OrigenPorAmbito> ToList(List<OrigenPorAmbito> list)
        {
            return list.Select(x => new Resultado_OrigenPorAmbito(x)).ToList();
        }
    }
}

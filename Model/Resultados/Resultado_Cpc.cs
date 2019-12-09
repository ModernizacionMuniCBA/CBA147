using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Cpc : Resultado_Base<Cpc>
    {
        public virtual string Nombre { get; set; }
        public virtual int IdCatastro { get; set; }
        public virtual int Numero { get; set; }

        public Resultado_Cpc()
            : base()
        {

        }

        public Resultado_Cpc(Cpc entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            IdCatastro = entity.IdCatastro;
            Numero = entity.Numero;
        }
        public static List<Resultado_Cpc> ToList(List<Cpc> list)
        {
            return list.Select(x => new Resultado_Cpc(x)).ToList();
        }
    }
}

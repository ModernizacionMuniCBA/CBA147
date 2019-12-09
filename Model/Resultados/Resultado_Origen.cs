using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Origen : Resultado_Base<Origen>
    {
        public virtual string Nombre { get; set; }
        public virtual string KeyAlias { get; set; }
        public virtual string KeySecret { get; set; }

        public Resultado_Origen()
            : base()
        {

        }

        public Resultado_Origen(Origen entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            KeyAlias = entity.KeyAlias;
            KeySecret = entity.KeySecret;
        }

        public static List<Resultado_Origen> ToList(List<Origen> list)
        {
            return list.Select(x => new Resultado_Origen(x)).ToList();
        }
    }
}

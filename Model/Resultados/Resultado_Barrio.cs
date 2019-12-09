using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Barrio : Resultado_Base<Barrio>
    {
        public virtual string Nombre { get; set; }
        public virtual int IdCatastro { get; set; }
        public virtual string XCentro { get; set; }
        public virtual string YCentro { get; set; }
        public virtual string XCentroGoogle { get; set; }
        public virtual string YCentroGoogle { get; set; }

        public Resultado_Barrio()
            : base()
        {

        }

        public Resultado_Barrio(Barrio entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            IdCatastro = entity.IdCatastro;
            XCentro = entity.XCentro;
            YCentro = entity.YCentro;
        }

        public static List<Resultado_Barrio> ToList(List<Barrio> list)
        {
            return list.Select(x => new Resultado_Barrio(x)).ToList();
        }
    }
}

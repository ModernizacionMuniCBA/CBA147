using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_InformacionOrganica : Resultado_Base<InformacionOrganica>
    {

        public Resultado_InformacionOrganicaDireccion Direccion { get; set; }
        public Resultado_Area Area { get; set; }

        public Resultado_InformacionOrganica()
            : base()
        {

        }

        public Resultado_InformacionOrganica(InformacionOrganica entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            this.Area = new Resultado_Area(entity.Area);
            this.Direccion = new Resultado_InformacionOrganicaDireccion(entity.Direccion);
        }

        public static List<Resultado_InformacionOrganica> ToList(List<InformacionOrganica> list)
        {
            return list.Select(x => new Resultado_InformacionOrganica(x)).ToList();
        }

    }
}

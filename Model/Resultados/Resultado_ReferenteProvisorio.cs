using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_ReferenteProvisorio : Resultado_Base<ReferenteProvisorio>
    {
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual int? DNI { get; set; }
        public virtual string Telefono { get; set; }
        public virtual bool GeneroMasculino { get; set; }

        public Resultado_ReferenteProvisorio()
            : base()
        {

        }

        public Resultado_ReferenteProvisorio(ReferenteProvisorio referente)
            : base(referente)
        {
            if (referente == null)
            {
                return;
            }

            Apellido = referente.Apellido;
            Nombre = referente.Nombre;
            DNI = referente.DNI;
            Telefono = referente.Telefono;
            GeneroMasculino = referente.GeneroMasculino;
        }
        public static List<Resultado_ReferenteProvisorio> ToList(List<ReferenteProvisorio> list)
        {
            return list.Select(x => new Resultado_ReferenteProvisorio(x)).ToList();
        }

    }
}

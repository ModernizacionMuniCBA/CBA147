using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_InformacionOrganicaSecretaria : Resultado_Base<InformacionOrganicaSecretaria>
    {
        public string Nombre { get; set; }
        public Resultado_InformacionOrganicaSecretaria()
            : base()
        {

        }

        public Resultado_InformacionOrganicaSecretaria(InformacionOrganicaSecretaria entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            this.Nombre = entity.Nombre;
        }

        public static List<Resultado_InformacionOrganicaSecretaria> ToList(List<InformacionOrganicaSecretaria> list)
        {
            return list.Select(x => new Resultado_InformacionOrganicaSecretaria(x)).ToList();
        }

    }
}

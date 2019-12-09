using Model.Comandos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class CategoriaEdificioMunicipal : BaseEntity
    {
       public virtual string Nombre { get; set; }
       public virtual IList<EdificioMunicipal> EdificiosMunicipales { get; set; }

        public CategoriaEdificioMunicipal()
        {

        }

    }
}

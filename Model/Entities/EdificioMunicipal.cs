using Model.Comandos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Serializable()]
    public class EdificioMunicipal : BaseEntity
    {
        //Datos Identificatorios
        public virtual string Nombre { get; set; }
        public virtual CategoriaEdificioMunicipal Categoria { get; set; }
        public virtual Domicilio Domicilio { get; set; }
        public EdificioMunicipal()
        {

        }

    }
}

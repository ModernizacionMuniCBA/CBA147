using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_CatalogoMotivos
    {
        public virtual string Nombre { get; set; }
        public virtual bool Urgente { get; set; }
        public virtual  Enums.TipoMotivo Tipo { get; set; }
        public virtual bool Principal { get; set; }
        public virtual int Prioridad { get; set; }

        public virtual int idCategoria { get; set; }
        public virtual string Categoria { get; set; }
        
        public Resultado_CatalogoMotivos() 

        {

        }
    }
}

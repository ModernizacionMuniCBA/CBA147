using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_CatalogoUsuarios
    {
        public virtual string Apellido { get; set; }
        public virtual string Nombre { get; set; }
        public virtual  int Dni { get; set; }
        public virtual string Usuario { get; set; }
        public virtual string Rol { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefono { get; set; }
        public virtual string Ubicacion { get; set; }  


        public Resultado_CatalogoUsuarios() 
        {

        }
    }
}

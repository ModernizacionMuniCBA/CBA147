using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_RequerimientoFavoritoPorUsuario
    {
        public int IdUser { get; set; }
        public int IdRequerimiento { get; set; }
        public bool Favorito { get; set; }
        public Comando_RequerimientoFavoritoPorUsuario()
        {

        }
    }
}

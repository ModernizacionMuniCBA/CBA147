using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_FCMToken
    {
        public int IdUsuario{get;set;}
        public string Token { get; set; }
        public Comando_FCMToken()
        {

        }
    }
}

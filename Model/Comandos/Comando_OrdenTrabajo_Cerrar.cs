using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_OrdenTrabajo_Cerrar
    {
        public int Id { get; set; }
        public List<Comando_OrdenTrabajo_Cerrar_Requerimientos> Requerimientos { get; set; }
        public string Observaciones { get; set; }
        public Comando_OrdenTrabajo_Cerrar()
        {

        }
    }

    public class Comando_OrdenTrabajo_Cerrar_Requerimientos
    {
        public int IdRequerimiento { get; set; }
        public int KeyValueEstado { get; set; }

        public Comando_OrdenTrabajo_Cerrar_Requerimientos()
        {

        }
    }
}

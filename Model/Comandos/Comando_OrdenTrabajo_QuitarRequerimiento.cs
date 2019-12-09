using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_OrdenTrabajo_QuitarRequerimiento
    {
        public int? IdOrdenTrabajo { get; set; }
        public int? IdOrdenInspeccion { get; set; }
        public int IdRequerimiento { get; set; }
        public Enums.EstadoRequerimiento EstadoKeyValue { get; set; }
        public string Observaciones { get; set; }
        public bool? Desmarcar { get; set; }
        public Comando_OrdenTrabajo_QuitarRequerimiento()
        {

        }
    }
}

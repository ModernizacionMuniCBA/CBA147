using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenTrabajoDetappe_EstadoHistorial
    {
        public int EstadoKeyValue { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public string EstadoFecha { get; set; }
        public string EstadoObservaciones { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioUsuario { get; set; }
    }
}

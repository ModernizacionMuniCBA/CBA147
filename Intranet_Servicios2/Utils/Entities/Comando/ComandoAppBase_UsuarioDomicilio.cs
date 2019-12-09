using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Intranet_Servicios2.Utils.Entities.Comando
{

    [Serializable]
    public class ComandoAppBase_UsuarioDomicilio
    {
        public string Direccion { get; set; }
        public string Altura { get; set; }
        public string Torre { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string CodigoPostal { get; set; }
        public string Ciudad { get; set; }
        public int? IdCiudad { get; set; }
        public string Barrio { get; set; }
        public int? IdBarrio { get; set; }
        public string Provincia { get; set; }
        public int? IdProvincia { get; set; }
        public ComandoAppBase_UsuarioDomicilio()
        {

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet_Servicios2.v1.Entities.Comandos
{
    public class ComandoApp_UsuarioEditarDatosPersonales
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public bool SexoMasculino { get; set; }

        public ComandoApp_UsuarioEditarDatosPersonales()
        {

        }
    }


}
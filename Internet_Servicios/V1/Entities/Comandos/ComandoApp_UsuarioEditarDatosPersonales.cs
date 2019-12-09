using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Comandos
{
    public class ComandoApp_UsuarioEditarDatosPersonales
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public bool SexoMasculino { get; set; }
    }
}
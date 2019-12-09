using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.WSVecinoVirtual.Comandos
{
    [Serializable]
    public class VVComando_UsuarioValidarRenaper
    {
        //Datos personales
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public bool SexoMasculino { get; set; }
        public DateTime? getFechaNacimiento()
        {
            try
            {
                return DateTime.ParseExact(FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
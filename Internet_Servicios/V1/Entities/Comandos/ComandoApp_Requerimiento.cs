using System;
using System.Linq;

namespace Internet_Servicios.V1.Entities.Comandos
{
    public class ComandoApp_Requerimiento
    {
        public ComandoApp_RequerimientoAutenticacion Autenticacion { get; set; }
        public int IdMotivo { get; set; }
        public string Descripcion { get; set; }
        public ComandoApp_Domicilio Domicilio { get; set; }
        public string Imagen { get; set; }
    }

    public class ComandoApp_RequerimientoAutenticacion
    {
        public string ReCaptcha { get; set; }
        public string KeyValidacionReCaptcha { get; set; }
        public string OrigenAlias { get; set; }
        public string OrigenKey { get; set; }
    }

    public class ComandoApp_Domicilio
    {
        public string Direccion { get; set; }
        public string Observaciones { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}
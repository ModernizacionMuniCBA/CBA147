using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet_Servicios2.Utils.Entities.Comando
{
    public class ComandoAppBase_Requerimiento
    {
        public ComandoAppBase_RequerimientoAutenticacion Autenticacion { get; set; }
        public int IdMotivo { get; set; }
        public string Descripcion { get; set; }
        public ComandoAppBase_Domicilio Domicilio { get; set; }
        public string Imagen { get; set; }

        public ComandoAppBase_Requerimiento()
        {

        }
    }

    public class ComandoAppBase_RequerimientoAutenticacion
    {
        public string ReCaptcha { get; set; }
        public string KeyValidacionReCaptcha { get; set; }
        public string OrigenAlias { get; set; }
        public string OrigenKey { get; set; }

        public ComandoAppBase_RequerimientoAutenticacion()
        {

        }
    }

    public class ComandoAppBase_Domicilio
    {

        public string Direccion { get; set; }
        public string Observaciones { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }


        public ComandoAppBase_Domicilio()
        {

        }
    }
}
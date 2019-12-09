using Internet_UI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internet_UI.Utils.Strings
{
    public class _Strings
    {
        public string NombreMuni { get; set; }
        public string NombreSistema { get; set; }

        public string Boton_Aceptar { get; set; }
        public string Boton_Cancelar { get; set; }
        public string Boton_Reintentar { get; set; }
        public string Boton_Volver { get; set; }
        public string Boton_Registrar { get; set; }
        public string Boton_Editar { get; set; }
        public _Strings()
        {
            this.NombreMuni = Recursos.NombreMuni;
            this.NombreSistema = Recursos.NombreSistema;

            this.Boton_Aceptar = Recursos.Boton_Aceptar;
            this.Boton_Cancelar = Recursos.Boton_Cancelar;
            this.Boton_Reintentar = Recursos.Boton_Reintentar;
            this.Boton_Volver = Recursos.Boton_Volver;
            this.Boton_Registrar = Recursos.Boton_Registrar;
            this.Boton_Editar = Recursos.Boton_Editar;

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Resources;

namespace Intranet_UI.Utils.Strings
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
            this.NombreMuni = RecursosGeneral.NombreMuni;
            this.NombreSistema = RecursosGeneral.NombreSistema;

            this.Boton_Aceptar = RecursosGeneral.Boton_Aceptar;
            this.Boton_Cancelar = RecursosGeneral.Boton_Cancelar;
            this.Boton_Reintentar = RecursosGeneral.Boton_Reintentar;
            this.Boton_Volver = RecursosGeneral.Boton_Volver;
            this.Boton_Registrar = RecursosGeneral.Boton_Registrar;
            this.Boton_Editar = RecursosGeneral.Boton_Editar;

        }
    }
}
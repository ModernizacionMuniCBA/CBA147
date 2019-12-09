using Internet_UI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internet_UI.Utils.Strings
{
    public class Strings_RequerimientoNuevo : _Strings
    {
        public string Texto_Titulo_NuevoRequerimiento { get; set; }

        public string Texto_Descripcion_Tipo { get; set; }
        public string Texto_Titulo_Tipo { get; set; }

        public string Texto_Descripcion_Descripcion { get; set; }
        public string Texto_Titulo_Descripcion { get; set; }

        public string Texto_Descripcion_Ubicacion { get; set; }
        public string Texto_Descripcion_Ubicacion_Mobile { get; set; }
        public string Texto_Titulo_Ubicacion { get; set; }

        public string Texto_Descripcion_Foto { get; set; }
        public string Texto_Titulo_Foto { get; set; }

        public string Texto_Descripcion_Confirmacion { get; set; }
        public string Texto_Titulo_Confirmacion { get; set; }


        public string Texto_RegistradoCorrectamente_ConEmail { get; set; }
        public string Texto_RegistradoCorrectamente_SinEmail { get; set; }

        public string Texto_Error_Registrando_ConDetalle { get; set; }
        public string Texto_Error_Registrando_SinDetalle { get; set; }

        public Strings_RequerimientoNuevo()
            : base()
        {
            this.Texto_Titulo_NuevoRequerimiento = Recursos_RequerimientoNuevo.Texto_Titulo_NuevoRequerimiento;

            this.Texto_Descripcion_Tipo = Recursos_RequerimientoNuevo.Texto_Descripcion_Tipo;
            this.Texto_Titulo_Tipo = Recursos_RequerimientoNuevo.Texto_Titulo_Tipo;

            this.Texto_Descripcion_Descripcion = Recursos_RequerimientoNuevo.Texto_Descripcion_Descripcion;
            this.Texto_Titulo_Descripcion = Recursos_RequerimientoNuevo.Texto_Titulo_Descripcion;

            this.Texto_Descripcion_Ubicacion = Recursos_RequerimientoNuevo.Texto_Descripcion_Ubicacion;
            this.Texto_Descripcion_Ubicacion_Mobile = Recursos_RequerimientoNuevo.Texto_Descripcion_Ubicacion_Mobile;
            this.Texto_Titulo_Ubicacion = Recursos_RequerimientoNuevo.Texto_Titulo_Ubicacion;

            this.Texto_Descripcion_Foto = Recursos_RequerimientoNuevo.Texto_Descripcion_Foto;
            this.Texto_Titulo_Foto = Recursos_RequerimientoNuevo.Texto_Titulo_Foto;

            this.Texto_Descripcion_Confirmacion = Recursos_RequerimientoNuevo.Texto_Descripcion_Confirmacion;
            this.Texto_Titulo_Confirmacion = Recursos_RequerimientoNuevo.Texto_Titulo_Confirmacion;

            this.Texto_RegistradoCorrectamente_ConEmail = Recursos_RequerimientoNuevo.Texto_RegistradoCorrectamente_ConEmail;
            this.Texto_RegistradoCorrectamente_SinEmail = Recursos_RequerimientoNuevo.Texto_RegistradoCorrectamente_SinEmail;
            this.Texto_Error_Registrando_ConDetalle = Recursos_RequerimientoNuevo.Texto_Error_Registrando_ConDetalle;
            this.Texto_Error_Registrando_SinDetalle = Recursos_RequerimientoNuevo.Texto_Error_Registrando_SinDetalle;
        }
    }
}
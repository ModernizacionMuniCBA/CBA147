using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Resources;

namespace Intranet_UI.Utils.Strings
{
    public class Strings_Login : _Strings
    {
        public string Texto_IniciarSesion { get; set; }
        public string Boton_RecuperarCuenta { get; set; }
        public string Boton_IniciarSesion { get; set; }
        public string Texto_NoTieneCuenta { get; set; }
        public string Boton_CrearUsuario { get; set; }
        public string Texto_Usuario_NoActivado { get; set; }

        public string Texto_InfoCBA147_Titulo { get; set; }
        public string Texto_InfoCBA147_Texto { get; set; }
        public string Boton_Contacto { get; set; }
        public string Boton_TerminosCondiciones{ get; set; }
        public string Boton_PoliticaSeguridad { get; set; }
        public string Boton_PoliticaPrivacidad { get; set; }

        public Strings_Login()
            : base()
        {
            this.Texto_IniciarSesion = Recursos_Login.Texto_IniciarSesion;
            this.Boton_RecuperarCuenta = Recursos_Login.Boton_RecuperarCuenta;
            this.Boton_IniciarSesion = Recursos_Login.Boton_IniciarSesion;
            this.Texto_NoTieneCuenta = Recursos_Login.Texto_NoTieneCuenta;
            this.Boton_CrearUsuario = Recursos_Login.Boton_CrearUsuario;
            this.Texto_Usuario_NoActivado = Recursos_Login.Texto_Usuario_NoActivado;
            this.Texto_InfoCBA147_Titulo = Recursos_Login.Texto_InfoCBA147_Titulo;
            this.Texto_InfoCBA147_Texto = Recursos_Login.Texto_InfoCBA147_Texto;
            this.Boton_Contacto = Recursos_Login.Boton_Contacto;
            this.Boton_TerminosCondiciones = Recursos_Login.Boton_TerminosCondiciones;
            this.Boton_PoliticaSeguridad = Recursos_Login.Boton_PoliticaSeguridad;
            this.Boton_PoliticaPrivacidad = Recursos_Login.Boton_PoliticaPrivacidad;
        }
    }
}
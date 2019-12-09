using Internet_Servicios;
using Internet_UI.Utils;
using InternetUI_Entities.Comandos;
using InternetUI_Entities.Resultados;
using System;
using System.Linq;
using System.Web.Services;

namespace Internet_UI.Servicios
{
    public class ServicioUsuario : _BaseService
    {
        [WebMethod(EnableSession = true)]
        public Resultado<bool?> SetToken(string token)
        {
            Session[Consts.TOKEN] = token;

            var resultadoToken = ValidarToken();
            if (!resultadoToken.Ok)
            {
                Session[Consts.TOKEN] = null;
                var resultado = new Resultado<bool?>();
                resultado.Error = resultadoToken.Error;
                return resultado;
            }

            return resultadoToken;
        }

        [WebMethod(EnableSession = true)]
        public Resultado<bool?> ValidarToken()
        {
            var token = Session[Consts.TOKEN];

            if (token == null)
            {
                var resultado = new Resultado<bool?>();
                resultado.Return = false;
                return resultado;
            }

            var url = "v1/Usuario/ValidarToken?token=" + token;
            return RestCall.Call<bool?>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<bool?> CerrarSesion()
        {
            var url = "v1/Usuario/CerrarSesion?token=" + Session[Consts.TOKEN];
            return RestCall.Call<bool?>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<ResultadoApp_Usuario> GetDatosUsuario()
        {
            var token = Session[Consts.TOKEN];
            var url = "v1/Usuario?token=" + token;
            return RestCall.Call<ResultadoApp_Usuario>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<string> ActualizarFotoPersonal(ComandoApp_UsuarioEditarFotoPersonal comando)
        {
            var url = "v1/Usuario/ActualizarFotoPersonal?token=" + Session[Consts.TOKEN];
            return RestCall.Call<string>(url, RestSharp.Portable.Method.PUT, comando);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<ResultadoApp_Usuario> ActualizarDatosPersonales(ComandoApp_UsuarioEditarDatosPersonales comando)
        {
            var url = "v1/Usuario/ActualizarDatosPersonales?token=" + Session[Consts.TOKEN];
            return RestCall.Call<ResultadoApp_Usuario>(url, RestSharp.Portable.Method.PUT, comando);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<ResultadoApp_Usuario> ActualizarDatosContacto(ComandoApp_UsuarioEditarDatosContacto comando)
        {
            var url = "v1/Usuario/ActualizarDatosContacto?token=" + Session[Consts.TOKEN];
            return RestCall.Call<ResultadoApp_Usuario>(url, RestSharp.Portable.Method.PUT, comando);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<ResultadoApp_Usuario> ActualizarEstadoCivil(int idEstadoCivil)
        {
            var url = "v1/Usuario/ActualizarEstadoCivil?token=" + Session[Consts.TOKEN] + "&idEstadoCivil=" + idEstadoCivil;
            return RestCall.Call<ResultadoApp_Usuario>(url, RestSharp.Portable.Method.PUT, null);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<ResultadoApp_Usuario> ActualizarDomicilio(ComandoApp_UsuarioDomicilio comando)
        {
            var url = "v1/Usuario/ActualizarDomicilio?token=" + Session[Consts.TOKEN];
            return RestCall.Call<ResultadoApp_Usuario>(url, RestSharp.Portable.Method.PUT, comando);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<bool?> EsValidadoRenaper()
        {
            var url = "v1/Usuario/EsValidadoRenaper?token=" + Session[Consts.TOKEN];
            return RestCall.Call<bool?>(url, RestSharp.Portable.Method.GET);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<bool?> CambiarUsername(ComandoApp_UsuarioCambiarUsername comando)
        {
            var url = "v1/Usuario/CambiarUsername?token=" + Session[Consts.TOKEN] + "&username=" + comando.Username;
            return RestCall.Call<bool?>(url, RestSharp.Portable.Method.PUT);
        }

        [WebMethod(EnableSession = true)]
        public Resultado<bool?> CambiarPassword(ComandoApp_UsuarioCambiarPassword comando)
        {
            var url = "v1/Usuario/CambiarPassword?token=" + Session[Consts.TOKEN] + "&passwordAnterior=" + comando.PasswordAnterior + "&passwordNuevo=" + comando.PasswordNuevo;
            return RestCall.Call<bool?>(url, RestSharp.Portable.Method.PUT);
        }
    }
}
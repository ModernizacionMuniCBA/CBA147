using Model;
using Model.WSVecinoVirtual.Resultados;
using Rules;
using Rules.Rules;
using System;
using System.Configuration;
using System.Linq;
using System.Web.SessionState;

namespace UI.Resources
{
    public class SessionKey
    {
        private static string Key_UsuarioLogueado = "UsuarioLogueado";
        private static string baseUrl = ConfigurationManager.AppSettings["URL_WS_VECINO_VIRTUAL"];

        public static void borrarSesssion(HttpSessionState session)
        {
            try
            {
                var resultadoUsuario = getUsuarioLogueado(session);
                if (resultadoUsuario == null)
                {

                    RestCall.Call<bool>(baseUrl+"v1/Usuario/CerrarSesion?token="+resultadoUsuario.Token, RestSharp.Portable.Method.PUT);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                session[SessionKey.Key_UsuarioLogueado] = null;
                session.Clear();
            }
        }

        public static Result<UsuarioLogueado> IniciarSesionEmpleado(HttpSessionState session, string token)
        {
            var resultado = new Result<UsuarioLogueado>();

            var resultadoLogin = new _VecinoVirtualUsuarioRules(null).IniciarSesionEmpleado(token);
            if (!resultadoLogin.Ok)
            {
                resultado.Copy(resultadoLogin.Errores);
                return resultado;
            }

            //Genero el usuario logeado
            session[SessionKey.Key_UsuarioLogueado] = resultadoLogin.Return;
            resultado.Return = resultadoLogin.Return;
            return resultado;
        }

        public static UsuarioLogueado getUsuarioLogueado(HttpSessionState session)
        {
            return session[SessionKey.Key_UsuarioLogueado] as UsuarioLogueado;
        }

        public static int? getOrigen(HttpSessionState session)
        {
            if (!IsLogin(session)) return null;
            var user = getUsuarioLogueado(session);
            if (user == null) return null;
            return user.IdOrigenElegido;
        }

        internal static bool IsLogin(HttpSessionState session)
        {
            //Valido estar logeado
            var resultadoUsuario = getUsuarioLogueado(session);
            if (resultadoUsuario == null) return false;

            try
            {
                //Valido el Token
                var resultadoToken=RestCall.Call<bool>(baseUrl+"v1/Usuario/ValidarToken?token="+getUsuarioLogueado(session).Token, RestSharp.Portable.Method.GET);
                if (!resultadoToken.Ok || !resultadoToken.Return) return false;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public static Result<bool> SetOrigen(HttpSessionState session, int? idOrigen)
        {
            var resultado = new Result<bool>();

            var usuarioLogeado = getUsuarioLogueado(session);
            if (usuarioLogeado == null) return resultado;

            if (idOrigen.HasValue && idOrigen.Value != -1)
            {
                if (!usuarioLogeado.OrigenesDisponibles.Where(x => x.Id == idOrigen.Value).Any())
                {
                    resultado.AddErrorPublico("El usuario no posee el origen indicado");
                    return resultado;
                }
                usuarioLogeado.IdOrigenElegido = idOrigen;
                session[Key_UsuarioLogueado] = usuarioLogeado;
            }
            else
            {
                usuarioLogeado.IdOrigenElegido = null;
                session[Key_UsuarioLogueado] = usuarioLogeado;
            }

            resultado.Return = true;
            return resultado;
        }


        //Objeto
        public static  VVResultado_Objeto GetObjeto(VVResultado_Permisos rol, String url)
        {
            if (rol == null || url == null) return null;

            var partes = url.Split('/');
            if (partes.Length == 0) return null;
            url = partes[partes.Length - 1];
            url = url.ToUpper();
            url = url.Split('?')[0];

            //Busco
            foreach (var item in rol.Objetos)
            {
                if (item.Valor.ToUpper().Equals(url))
                {
                    return item;
                }
            }
            return null;
        }

        //Acceso
        public static bool ValidarAcceso(VVResultado_Permisos rol, string url)
        {
            if (rol == null || url == null) return false;

            //El admin accede a todas las paginas
            if (rol.Rol.ToUpper() == "ADMINISTRADOR") return true;

            //Obtengo el objeto y valido su ALTA
            var obj = GetObjeto(rol, url);
            if (obj == null) return false;
            return obj.Acceso.Consulta;
        }
    }
}
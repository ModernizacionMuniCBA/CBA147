using Rules;
using Rules.Rules;
using System;
using System.Linq;
using System.Web.Http;

namespace Intranet_Servicios2.Utils.Controllers
{
    public class Control : ApiController
    {
        protected string GetToken()
        {
            return Request.Headers.GetValues("Token").First();
        }

        public ResultadoServicio<bool> ValidarToken()
        {
            var token = GetToken();
            var resultado = new ResultadoServicio<bool>();

            try
            {
                var resultadoValidarToken = new _VecinoVirtualUsuarioRules(null).ValidarToken(token);
                if (!resultadoValidarToken.Ok)
                {
                    resultado.Error = resultadoValidarToken.Error;
                    return resultado;
                }

                resultado.Return = resultadoValidarToken.Return;
            }
            catch (Exception)
            {
                resultado.SetError();
            }

            return resultado;
        }

        public ResultadoServicio<UsuarioLogueado> GetUsuarioLogeado(string t = null)
        {
            string token = null;
            if (t != null)
            {
                token = t;
            }
            else
            {
                token = GetToken();
            }
            var resultado = new ResultadoServicio<UsuarioLogueado>();

            try
            {
                var resultadoIdByToken = new _VecinoVirtualUsuarioRules(null).GetIdByToken(token);
                if (!resultadoIdByToken.Ok)
                {
                    resultado.Error = resultadoIdByToken.Error;
                    return resultado;
                }

                var resultadoId = new _VecinoVirtualUsuarioRules(null).GetById(resultadoIdByToken.Return);
                if (!resultadoId.Ok)
                {
                    resultado.Error = resultadoId.Error;
                    return resultado;
                }

                var usuarioLogueado = new UsuarioLogueado();

                usuarioLogueado.Usuario = new Model.Entities._Resultado_VecinoVirtualUsuario(resultadoId.Return);
                usuarioLogueado.Token = token;

                resultado.Return = usuarioLogueado;
            }
            catch (Exception e)
            {
                resultado.SetError();
            }

            return resultado;
        }
    }
}
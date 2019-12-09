using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Consultas;
using Model.Comandos;
using System.Configuration;

namespace Rules.Rules
{
    public class FCMTokenRules : BaseRules<FCMToken>
    {
        private readonly FCMTokenDAO dao;

        public FCMTokenRules(UsuarioLogueado data)
            : base(data)
        {
            dao = FCMTokenDAO.Instance;
        }

        public Result<bool?> Insertar(string token)
        {
            var result = new Result<bool?>();
            var resultToken = ExisteToken(token);
            if (!resultToken.Ok)
            {
                result.Copy(resultToken.Errores);
                return result;
            }

            //si no existe, lo inserto
            if (!resultToken.Return)
            {
                var FCMToken = new FCMToken();
                FCMToken.Observaciones = token;
                FCMToken.UsuarioToken = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(getUsuarioLogueado().Usuario.Id).Return;
                
                var resultInsert = base.Insert(FCMToken);
                if (!resultInsert.Ok)
                {
                    result.Copy(resultInsert.Errores);
                    return result;
                }
            }

            result.Return = true;
            return result;
        }

        public Result<bool> Borrar(string token)
        {
            var result = new Result<bool>();
            var resultToken = GetToken(token);
            if (!resultToken.Ok)
            {
                result.Copy(resultToken.Errores);
                return result;
            }

            //si no existe, doy error
            if (resultToken.Return==null)
            {
                result.AddErrorPublico("El token no existe");
                return result;
            }

            //si existe, lo elimino 
            var resultQuitar = base.Delete(resultToken.Return);
            if (resultToken.Return == null)
            {
                result.AddErrorPublico("Error al eliminar el token");
                return result;
            }

            result.Return = true;
            return result;
        }

        public Result<List<FCMToken>> GetTokensByIdUsuario(int idUsuario)
        {
            return dao.GetTokensByIdUsuario(idUsuario);
        }

        private Result<bool> ExisteToken(string token)
        {
            return dao.ExisteToken(token, getUsuarioLogueado().Usuario.Id);
        }

        private Result<FCMToken> GetToken(string token)
        {
            return dao.GetToken(token, getUsuarioLogueado().Usuario.Id);
        }

        public Result<List<FCMToken>> EnviarNotificacion(Comando_NotificacionPush comando)
        {
            var result = new Result<List<FCMToken>>();
            try
            {
                var resultTokens = GetTokensByIdUsuario(comando.IdUsuario);
                if (!resultTokens.Ok)
                {
                    result.Copy(resultTokens.Errores);
                    return result;
                }


            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
    }
}

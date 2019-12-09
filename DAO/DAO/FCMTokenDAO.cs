using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using Model.Comandos;
using NHibernate;

namespace DAO.DAO
{
    public class FCMTokenDAO : BaseDAO<FCMToken>
    {
        private static FCMTokenDAO instance;

        public static FCMTokenDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FCMTokenDAO();
                }
                return instance;
            }
        }

        public IQueryOver<FCMToken, FCMToken> GetQuery(string token, int? idUsuario, bool? dadosDeBaja)
        {
            var query = GetSession().QueryOver<FCMToken>();

            if (idUsuario.HasValue)
            {
                query.Where(x => x.UsuarioToken.Id == (int)idUsuario);
            }

            if (!String.IsNullOrEmpty(token))
            {
                query.Where(x => x.Observaciones.IsLike(token));
            }

            if (dadosDeBaja.HasValue && dadosDeBaja.Value)
            {
                query.Where(x=>x.FechaBaja != null);
            }
            else if (dadosDeBaja.HasValue && !dadosDeBaja.Value)
            {
                query.Where(x => x.FechaBaja == null);
            }

            return query;
        }
        public Result<FCMToken> GetToken(string token, int idUsuario)
        {
            var result = new Result<FCMToken>();
            try
            {
                var query = GetQuery(token, idUsuario, false);
                result.Return = query.List().FirstOrDefault();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<bool> ExisteToken(string token, int idUsuario)
        {
            var result = new Result<bool>();
            var resultToken = GetToken(token, idUsuario);
            if (!resultToken.Ok)
            {
                result.AddErrorPublico("Error al consultar el token");
                return result;
            }

            //no existe
            if (resultToken.Return == null)
            {
                result.Return = false;
                return result;
            }

            //existe
            result.Return = true;
            return result;
        }

        public Result<List<FCMToken>> GetTokensByIdUsuario(int idUsuario)
        {
            var result = new Result<List<FCMToken>>();
            try
            {
                var query = GetQuery(null, idUsuario, false);
                result.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }


    }
}

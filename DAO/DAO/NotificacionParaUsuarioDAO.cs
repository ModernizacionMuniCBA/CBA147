using Model;
using Model.Consultas;
using Model.Entities;
using Model.Resultados;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAO.DAO
{
    public class NotificacionParaUsuarioDAO : BaseDAO<NotificacionSistema>
    {
        private static NotificacionParaUsuarioDAO instance;

        public static NotificacionParaUsuarioDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new NotificacionParaUsuarioDAO();
                return instance;
            }
        }

        private IQueryOver<NotificacionSistema, NotificacionSistema> GetQuery(Consulta_NotificacionParaUsuario consulta)
        {
            var query = GetSession().QueryOver<NotificacionSistema>();



            //Notificar
            if (consulta.Notificar.HasValue)
            {
                query.Where(x => x.Notificar == consulta.Notificar.Value);
            }

            //Dados de baja
            if (consulta.DadosDeBaja.HasValue)
            {
                if (consulta.DadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }
            }

            return query;
        }


        public Result<List<NotificacionSistema>> GetByFilters(Consulta_NotificacionParaUsuario consulta)
        {
            var result = new Result<List<NotificacionSistema>>();
            try
            {
                var query = GetQuery(consulta);
                result.Return = query.List().ToList();
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<Resultado_NotificacionSistema>> GetResultadoByFilters(Consulta_NotificacionParaUsuario consulta)
        {
            var result = new Result<List<Resultado_NotificacionSistema>>();
            try
            {
                var query = GetQuery(consulta);
                result.Return = Resultado_NotificacionSistema.ToList(query.List().ToList());
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<int> GetCantidadByFilters(Consulta_NotificacionParaUsuario consulta)
        {
            var result = new Result<int>();
            try
            {
                var query = GetQuery(consulta);
                result.Return = query.RowCount();
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
    }
}

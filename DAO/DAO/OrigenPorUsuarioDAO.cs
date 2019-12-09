using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using Model.Consultas;
using NHibernate;
using Model.Resultados;
using Model.Comandos;


namespace DAO.DAO
{
    public class OrigenPorUsuarioDAO : BaseDAO<OrigenPorUsuario>
    {
        private static OrigenPorUsuarioDAO instance;

        public static OrigenPorUsuarioDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OrigenPorUsuarioDAO();
                }
                return instance;
            }
        }

        private IQueryOver<OrigenPorUsuario, OrigenPorUsuario> GetQuery(Consulta_OrigenPorUsuario consulta)
        {
            var query = GetSession().QueryOver<OrigenPorUsuario>();

            //Area Id
            if (consulta.UsuarioId.HasValue && consulta.UsuarioId.Value != -1)
            {
                query.JoinQueryOver<_VecinoVirtualUsuario>(x => x.UsuarioOrigen).Where(x => x.Id == consulta.UsuarioId.Value);
            }

            //Origen Id
            if (consulta.OrigenId.HasValue && consulta.OrigenId.Value!=-1)
            {
                query.JoinQueryOver<Origen>(x => x.Origen).Where(x => x.Id == consulta.OrigenId.Value);
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

        public Result<List<Resultado_OrigenPorUsuario>> GetByFilters(Consulta_OrigenPorUsuario consulta)
        {
            var result = new Result<List<Resultado_OrigenPorUsuario>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = Resultado_OrigenPorUsuario.ToList(query.List().ToList());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }

            return result;
        } 
    }
}

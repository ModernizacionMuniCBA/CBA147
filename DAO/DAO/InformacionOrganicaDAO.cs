using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using Model.Consultas;
using NHibernate;


namespace DAO.DAO
{
    public class InformacionOrganicaDAO : BaseDAO<InformacionOrganica>
    {
        private static InformacionOrganicaDAO instance;

        public static InformacionOrganicaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InformacionOrganicaDAO();
                }
                return instance;
            }
        }

        public IQueryOver<InformacionOrganica, InformacionOrganica> GetQuery(Consulta_InformacionOrganica consulta)
        {
            var query = GetSession().QueryOver<InformacionOrganica>();

            if (consulta.Id.HasValue)
            {
                query.Where(x => x.Id == consulta.Id.Value);
            }

            if (consulta.IdArea.HasValue)
            {
                query.Where(x => x.Area.Id == consulta.IdArea.Value);
            }

            if (consulta.IdDireccion.HasValue)
            {
                query.Where(x => x.Direccion.Id == consulta.IdDireccion.Value);
            }

            if (consulta.IdSecretaria.HasValue)
            {
                query.Where(x => x.Direccion.Secretaria.Id == consulta.IdSecretaria.Value);
            }

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

        public Result<List<InformacionOrganica>> GetByFilters(Consulta_InformacionOrganica consulta)
        {
            var resultado = new Result<List<InformacionOrganica>>();
            try
            {
                resultado.Return = GetQuery(consulta).List().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<int> GetCantidadDuplicados(int? id, int idDireccion, int idArea, string nombre)
        {
            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<InformacionOrganica>();

                if (id.HasValue)
                {
                    query.Where(x => x.Id != id);
                }
                query.Where(x => x.Direccion.Id == idDireccion && x.Area.Id == idArea && x.FechaBaja == null);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        


    }
}

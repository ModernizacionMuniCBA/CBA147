using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using NHibernate.Criterion;
using Model.Entities;
using Model.Consultas;
using NHibernate;


namespace DAO.DAO
{
    public class SeccionDAO : BaseDAO<Seccion>
    {
        private static SeccionDAO instance;

        public static SeccionDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SeccionDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<int> GetCantidadDuplicados(int? id, string nombre, int? idArea)
        {
            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<Seccion>();

                if (id.HasValue)
                {
                    query.Where(x => x.Id != id);
                }
                query.Where(x => x.Nombre == nombre && x.Area.Id == idArea && x.FechaBaja==null);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
        private IQueryOver<Seccion, Seccion> GetQuery(Consulta_Seccion consulta)
        {
            var query = GetSession().QueryOver<Seccion>();

            //Nombre
            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                query.Where(x => x.Nombre == consulta.Nombre);
            }

            //Area
            if (consulta.IdsArea != null && consulta.IdsArea.Count != 0)
            {
                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id.IsIn(consulta.IdsArea));
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
        public Result<List<Seccion>> GetByFilters(Consulta_Seccion consulta)
        {
            var result = new Result<List<Seccion>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = new List<Seccion>(query.List());
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

        public Result<List<int>> GetIdsByFilters(Consulta_Seccion consulta)
        {
            var result = new Result<List<int>>();

            try
            {
                var query = GetQuery(consulta);
                query.Select(x => x.Id);
                result.Return = query.List<int>().ToList();
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

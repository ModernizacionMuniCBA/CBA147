using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Model;
using Model.Entities;
using NHibernate;
using NHibernate.Criterion;
using Model.Consultas;

namespace DAO.DAO
{
    public class PersonalDAO : BaseDAO<Personal>
    {
        private static PersonalDAO instance;

        public static PersonalDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new PersonalDAO();
                return instance;
            }
        }


        private IQueryOver<Personal, Personal> GetQuery(Consulta_Personal filtros)
        {
            var query = GetSession().QueryOver<Personal>();

            //Area
            if (filtros.idArea.HasValue)
            {
                query.Where(x => x.Area.Id == filtros.idArea.Value);
            }

            //persona fisica
            if (filtros.idPersonaFisica.HasValue)
            {
                query.Where(x => x.PersonaFisica.Id == filtros.idPersonaFisica.Value);
            }

            //Dado de baja
            if (filtros.dadosDeBaja.HasValue)
            {
                if (filtros.dadosDeBaja.Value)
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

        public Result<List<Personal>> GetByFilters(Consulta_Personal filtros)
        {
            var result = new Result<List<Personal>>();
            try
            {
                var query = GetQuery(filtros);
                result.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;

        }

        public Result<int> GetCantidadDuplicados(Personal personal)
        {

            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<Personal>();

                if (personal.Id != 0)
                {
                    query.Where(x => x.Id != personal.Id);
                }
                query.Where(x => x.Area.Id == personal.Area.Id &&  x.PersonaFisica.Id == personal.PersonaFisica.Id && x.FechaBaja == null);
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
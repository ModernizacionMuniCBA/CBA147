using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class EdificioMunicipalDAO : BaseDAO<EdificioMunicipal>
    {
        private static EdificioMunicipalDAO instance;

        public static EdificioMunicipalDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EdificioMunicipalDAO();
                }
                return instance;
            }
        }


        public Result<List<EdificioMunicipal>> GetByIdCategoria(int idCategoria)
        {
            var resultado = new Result<List<EdificioMunicipal>>();
            try
            {
                var query = GetSession().QueryOver<EdificioMunicipal>();
                query.JoinQueryOver(x => x.Categoria).Where(x => x.Id == idCategoria);
                query.Where(x => x.FechaBaja==null);
        
                resultado.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }
    }
}

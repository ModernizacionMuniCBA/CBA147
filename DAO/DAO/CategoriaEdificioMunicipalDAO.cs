using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class CategoriaEdificioMunicipalDAO : BaseDAO<CategoriaEdificioMunicipal>
    {
        private static CategoriaEdificioMunicipalDAO instance;

        public static CategoriaEdificioMunicipalDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CategoriaEdificioMunicipalDAO();
                }
                return instance;
            }
        }


    }
}

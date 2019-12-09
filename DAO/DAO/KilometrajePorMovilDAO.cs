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
    public class KilometrajePorMovilDAO : BaseDAO<KilometrajePorMovil>
    {
        private static KilometrajePorMovilDAO instance;

        public static KilometrajePorMovilDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new KilometrajePorMovilDAO();
                }
                return instance;
            }
        }


    }
}

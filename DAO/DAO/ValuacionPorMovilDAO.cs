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
    public class ValuacionPorMovilDAO : BaseDAO<ValuacionPorMovil>
    {
        private static ValuacionPorMovilDAO instance;

        public static ValuacionPorMovilDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ValuacionPorMovilDAO();
                }
                return instance;
            }
        }


    }
}

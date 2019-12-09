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
    public class TUVPorMovilDAO : BaseDAO<TUVPorMovil>
    {
        private static TUVPorMovilDAO instance;

        public static TUVPorMovilDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TUVPorMovilDAO();
                }
                return instance;
            }
        }


    }
}

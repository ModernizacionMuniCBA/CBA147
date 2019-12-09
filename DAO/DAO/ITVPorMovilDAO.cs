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
    public class ITVPorMovilDAO : BaseDAO<ITVPorMovil>
    {
        private static ITVPorMovilDAO instance;

        public static ITVPorMovilDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ITVPorMovilDAO();
                }
                return instance;
            }
        }


    }
}

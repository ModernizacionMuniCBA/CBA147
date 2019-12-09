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
    public class ReparacionPorMovilDAO : BaseDAO<ReparacionPorMovil>
    {
        private static ReparacionPorMovilDAO instance;

        public static ReparacionPorMovilDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReparacionPorMovilDAO();
                }
                return instance;
            }
        }


    }
}

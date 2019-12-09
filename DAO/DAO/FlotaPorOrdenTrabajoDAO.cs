using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using Model.Consultas;
using NHibernate;

namespace DAO.DAO
{
    public class FlotaPorOrdenTrabajoDAO : BaseDAO<FlotaPorOrdenTrabajo>
    {
        private static FlotaPorOrdenTrabajoDAO instance;

        public static FlotaPorOrdenTrabajoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FlotaPorOrdenTrabajoDAO();
                }
                return instance;
            }
        }

    }
}

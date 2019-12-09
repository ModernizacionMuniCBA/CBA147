using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Transform;
using Model.Consultas;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate;
using System.Text;
using System.Diagnostics;
using NHibernate.Criterion;
using Model.Utiles;

namespace DAO.DAO
{
    public class EmpleadoPorOrdenTrabajoDAO : BaseDAO<EmpleadoPorOrdenTrabajo>
    {
        private static EmpleadoPorOrdenTrabajoDAO instance;

        public static EmpleadoPorOrdenTrabajoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmpleadoPorOrdenTrabajoDAO();
                }
                return instance;
            }
        }

    }
}

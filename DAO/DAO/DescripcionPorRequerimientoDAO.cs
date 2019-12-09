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
    public class DescripcionPorRequerimientoDAO : BaseDAO<DescripcionPorRequerimiento>
    {
        private static DescripcionPorRequerimientoDAO instance;

        public static DescripcionPorRequerimientoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DescripcionPorRequerimientoDAO();
                }
                return instance;
            }
        }

    }
}

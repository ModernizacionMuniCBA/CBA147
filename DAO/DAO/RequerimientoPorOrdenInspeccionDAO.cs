using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class RequerimientoPorOrdenInspeccionDAO : BaseDAO<RequerimientoPorOrdenInspeccion>
    {
        private static RequerimientoPorOrdenInspeccionDAO instance;

        public static RequerimientoPorOrdenInspeccionDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequerimientoPorOrdenInspeccionDAO();
                }
                return instance;
            }
        }

    }
}
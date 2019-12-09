using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class CampoPorMotivoPorRequerimientoDAO : BaseDAO<CampoPorMotivoPorRequerimiento>
    {
        private static CampoPorMotivoPorRequerimientoDAO instance;

        public static CampoPorMotivoPorRequerimientoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CampoPorMotivoPorRequerimientoDAO();
                }
                return instance;
            }
        }

    }
}

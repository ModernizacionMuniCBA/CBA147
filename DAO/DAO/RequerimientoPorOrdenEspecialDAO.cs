using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class RequerimientoPorOrdenEspecialDAO : BaseDAO<RequerimientoPorOrdenEspecial>
    {
        private static RequerimientoPorOrdenEspecialDAO instance;

        public static RequerimientoPorOrdenEspecialDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RequerimientoPorOrdenEspecialDAO();
                }
                return instance;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class EstadoFlotaHistorialDAO : BaseDAO<EstadoFlotaHistorial>
    {
        private static EstadoFlotaHistorialDAO instance;

        public static EstadoFlotaHistorialDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EstadoFlotaHistorialDAO();
                }
                return instance;
            }
        }
    
    }
}

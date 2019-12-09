using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class TipoCampoPorMotivoDAO : BaseDAO<TipoCampo>
    {
        private static TipoCampoPorMotivoDAO instance;

        public static TipoCampoPorMotivoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TipoCampoPorMotivoDAO();
                }
                return instance;
            }
        }

    }
}

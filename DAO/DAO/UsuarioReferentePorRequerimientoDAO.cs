using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class UsuarioReferentePorRequerimientoDAO : BaseDAO<UsuarioReferentePorRequerimiento>
    {
        private static UsuarioReferentePorRequerimientoDAO instance;

        public static UsuarioReferentePorRequerimientoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UsuarioReferentePorRequerimientoDAO();
                }
                return instance;
            }
        }

    }
}

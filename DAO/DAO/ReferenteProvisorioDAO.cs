using Model;
using Model.Entities;
using System;
using System.Linq;

namespace DAO.DAO
{
    public class ReferenteProvisorioDAO : BaseDAO<ReferenteProvisorio>
    {
        private static ReferenteProvisorioDAO instance;

        public static ReferenteProvisorioDAO Instance
        {
            get {
                if (instance == null)
                    instance = new ReferenteProvisorioDAO();
                return instance;
            }         
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;


namespace DAO.DAO
{
    public class InformacionOrganicaSecretariaDAO : BaseDAO<InformacionOrganicaSecretaria>
    {
        private static InformacionOrganicaSecretariaDAO instance;

        public static InformacionOrganicaSecretariaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InformacionOrganicaSecretariaDAO();
                }
                return instance;
            }
        }


        public Result<int> GetCantidadDuplicados(int? id, string nombre)
        {
            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<InformacionOrganicaSecretaria>();

                if (id.HasValue)
                {
                    query.Where(x => x.Id != id);
                }
                query.Where(x => x.Nombre == nombre && x.FechaBaja == null);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

    }
}

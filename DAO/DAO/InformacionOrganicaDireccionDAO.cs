using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;


namespace DAO.DAO
{
    public class InformacionOrganicaDireccionDAO : BaseDAO<InformacionOrganicaDireccion>
    {
        private static InformacionOrganicaDireccionDAO instance;

        public static InformacionOrganicaDireccionDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InformacionOrganicaDireccionDAO();
                }
                return instance;
            }
        }


        public Result<int> GetCantidadDuplicados(int? id, int idSecretaria, string nombre)
        {
            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<InformacionOrganicaDireccion>();

                if (id.HasValue)
                {
                    query.Where(x => x.Id != id);
                }
                query.Where(x => x.Nombre == nombre && x.FechaBaja == null && x.Secretaria.Id == idSecretaria);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<InformacionOrganicaDireccion>> GetByIdSecretaria(int idSecretaria)
        {
            var result = new Result<List<InformacionOrganicaDireccion>>();
            try
            {
                var query = GetSession().QueryOver<InformacionOrganicaDireccion>();
                query.Where(x => x.FechaBaja == null).JoinQueryOver<InformacionOrganicaSecretaria>(x => x.Secretaria).Where(x => x.Id == idSecretaria);
                result.Return = query.List().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

    }
}

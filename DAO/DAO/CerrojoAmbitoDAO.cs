using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate;

namespace DAO.DAO
{
    public class CerrojoAmbitoDAO : BaseDAO<CerrojoAmbito>
    {
        private static CerrojoAmbitoDAO instance;

        public static CerrojoAmbitoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CerrojoAmbitoDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<CerrojoAmbito> GetByKeyValue(int keyValue)
        {
            var result = new Result<CerrojoAmbito>();

            try
            {
                var query = GetSession().QueryOver<CerrojoAmbito>();

                //Id Cerrojo
                query.Where(x => x.KeyValue == keyValue);

                //Traigo los los activos
                query.Where(x => x.FechaBaja == null);
                result.Return = query.SingleOrDefault();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;

        }


        public  Result<List<int>> GetIdsAreas (int id)
        {
            var resultado = new Result<List<int>>();

            IQuery query = GetSession().CreateSQLQuery("exec VecinoVirtualUsuariosPerfiladosPorCPC @id="+id);
            try
            {
                var data = query.List<int>().ToList();
                resultado.Return = data;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

    }
}

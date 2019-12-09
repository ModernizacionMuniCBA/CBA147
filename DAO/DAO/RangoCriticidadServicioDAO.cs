using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;

namespace DAO.DAO
{
    public class RangoCriticidadServicioDAO : BaseDAO<RangoCriticidadServicio>
    {
        private static RangoCriticidadServicioDAO instance;

        public static RangoCriticidadServicioDAO Instance
        {
            get
            {
                if (instance == null)
                {

                    instance = new RangoCriticidadServicioDAO();
                }
                return instance;
            }
        }

        public Result<List<RangoCriticidadServicio>> GetByIdServicio(int idServicio, bool? dadosDeBaja)
        {
            var result = new Result<List<RangoCriticidadServicio>>();

            try
            {
                var query = GetSession().QueryOver<RangoCriticidadServicio>();
                query.Where(x => x.IdServicio == idServicio);

                //Dados de baja
                if (dadosDeBaja.HasValue)
                {
                    if (dadosDeBaja.Value)
                    {
                        query.Where(x => x.FechaBaja != null);
                    }
                    else
                    {
                        query.Where(x => x.FechaBaja == null);
                    }
                }

                var rangos = query.List().ToList();
                if (rangos.Count == 0)
                {
                    query = GetSession().QueryOver<RangoCriticidadServicio>();
                    query.Where(x => x.IdServicio == null);
                    query.Where(x => x.FechaBaja == null);
                    rangos = query.List().ToList();
                }
                result.Return = rangos;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }
            return result;
        }

    }
}

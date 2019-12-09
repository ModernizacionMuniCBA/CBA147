using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;

namespace DAO.DAO
{
    public class TemaDAO : BaseDAO<Tema>
    {
        private static TemaDAO instance;

        public static TemaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TemaDAO();
                }
                return instance;
            }
        }

        public Result<List<Tema>> GetByFilters(int? idServicio, string nombre, bool? dadosDeBaja)
        {
            var result = new Result<List<Tema>>();

            try
            {
                var query = GetSession().QueryOver<Tema>();

                //Servicio
                if (idServicio.HasValue)
                {
                    query.JoinQueryOver<Servicio>(x => x.Servicio).Where(x => x.Id == idServicio.Value);
                }

                //Nombre
                if (!string.IsNullOrEmpty(nombre))
                {
                    query.Where(x => x.Nombre.IsLike(nombre, MatchMode.Anywhere));
                }

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
                result.Return = new List<Tema>(query.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.Message);
            }
            return result;
        }

      }
}

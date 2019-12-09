using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Transform;
using Model.Consultas;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate;
using System.Text;
using System.Diagnostics;
using NHibernate.Criterion;
using Model.Utiles;

namespace DAO.DAO
{
    public class TareaPorAreaDAO : BaseDAO<TareaPorArea>
    {
        private static TareaPorAreaDAO instance;

        public static TareaPorAreaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TareaPorAreaDAO();
                }
                return instance;
            }
        }

        public IQueryOver<TareaPorArea, TareaPorArea> GetQuery(List<int> idsArea, bool? dadosDeBaja)
        {
            var query = GetSession().QueryOver<TareaPorArea>();
            query.Where(x => x.Area.Id.IsIn(idsArea));

            if (dadosDeBaja.HasValue && dadosDeBaja.Value)
            {
                query.Where(x => x.FechaBaja != null);
            }
            else if (dadosDeBaja.HasValue && !dadosDeBaja.Value)
            {
                query.Where(x => x.FechaBaja == null);
            }

            return query;
        }

        public Result<List<TareaPorArea>> GetByIdsArea(List<int> idsArea, bool? dadosDeBaja)
        {
            var result = new Result<List<TareaPorArea>>();

            try
            {
                var query = GetQuery(idsArea, dadosDeBaja);
                var resultado = query.List().ToList();
                result.Return = resultado;
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<int> GetCantidadByArea(int idArea)
        {
            var result = new Result<int>();


            try
            {
                var idsArea = new List<int>();
                idsArea.Add(idArea);

                var query = GetQuery(idsArea, false);
                var resultado = query.RowCount();
                result.Return = resultado;
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public  Result<bool> Equals(TareaPorArea obj)
        {
            try
            {
                var result = new Result<bool>();
                var query = GetSession().QueryOver<TareaPorArea>();
                query.Where(x => x.Nombre.IsLike(obj.Nombre.ToUpper().Trim()));
                query.Where(x => x.FechaBaja == null);
                result.Return = query.List().Count != 0;
                return result;
            }
            catch (Exception e)
            {
                var result = new Result<bool>();
                result.AddErrorInterno(e.InnerException);
                return result;
            }
        }

    }
}

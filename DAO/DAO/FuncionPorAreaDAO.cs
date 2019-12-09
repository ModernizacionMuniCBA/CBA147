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
    public class FuncionPorAreaDAO : BaseDAO<FuncionPorArea>
    {
        private static FuncionPorAreaDAO instance;

        public static FuncionPorAreaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FuncionPorAreaDAO();
                }
                return instance;
            }
        }

        public Result<List<FuncionPorArea>> GetByIdsArea(List<int> idsArea, bool? dadosDeBaja)
        {
            var result = new Result<List<FuncionPorArea>>();

            try
            {
                var query = GetSession().QueryOver<FuncionPorArea>();
                query.Where(x => x.Area.Id.IsIn(idsArea));

                if (dadosDeBaja.HasValue && dadosDeBaja.Value)
                {
                    query.Where(x=>x.FechaBaja!=null);
                }
                else if (dadosDeBaja.HasValue && !dadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja == null);
                }

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

        public Result<bool> Equals(FuncionPorArea obj)
        {
            try
            {
                var result = new Result<bool>();
                var query = GetSession().QueryOver<FuncionPorArea>();
                query.Where(x=>x.Area==obj.Area);
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

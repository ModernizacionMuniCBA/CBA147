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
    public class FuncionPorEmpleadoDAO : BaseDAO<FuncionPorEmpleado>
    {
        private static FuncionPorEmpleadoDAO instance;

        public static FuncionPorEmpleadoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FuncionPorEmpleadoDAO();
                }
                return instance;
            }
        }

        public Result<List<Resultado_FuncionPorEmpleado>> GetByIdsEmpleados(List<int> idsEmpleados)
        {
            var result = new Result<List<Resultado_FuncionPorEmpleado>>();

            if (idsEmpleados.Count == 0)
            {
                return result;
            }

            try
            {
                var sb = new StringBuilder();
                var sql = @"
                    SELECT fxe.IdEmpleado as IdEmpleado,
                    fxe.IdFuncion as IdFuncion
                    from FuncionPorEmpleado fxe 
	                inner join (
 	                select -1 as Id2";
                sb.Append(sql);
                foreach (var id in idsEmpleados)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on fxe.IdEmpleado = x.Id2
                    WHERE fxe.FechaBaja is null 
	                ORDER BY fxe.FechaAlta DESC
	                ");

                var query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_FuncionPorEmpleado)));
                var resultado = query.List<Resultado_FuncionPorEmpleado>().ToList();
                if (resultado != null)
                {
                    result.Return = resultado;
                }
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
    }
}

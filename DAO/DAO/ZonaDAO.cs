using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Model;
using Model.Entities;
using Model.Resultados;
using Model.Consultas;
using NHibernate;
using System.Text;
using System.Diagnostics;
using NHibernate.Criterion;
using NHibernate.Transform;
using Model.Utiles;

namespace DAO.DAO
{
    public class ZonaDAO : BaseDAO<Zona>
    {
        private static ZonaDAO instance;

        public static ZonaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ZonaDAO();
                }
                return instance;
            }
        }

        public Result<int> GetCantidadDuplicados(int? id, string nombre, int? idArea)
        {
            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<Zona>();

                if (id.HasValue)
                {
                    query.Where(x => x.Id != id);
                }
                query.Where(x => x.Nombre == nombre && x.Area.Id == idArea && x.FechaBaja == null);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        private IQueryOver<Zona, Zona> GetQuery(Consulta_Zona consulta)
        {
            var query = GetSession().QueryOver<Zona>();

            //Nombre
            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                query.Where(x => x.Nombre == consulta.Nombre);
            }

            //Area
            if (consulta.IdsArea!=null && consulta.IdsArea.Count!=0)
            {
                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id.IsIn(consulta.IdsArea));
            }

            //Dados de baja
            if (consulta.DadosDeBaja.HasValue)
            {
                if (consulta.DadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }
            }


            return query;
        }

        public Result<List<Zona>> GetByFilters(Consulta_Zona consulta)
        {
            var result = new Result<List<Zona>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = new List<Zona>(query.List());
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

        public Result<List<int>> GetIdsByFilters(Consulta_Zona consulta)
        {
            var result = new Result<List<int>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = query.Select(x=>x.Id).List<int>().ToList();
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

        public Result<ResultadoTabla<ResultadoTabla_Zona>> GetResultadoTablaByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_Zona>>();

            if (ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_Zona>();
                resultadoTabla.Data = new List<ResultadoTabla_Zona>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_Zona>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoTabla_Zona>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                z.Id as Id,
	                z.Nombre as Nombre,
	                a.Nombre as AreaNombre,
	                a.Id as AreaId,
                    z.FechaBaja as FechaBaja
	                FROM
 	                Zona z
                    inner join CerrojoArea a on a.Id = z.IdAreaCerrojo
	                inner join (
 	                    select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on z.Id = x.Id2
	                ORDER BY z.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_Zona)));
                var resultado = query.List<ResultadoTabla_Zona>().ToList();
                if (resultado != null && resultado.Count != 0 && resultado[0].Id != 0)
                {
                    data.AddRange(resultado);
                }

                resultadoTabla.Data = data;
                resultadoTabla.Cantidad = ids.Count();
                result.Return = resultadoTabla;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<int?> GetColorDisponible(int idArea)
        {
            var resultado = new Result<int?>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Zona_ColorDisponible @id=:id");
                query.SetInt32("id", idArea);
                var data = query.List<int?>().ToList();
                if (data.Count == 0 || data[0] == null)
                {
                    resultado.Return = null;
                }
                else
                {
                    resultado.Return = data[0];
                }

                return resultado;

            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;
        }
    }


}

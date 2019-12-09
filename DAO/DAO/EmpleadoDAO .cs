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
    public class EmpleadoDAO : BaseDAO<EmpleadoPorArea>
    {
        private static EmpleadoDAO instance;

        public static EmpleadoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmpleadoDAO();
                }
                return instance;
            }
        }

        private IQueryOver<EmpleadoPorArea, EmpleadoPorArea> GetQuery(Consulta_Empleado consulta)
        {
            var query = GetSession().QueryOver<EmpleadoPorArea>();

            //Areas
            if (consulta.IdArea.HasValue && consulta.IdArea > 0)
            {
                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id == consulta.IdArea && x.FechaBaja==null);
            }

            //OT
            if (consulta.IdOT.HasValue && consulta.IdOT > 0)
            {
                query.JoinQueryOver<EmpleadoPorOrdenTrabajo>(x => x.OrdenesTrabajo).Where(x => x.OrdenTrabajo.Id == consulta.IdOT && x.FechaBaja == null);
            }

            //Estados
            if (consulta.Estados != null && consulta.Estados.Count != 0)
            {
                query.JoinQueryOver<EstadoEmpleadoHistorial>(x => x.Estados).Where(x => x.Ultimo == true).JoinQueryOver<EstadoEmpleado>(x => x.Estado).Where(x => x.KeyValue.IsIn(consulta.Estados));
            }

            //Usuario
            if (consulta.IdUsuario.HasValue)
            {
                query.JoinQueryOver<_VecinoVirtualUsuario>(x => x.UsuarioEmpleado).Where(z => z.Id == consulta.IdUsuario.Value);
            }

            //Tiene seccion?
            if (consulta.Seccion.HasValue)
            {
                if (consulta.Seccion.Value)
                {
                    query.Where(x => x.Seccion != null);
                }
                else
                {
                    query.Where(x => x.Seccion == null);
                }
            }


            //Tiene flota?
            if (consulta.Flota.HasValue)
            {
                if (consulta.Flota.Value)
                {
                    query.Where(x => x.FlotaActiva!=null);
                }
                else
                {
                    query.Where(x => x.FlotaActiva == null);
                }
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

        public Result<int> GetCantidadByFilters(Consulta_Empleado consulta)
        {
            var result = new Result<int>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<int>> GetIdsByFilters(Consulta_Empleado consulta)
        {
            var result = new Result<List<int>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = query.Select(x => x.Id).List<int>().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<ResultadoTabla<ResultadoTabla_Empleado>> GetResultadoTablaByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_Empleado>>();
            if (ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_Empleado>();
                resultadoTabla.Data = new List<ResultadoTabla_Empleado>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_Empleado>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoTabla_Empleado>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                e.Id as Id,
                    u.Id as IdUsuarioCerrojoEmpleado,
                    u.Nombre as Nombre,
                    u.Apellido as Apellido,
                    e.FechaAlta as FechaAlta,
                    u.Dni as Dni,
                    s.Nombre as NombreSeccion, 
                    s.Id as IdSeccion,
                    u.SexoMasculino as SexoMasculino 

                    from EmpleadoPorArea e inner join VecinoVirtualUsuario u on e.IdUsuarioCerrojoEmpleado=u.Id
                    left join Seccion s on s.Id=e.IdSeccion
                    
	                inner join (
 	                select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on e.Id = x.Id2
                    WHERE u.FechaBaja is null
	                ORDER BY e.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_Empleado)));
                var resultado = query.List<ResultadoTabla_Empleado>().ToList();
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

        public Result<ResultadoTabla<ResultadoTabla_EmpleadoPanel>> GetResultadoTablaPanelByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_EmpleadoPanel>>();
            if (ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_EmpleadoPanel>();
                resultadoTabla.Data = new List<ResultadoTabla_EmpleadoPanel>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_EmpleadoPanel>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoTabla_EmpleadoPanel>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                        e.Id as Id,
                            u.Id as IdUsuarioCerrojoEmpleado,
                            u.Nombre as Nombre,
                            u.Apellido as Apellido,
                            u.Dni as Dni,
                            s.Nombre as NombreSeccion, 
                            s.Id as IdSeccion,
                            u.Cargo as Cargo,
                            er.Nombre as EstadoNombre,
                            cast(er.KeyValue as int) as EstadoKeyValue,
                            erh.Id as EstadoId,
                            er.Color as EstadoColor,
                            u.IdentificadorFotoPersonal as IdentificadorFotoPersonal,
                            u.SexoMasculino as SexoMasculino,
                	        
	CASE
WHEN subquery.FechaComienzoTrabajo > subqueryFlota.FechaComienzoTrabajoFlota THEN
	subquery.NombreEstadoOrdenTrabajo
ELSE
	subqueryFlota.NombreEstadoOrdenTrabajoFlota
END AS NombreEstadoOrdenTrabajo,

	CASE
WHEN subquery.FechaComienzoTrabajo > subqueryFlota.FechaComienzoTrabajoFlota THEN
	subquery.NumeroOrdenTrabajo
ELSE
	subqueryFlota.NumeroOrdenTrabajoFlota
END AS NumeroOrdenTrabajo,

	CASE
WHEN subquery.FechaComienzoTrabajo > subqueryFlota.FechaComienzoTrabajoFlota THEN
	subquery.IdOrdenTrabajo
ELSE
	subqueryFlota.IdOrdenTrabajoFlota
END AS IdOrdenTrabajo,

	CASE
WHEN subquery.FechaComienzoTrabajo > subqueryFlota.FechaComienzoTrabajoFlota THEN
	subquery.FechaComienzoTrabajo
ELSE
	subqueryFlota.FechaComienzoTrabajoFlota
END AS FechaComienzoTrabajo,

 (
	SELECT DISTINCT
		SUBSTRING (
			(
				SELECT
					', ' + f2.Nombre AS [text()]
				FROM
					FuncionPorArea f2
				INNER JOIN FuncionPorEmpleado fxe2 ON f2.Id = fxe2.IdFuncion
				WHERE
					e.Id = fxe2.IdEmpleado
				AND fxe2.FechaBaja IS NULL
				AND f2.FechaBaja IS NULL FOR XML PATH ('')
			),
			3,
			5000
		) [Funciones]
	FROM
		FuncionPorEmpleado fxe
	INNER JOIN FuncionPorArea f ON fxe.IdFuncion = f.Id
	INNER JOIN EmpleadoPorArea e2 ON e2.Id = fxe.IdEmpleado
	WHERE
		e2.Id = e.Id
) AS Funciones
FROM
	EmpleadoPorArea e
INNER JOIN VecinoVirtualUsuario u ON e.IdUsuarioCerrojoEmpleado = u.Id
LEFT JOIN Seccion s ON s.Id = e.IdSeccion
INNER JOIN EstadoEmpleadoHistorial erh ON erh.IdEmpleado = e.Id
AND erh.Ultimo = 1
INNER JOIN EstadoEmpleado er ON erh.IdEstado = er.Id
LEFT JOIN (
	SELECT
		exot2.IdEmpleado AS IdEmpleadoSubquery,
		ot3.Id AS IdOrdenTrabajo,
		eot3.Nombre AS NombreEstadoOrdenTrabajo,
		concat (ot3.Numero, '/', ot3.Anio) AS NumeroOrdenTrabajo,
		eoth.FechaAlta AS FechaComienzoTrabajo
	FROM
		OrdenTrabajo ot3
	INNER JOIN EstadoOrdenTrabajoHistorial eoth ON eoth.IdOrdenTrabajo = ot3.Id
	INNER JOIN EstadoOrdenTrabajo eot3 ON eot3.Id = eoth.IdEstado
	AND eoth.Ultimo = 1
	INNER JOIN EmpleadoPorOrdenTrabajo exot2 ON exot2.IdOrdenTrabajo = ot3.Id
	WHERE
		ot3.Id = (
			SELECT
				TOP 1 exa.IdOrdenTrabajo
			FROM
				EmpleadoPorOrdenTrabajo exa
			WHERE
				exa.IdEmpleado = exot2.IdEmpleado
			AND exa.FechaBaja IS NULL
			ORDER BY
				exa.FechaAlta DESC
		)
	AND exot2.FechaBaja IS NULL
) subquery ON subquery.IdEmpleadoSubquery = e.Id

LEFT JOIN (
	SELECT
		exf4.IdEmpleado AS IdEmpleadoSubqueryOTFlota,
		ot4.Id AS IdOrdenTrabajoFlota,
		eot4.Nombre AS NombreEstadoOrdenTrabajoFlota,
		concat (ot4.Numero, '/', ot4.Anio) AS NumeroOrdenTrabajoFlota,
		eoth4.FechaAlta AS FechaComienzoTrabajoFlota
	FROM
		OrdenTrabajo ot4
	INNER JOIN EstadoOrdenTrabajoHistorial eoth4 ON eoth4.IdOrdenTrabajo = ot4.Id
	INNER JOIN EstadoOrdenTrabajo eot4 ON eot4.Id = eoth4.IdEstado
	AND eoth4.Ultimo = 1
	INNER JOIN FlotaPorOrdenTrabajo fxot4 ON fxot4.IdOrdenTrabajo = ot4.Id
	INNER JOIN Flota f4 ON f4.Id = fxot4.IdFlota
	INNER JOIN EmpleadoPorFlota exf4 ON exf4.IdFlota = f4.Id
	WHERE
		ot4.Id = (
			SELECT
				TOP 1 exa.IdOrdenTrabajo
			FROM
				FlotaPorOrdenTrabajo exa
			WHERE
				exa.IdFlota = f4.Id
			AND exa.FechaBaja IS NULL
			ORDER BY
				exa.FechaAlta DESC
		)
	AND fxot4.FechaBaja IS NULL
	AND f4.FechaBaja IS NULL
	AND exf4.FechaBaja IS NULL
) subqueryFlota ON subqueryFlota.IdEmpleadoSubqueryOTFlota = e.Id

	                        inner join (
 	                        select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                        as x on e.Id = x.Id2
                            WHERE e.FechaBaja is null and u.FechaBaja is null
	                        ORDER BY e.FechaAlta DESC
	                        ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_EmpleadoPanel)));
                var resultado = query.List<ResultadoTabla_EmpleadoPanel>().ToList();
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


        public Result<ResultadoTabla<ResultadoTabla_EmpleadoPanel>> GetResultadoTablaPanelByIds2(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_EmpleadoPanel>>();
            if (ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_EmpleadoPanel>();
                resultadoTabla.Data = new List<ResultadoTabla_EmpleadoPanel>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_EmpleadoPanel>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoTabla_EmpleadoPanel>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                e.Id as Id,
                    u.Id as IdUsuarioCerrojoEmpleado,
                    u.Nombre as Nombre,
                    u.Apellido as Apellido,
                    u.Dni as Dni,
                    s.Nombre as NombreSeccion, 
                    s.Id as IdSeccion,
                    u.Cargo as Cargo,
                    er.Nombre as EstadoNombre,
                    cast(er.KeyValue as int) as EstadoKeyValue,
                    erh.Id as EstadoId,
                    er.Color as EstadoColor,
                    u.IdentificadorFotoPersonal as IdentificadorFotoPersonal,
                    u.SexoMasculino as SexoMasculino,
                	(Select concat(ot.Numero, '/',ot.Anio) as NumeroOrdenTrabajo
                        from OrdenTrabajo ot where ot.Id = 
                    	(
                		SELECT top 1
	            	    	exa.IdOrdenTrabajo
	                	FROM
	                		EmpleadoPorOrdenTrabajo exa
		                WHERE
	                		exa.IdEmpleado = e.Id
		                    AND FechaBaja IS NULL
		                    order by FechaAlta desc
	                    )) as NumeroOrdenTrabajo , 
                	(Select ot2.Id 
                        from OrdenTrabajo ot2 where ot2.Id = 
                    	(
                		SELECT top 1
	            	    	exa.IdOrdenTrabajo
	                	FROM
	                		EmpleadoPorOrdenTrabajo exa
		                WHERE
	                		exa.IdEmpleado = e.Id
		                    AND FechaBaja IS NULL
		                    order by FechaAlta desc
	                    )) as IdOrdenTrabajo , 
                    (select eoth.FechaAlta
                        from OrdenTrabajo ot3 
                        inner join EstadoOrdenTrabajoHistorial eoth on eoth.IdOrdenTrabajo=ot3.Id and eoth.Ultimo=1
                        where ot3.Id=(
                		SELECT top 1
	            	    	exa.IdOrdenTrabajo
	                	FROM
	                		EmpleadoPorOrdenTrabajo exa
		                WHERE
	                		exa.IdEmpleado = e.Id
		                    AND FechaBaja IS NULL
		                    order by FechaAlta desc
	                    )) as  FechaComienzoTrabajo, 
                        (
                        SELECT DISTINCT
                    	SUBSTRING (
	                	(
	            		SELECT
	            			', ' + f2.Nombre AS [text()]
	            		FROM
			            	FuncionPorArea f2
			            INNER JOIN FuncionPorEmpleado fxe2 ON f2.Id = fxe2.IdFuncion
			            WHERE
			            	e.Id = fxe2.IdEmpleado
			            AND fxe2.FechaBaja IS NULL
			            AND f2.FechaBaja IS NULL FOR XML PATH ('')
		                ),
	                	3,
	                	5000
	                    ) [Funciones]
                        FROM
	                        FuncionPorEmpleado fxe
                        INNER JOIN FuncionPorArea f ON fxe.IdFuncion = f.Id
                        INNER JOIN EmpleadoPorArea e2 ON e2.Id = fxe.IdEmpleado
                        where e2.Id=e.Id
                        ) as Funciones
                    from EmpleadoPorArea e inner join VecinoVirtualUsuario u on e.IdUsuarioCerrojoEmpleado=u.Id
                    left join Seccion s on s.Id=e.IdSeccion
                    inner join EstadoEmpleadoHistorial erh on erh.IdEmpleado = e.Id and erh.Ultimo = 1
                    inner join EstadoEmpleado er on erh.IdEstado = er.Id
	                inner join (
 	                select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on e.Id = x.Id2
                    WHERE e.FechaBaja is null and u.FechaBaja is null
	                ORDER BY e.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_EmpleadoPanel)));
                var resultado = query.List<ResultadoTabla_EmpleadoPanel>().ToList();
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

        public Result<ResultadoTabla<ResultadoTabla_EmpleadoPanelOT>> GetResultadoTablaPanelOTByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_EmpleadoPanelOT>>();
            if (ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_EmpleadoPanelOT>();
                resultadoTabla.Data = new List<ResultadoTabla_EmpleadoPanelOT>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_EmpleadoPanelOT>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoTabla_EmpleadoPanelOT>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                o.Id,
                    o.Numero +' '+o.Año as Numero
                    from OrdenTrabajo o
                    inner join EmpleadoPorOrdenTrabajo exot on exot.IdEmpleado=e.Id
                    inner join (
                     select username, max(date) as MaxDate
                      from MyTable
                     group by username where idempleado=
                    ) tm on t.username = tm.username and t.date = tm.MaxDate
	                inner join (
 	                select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on e.Id = x.Id2
                    WHERE e.FechaBaja is null and u.FechaBaja is null
	                ORDER BY e.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_EmpleadoPanelOT)));
                var resultado = query.List<ResultadoTabla_EmpleadoPanelOT>().ToList();
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

        public Result<Resultado_EmpleadoPorArea> GetDetalleById(int id)
        {
            var resultado = new Result<Resultado_EmpleadoPorArea>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Empleado_Detalle @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_EmpleadoPorArea>());
                query.SetInt32("id", id);
                var data = query.List<Resultado_EmpleadoPorArea>().ToList();
                if (data.Count == 0 || data[0].Id == 0)
                {
                    resultado.Return = null;
                }
                else
                {
                    resultado.Return = data[0];
                }
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Model.Resultados.Resultado_EmpleadoPorArea.Resultado_Empleado_HistoricoEstados>> GetDetalleHistorialEstadosById(int id)
        {
            var resultado = new Result<List<Model.Resultados.Resultado_EmpleadoPorArea.Resultado_Empleado_HistoricoEstados>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Empleado_Detalle_HistoricoEstados @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Model.Resultados.Resultado_EmpleadoPorArea.Resultado_Empleado_HistoricoEstados>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Model.Resultados.Resultado_EmpleadoPorArea.Resultado_Empleado_HistoricoEstados>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Model.Resultados.Resultado_EmpleadoPorArea.Resultado_FuncionPorEmpleado>> GetDetalleFuncionesById(int id)
        {
            var resultado = new Result<List<Model.Resultados.Resultado_EmpleadoPorArea.Resultado_FuncionPorEmpleado>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Empleado_Detalle_Funciones @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Model.Resultados.Resultado_EmpleadoPorArea.Resultado_FuncionPorEmpleado>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Model.Resultados.Resultado_EmpleadoPorArea.Resultado_FuncionPorEmpleado>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

    }
}

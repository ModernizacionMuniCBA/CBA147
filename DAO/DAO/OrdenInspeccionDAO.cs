using System;
using System.Collections.Generic;
using System.Linq;
using Model.Entities;
using Model;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Model.Resultados;
using Model.Consultas;
using System.Text;

namespace DAO.DAO
{
    public class OrdenInspeccionDAO : BaseDAO<OrdenInspeccion>
    {
        private static OrdenInspeccionDAO instance;

        public static OrdenInspeccionDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new OrdenInspeccionDAO();
                return instance;
            }
        }

        public Result<bool> ExisteNumero(string numero, int año)
        {
            var result = new Result<bool>();

            try
            {
                var query = GetSession().QueryOver<OrdenInspeccion>();
                query.Where(x => x.Numero == numero);
                query.Where(x => x.Año == año);
                result.Return = query.RowCount() != 0;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

//        private IQueryOver<OrdenTrabajo, OrdenTrabajo> GetQuery(List<int> idAreas, int? idZona, int? idSeccion, List<Enums.EstadoOrdenTrabajo> estados, List<int> idServicios, string numero, int? año, DateTime? fechaDesde, DateTime? fechaHasta, bool? dadosDeBaja)
//        {
//            var query = GetSession().QueryOver<OrdenTrabajo>();

//            //Areas
//            if (idAreas != null && idAreas.Count != 0)
//            {
//                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id.IsIn(idAreas));
//            }

//            if (idZona.HasValue)
//            {
//                query.JoinQueryOver<Zona>(x => x.Zona).Where(x => x.Id == idZona.Value);
//            }

//            if (idSeccion.HasValue)
//            {
//                query.JoinQueryOver<Seccion>(x => x.Seccion).Where(x => x.Id == idSeccion.Value);
//            }

//            //Estados
//            if (estados != null && estados.Count != 0)
//            {
//                query.JoinQueryOver<EstadoOrdenTrabajoHistorial>(x => x.Estados).Where(x => x.Ultimo == true).JoinQueryOver<EstadoOrdenTrabajo>(x => x.Estado).Where(x => x.KeyValue.IsIn(estados));
//            }

//            //Servicios
//            if (idServicios != null && idServicios.Count != 0)
//            {
//                //query.JoinQueryOver<Area>(x => x.Area).JoinQueryOver<Motivo>(x => x.Motivos).JoinQueryOver<Tema>(x => x.Tema).JoinQueryOver<Servicio>(x => x.Servicio).Where(x => x.Id.IsIn(idServicios));
//                query.JoinQueryOver<RequerimientoPorOrdenTrabajo>(x => x.RequerimientosPorOrdenTrabajo).JoinQueryOver<Requerimiento>(x => x.Requerimiento).JoinQueryOver<Motivo>(x => x.Motivo).JoinQueryOver<Tema>(x => x.Tema).JoinQueryOver<Servicio>(x => x.Servicio).Where(x => x.Id.IsIn(idServicios))
//                    .TransformUsing(Transformers.DistinctRootEntity);
//            }

//            //Numero
//            if (numero != null)
//            {
//                query.Where(x => x.Numero == numero);
//            }

//            //Año
//            if (año.HasValue)
//            {
//                query.Where(x => x.Año == año.Value);
//            }

//            //Fechas
//            if (fechaDesde.HasValue && fechaHasta.HasValue)
//            {
//                query.Where(x => x.FechaAlta >= fechaDesde.Value && x.FechaAlta <= fechaHasta.Value);
//            }

//            //Dado de baja
//            if (dadosDeBaja.HasValue)
//            {
//                if (dadosDeBaja.Value)
//                {
//                    query.Where(x => x.FechaBaja != null);
//                }
//                else
//                {
//                    query.Where(x => x.FechaBaja == null);
//                }
//            }
//            return query;
//        }

//        private IQueryOver<OrdenTrabajo, OrdenTrabajo> GetQuery(Consulta_OrdenTrabajo consulta)
//        {
//            var query = GetSession().QueryOver<OrdenTrabajo>();

//            //Numero
//            if (consulta.Numero != null && consulta.Numero != "")
//            {
//                query.Where(x => x.Numero.IsLike(consulta.Numero, MatchMode.Exact));
//            }

//            //Año
//            if (consulta.Año.HasValue)
//            {
//                query.Where(x => x.Año == consulta.Año.Value);
//            }

//            //Movil
//            if (consulta.IdMovil.HasValue)
//            {
//                query.JoinQueryOver<MovilPorOrdenTrabajo>(x => x.MovilesPorOrdenTrabajo).Where(x=>x.FechaBaja==null).JoinQueryOver<Movil>(x => x.Movil).Where(x => x.Id == (int)consulta.IdMovil);
//            }

//            //Areas
//            if (consulta.IdArea.HasValue)
//            {
//                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id == consulta.IdArea.Value);
//            }

//            //Zonas
//            if (consulta.IdZona.HasValue)
//            {
//                var idsBarrios = GetSession().QueryOver<BarrioPorZona>().Where(x => x.Zona.Id == consulta.IdZona.Value).Select(x => x.Barrio.Id).List<int>().ToList();
//                var joinRequerimiento = query.JoinQueryOver<RequerimientoPorOrdenTrabajo>(x => x.RequerimientosPorOrdenTrabajo).JoinQueryOver<Requerimiento>(x => x.Requerimiento).Where(x => x.FechaBaja == null);
//                var joinDomicilio = joinRequerimiento.JoinQueryOver<Domicilio>(x => x.Domicilio).JoinQueryOver<Barrio>(x => x.Barrio);
//                joinDomicilio.Where(x => x.Id.IsIn(idsBarrios));
//            }

//            //Seccion
//            if (consulta.IdSeccion.HasValue)
//            {
//                query.JoinQueryOver<Seccion>(x => x.Seccion).Where(x => x.Id == consulta.IdSeccion.Value);
//            }

//            //Estados
//            if (consulta.EstadosKeyValue != null && consulta.EstadosKeyValue.Count != 0)
//            {
//                query.JoinQueryOver<EstadoOrdenTrabajoHistorial>(x => x.Estados).Where(x => x.Ultimo == true).JoinQueryOver<EstadoOrdenTrabajo>(x => x.Estado).Where(x => x.KeyValue.IsIn(consulta.EstadosKeyValue));
//            }

//            //Fechas
//            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
//            {
//                query.Where(x => x.FechaAlta >= consulta.FechaDesde.Value && x.FechaAlta <= consulta.FechaHasta.Value);
//            }

//            //Dado de baja
//            if (consulta.DadosDeBaja.HasValue)
//            {
//                if (consulta.DadosDeBaja.Value)
//                {
//                    query.Where(x => x.FechaBaja != null);
//                }
//                else
//                {
//                    query.Where(x => x.FechaBaja == null);
//                }
//            }
//            return query;
//        }

//        //Reportes
//        public Result<List<Object[]>> GetReporteListado(List<int> ids)
//        {
//            var r = ids.ToString();

//            var result = new Result<List<object[]>>();

//            try
//            {
//                var idsString = "(" + string.Join(",", ids) + ")";

//                var sql = @"
//                      SELECT
//                        (ot.Numero + '/' + CONVERT(varchar, ot.Anio)) AS numero,
//                        ot.FechaAlta AS fecha,
//                        ot.IdAreaCerrojo AS idArea,
//                        a.Nombre AS area,
//                        e.Nombre as estado
//
//                        FROM
//                        dbo.OrdenTrabajo AS ot
//                        INNER JOIN AreaCerrojo a ON ot.IdAreaCerrojo  = a.Id
//                        INNER JOIN EstadoOrdenTrabajoHistorial eoth ON eoth.IdOrdenTrabajo = ot.Id
//                        INNER JOIN EstadoOrdenTrabajo e ON eoth.IdEstado = e.Id
//                        where eoth.Ultimo = 1 AND
//                        ot.Id in " + idsString;
//                sql += " ORDER BY ot.FechaAlta DESC";


//                var query = GetSession().CreateSQLQuery(sql);

//                result.Return = query.List<object[]>().ToList();
//            }
//            catch (Exception e)
//            {
//                result.AddErrorInterno(e);
//            }

//            return result;
//        }

//        public Result<List<Object[]>> GetReporteDetalleOrden(int idOrden)
//        {
//            var result = new Result<List<object[]>>();

//            try
//            {

//                var sql = @"
//                        SELECT
//                        (r.Numero + '/' + CONVERT(varchar, r.Anio)) AS numero,
//                        m.Nombre as motivo,
//                        r.FechaAlta as fechaReclamo,
//                        nr.Observaciones as nota,
//                        nr.FechaAlta as fechaNota,
//                        rot.Id as idRequerimiento,
//                        (c.Nombre + ' ' + CONVERT(varchar, d.Altura )+ ' - Bº ' + b.Nombre) as direccion,
//                        r.DomicilioManual as domicilioManual,
//						d.Observaciones as detalleDomicilio,
//						r.Descripcion as detalleMotivo,
//                        b.Nombre as barrio
//                        FROM
//                        RequerimientoPorOrdenTrabajo rot
//                        INNER JOIN Requerimiento r ON rot.IdRequerimiento = r.Id
//                        INNER JOIN Motivo m ON r.IdMotivo = m.Id
//                        LEFT JOIN NotaPorRequerimiento nr ON nr.IdRequerimiento = r.Id
//                        LEFT JOIN Domicilio d ON r.IdDomicilio = d.Id
//                        LEFT JOIN Calle c ON d.IdCalle = c.Id
//                        LEFT JOIN Barrio b ON d.IdBarrio = b.Id
//                        WHERE
//                        rot.IdOrdenTrabajo = " + idOrden;
//                sql += "ORDER BY r.FechaAlta";




//                var query = GetSession().CreateSQLQuery(sql);

//                result.Return = query.List<object[]>().ToList();
//            }
//            catch (Exception e)
//            {
//                result.AddErrorInterno(e);
//            }

//            return result;
//        }

//        public Result<List<int>> GetIds(Consulta_OrdenTrabajo consulta)
//        {
//            var result = new Result<List<int>>();

//            try
//            {
//                var query = GetQuery(consulta);
//                query.Select(x => x.Id).SelectList(list => list.SelectGroup((x => x.Id)));
//                result.Return = query.List<int>().ToList();
//            }
//            catch (Exception e)
//            {
//                result.AddErrorInterno(e);
//            }
//            return result;
//        }

//        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetUltimos(int cantidad, Consulta_OrdenTrabajo consulta)
//        {
//            var result = new Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>>();

//            try
//            {
//                var query = GetQuery(consulta);
//                query.Select(x => x.Id).SelectList(list => list.SelectGroup((x => x.Id)));
//                query.OrderBy(x => x.FechaAlta).Desc();
//                var ultimos = query.Take(cantidad).List<int>().ToList();

//                return GetResultadoTablaByIds(5, ultimos);
//            }
//            catch (Exception e)
//            {
//                result.AddErrorInterno(e);
//                return result;
//            }
//        }

//        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetAntiguas(int cantidad, Consulta_OrdenTrabajo consulta)
//        {
//            var result = new Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>>();

//            try
//            {
//                var query = GetQuery(consulta);
//                query.Select(x => x.Id);
//                query.OrderBy(x => x.FechaAlta).Asc();
//                var ultimos = query.Take(cantidad).List<int>().ToList();

//                return GetResultadoTablaByIds(5, ultimos);
//            }
//            catch (Exception e)
//            {
//                result.AddErrorInterno(e);
//                return result;
//            }
//        }
//        public Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>> GetResultadoTablaByIds(int limite, List<int> ids)
//        {
//            var result = new Result<ResultadoTabla<ResultadoTabla_OrdenTrabajo>>();

//            if (ids.Count == 0)
//            {
//                var resultadoTabla = new ResultadoTabla<ResultadoTabla_OrdenTrabajo>();
//                resultadoTabla.Data = new List<ResultadoTabla_OrdenTrabajo>();
//                resultadoTabla.CantidadMaxima = limite;
//                result.Return = resultadoTabla;
//                return result;
//            }

//            try
//            {
//                var resultadoTabla = new ResultadoTabla<ResultadoTabla_OrdenTrabajo>();
//                resultadoTabla.CantidadMaxima = limite;

//                var data = new List<ResultadoTabla_OrdenTrabajo>();

//                var sb = new StringBuilder();
//                var sql = @"      
//                            SELECT TOP " + @limite + @"
//                            o.Id as Id, 
//                            o.FechaAlta as FechaAlta, 
//                                 (o.Numero + '/' + CONVERT(varchar, o.Anio)) as Numero, 
//
//                            a.Id as AreaId, 
//                            a.Nombre as AreaNombre,
//
//                            er.Nombre as EstadoNombre,
//                            cast(er.KeyValue as int) as EstadoKeyValue,
//                            erh.Id as EstadoId,
//                            er.Color as EstadoColor,
//                            DATEDIFF(DAY, erh.FechaAlta , GETDATE()) as Dias,
//
//                            s.Id as SeccionId,
//                            s.Nombre as SeccionNombre,
//
//                            o.Descripcion as Descripcion
//
//                            from OrdenTrabajo o
//                            
//                            left join Seccion s on s.Id=o.IdSeccion 
//    
//                            inner join CerrojoArea a on a.Id = o.IdAreaCerrojo
//
//                            inner join EstadoOrdenTrabajoHistorial erh on erh.IdOrdenTrabajo = o.Id and erh.Ultimo = 1
//                            inner join EstadoOrdenTrabajo er on erh.IdEstado = er.Id
//												
//                            inner join (
// 	                        select -1 as Id2 ";
//                sb.Append(sql);
//                foreach (var id in ids)
//                {
//                    sb.Append(" union all select " + id + " ");
//                }
//                sb.Append(@") 
//	                      as x on o.Id = x.Id2
//	                    WHERE erh.Ultimo = 1 AND (erh.Ultimo = 1 or erh.Ultimo is null)
//	                    ORDER BY Dias DESC
//	                 ");

//                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
//                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_OrdenTrabajo)));
//                var resultado = query.List<ResultadoTabla_OrdenTrabajo>().ToList();
//                if (resultado != null && resultado.Count != 0 && resultado[0].Id != 0)
//                {
//                    data.AddRange(resultado);
//                }

//                resultadoTabla.Data = data;
//                resultadoTabla.Cantidad = ids.Count();
//                result.Return = resultadoTabla;
//                return result;

//            }
//            catch (Exception e)
//            {
//                result.AddErrorInterno(e);
//            }

//            return result;
//        }

//        //Detalle Orden Trabajo

//        public Result<Resultado_OrdenTrabajoDetalle> GetDetalleById(int id, int? idUsuario)
//        {
//            var resultado = new Result<Resultado_OrdenTrabajoDetalle>();

//            try
//            {
//                IQuery query = GetSession().CreateSQLQuery("exec OrdenTrabajo_Detalle @id=:id");
//                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenTrabajoDetalle>());
//                query.SetInt32("id", id);
//                var data = query.List<Resultado_OrdenTrabajoDetalle>().ToList();
//                if (data.Count == 0 || data[0].Id == 0)
//                {
//                    resultado.Return = null;
//                }
//                else
//                {
//                    resultado.Return = data[0];
//                }

//            }
//            catch (Exception e)
//            {
//                resultado.AddErrorInterno(e);
//            }
//            return resultado;
//        }

//        public Result<List<Resultado_OrdenTrabajoDetalle_EstadoHistorial>> GetDetalleHistorialEstadosById(int id)
//        {
//            var resultado = new Result<List<Resultado_OrdenTrabajoDetalle_EstadoHistorial>>();

//            try
//            {
//                IQuery query = GetSession().CreateSQLQuery("exec OrdenTrabajo_Detalle_HistoricoEstados @id=:id");
//                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenTrabajoDetalle_EstadoHistorial>());
//                query.SetInt32("id", id);
//                resultado.Return = query.List<Resultado_OrdenTrabajoDetalle_EstadoHistorial>().ToList();
//            }
//            catch (Exception e)
//            {
//                resultado.AddErrorInterno(e);
//            }
//            return resultado;
//        }

//        public Result<List<Resultado_OrdenTrabajoDetalle_Nota>> GetDetalleNotasById(int id)
//        {
//            var resultado = new Result<List<Resultado_OrdenTrabajoDetalle_Nota>>();

//            try
//            {
//                IQuery query = GetSession().CreateSQLQuery("exec OrdenTrabajo_Detalle_Notas @id=:id");
//                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenTrabajoDetalle_Nota>());
//                query.SetInt32("id", id);
//                resultado.Return = query.List<Resultado_OrdenTrabajoDetalle_Nota>().ToList();
//            }
//            catch (Exception e)
//            {
//                resultado.AddErrorInterno(e);
//            }
//            return resultado;
//        }

//        public Result<List<Resultado_OrdenTrabajoDetalle_Requerimiento>> GetDetalleRequerimientosById(int id)
//        {
//            var resultado = new Result<List<Resultado_OrdenTrabajoDetalle_Requerimiento>>();

//            try
//            {
//                IQuery query = GetSession().CreateSQLQuery("exec OrdenTrabajo_Detalle_Requerimientos @id=:id");
//                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenTrabajoDetalle_Requerimiento>());
//                query.SetInt32("id", id);
//                resultado.Return = query.List<Resultado_OrdenTrabajoDetalle_Requerimiento>().ToList();
//            }
//            catch (Exception e)
//            {
//                resultado.AddErrorInterno(e);
//            }
//            return resultado;
//        }

//        public Result<List<Resultado_OrdenTrabajoDetalle_Movil>> GetDetalleMovilesById(int id)
//        {
//            var resultado = new Result<List<Resultado_OrdenTrabajoDetalle_Movil>>();

//            try
//            {
//                IQuery query = GetSession().CreateSQLQuery("exec OrdenTrabajo_Detalle_Moviles @id=:id");
//                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenTrabajoDetalle_Movil>());
//                query.SetInt32("id", id);
//                resultado.Return = query.List<Resultado_OrdenTrabajoDetalle_Movil>().ToList();
//            }
//            catch (Exception e)
//            {
//                resultado.AddErrorInterno(e);
//            }
//            return resultado;
//        }

//        public Result<List<Resultado_OrdenTrabajoDetalle_Barrio>> GetDetalleBarriosById(int id)
//        {
//            var resultado = new Result<List<Resultado_OrdenTrabajoDetalle_Barrio>>();

//            try
//            {
//                IQuery query = GetSession().CreateSQLQuery("exec OrdenTrabajo_Detalle_Barrios @id=:id");
//                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenTrabajoDetalle_Barrio>());
//                query.SetInt32("id", id);
//                resultado.Return = query.List<Resultado_OrdenTrabajoDetalle_Barrio>().ToList();
//            }
//            catch (Exception e)
//            {
//                resultado.AddErrorInterno(e);
//            }
//            return resultado;
//        }

        //Detalle Orden Trabajo

        public Result<Resultado_OrdenInspeccionDetalle> GetDetalleById(int id, int? idUsuario)
        {
            var resultado = new Result<Resultado_OrdenInspeccionDetalle>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec OrdenInspeccion_Detalle @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenInspeccionDetalle>());
                query.SetInt32("id", id);
                var data = query.List<Resultado_OrdenInspeccionDetalle>().ToList();
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

        public Result<List<Resultado_OrdenInspeccionDetalle_EstadoHistorial>> GetDetalleHistorialEstadosById(int id)
        {
            var resultado = new Result<List<Resultado_OrdenInspeccionDetalle_EstadoHistorial>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec OrdenInspeccion_Detalle_HistoricoEstados @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenInspeccionDetalle_EstadoHistorial>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_OrdenInspeccionDetalle_EstadoHistorial>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Resultado_OrdenInspeccionDetalle_Nota>> GetDetalleNotasById(int id)
        {
            var resultado = new Result<List<Resultado_OrdenInspeccionDetalle_Nota>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec OrdenInspeccion_Detalle_Notas @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenInspeccionDetalle_Nota>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_OrdenInspeccionDetalle_Nota>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Resultado_OrdenInspeccionDetalle_Requerimiento>> GetDetalleRequerimientosById(int id)
        {
            var resultado = new Result<List<Resultado_OrdenInspeccionDetalle_Requerimiento>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec OrdenInspeccion_Detalle_Requerimientos @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenInspeccionDetalle_Requerimiento>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Resultado_OrdenInspeccionDetalle_Requerimiento>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        //public Result<List<Resultado_OrdenInspeccionDetalle_Barrio>> GetDetalleBarriosById(int id)
        //{
        //    var resultado = new Result<List<Resultado_OrdenInspeccionDetalle_Barrio>>();

        //    try
        //    {
        //        IQuery query = GetSession().CreateSQLQuery("exec OrdenInspeccion_Detalle_Barrios @id=:id");
        //        query.SetResultTransformer(Transformers.AliasToBean<Resultado_OrdenInspeccionDetalle_Barrio>());
        //        query.SetInt32("id", id);
        //        resultado.Return = query.List<Resultado_OrdenInspeccionDetalle_Barrio>().ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        resultado.AddErrorInterno(e);
        //    }
        //    return resultado;
        //}

        public Result<List<int>> GetIds(Consulta_OrdenInspeccion consulta)
        {
            var result = new Result<List<int>>();

            try
            {
                var query = GetQuery(consulta);
                query.Select(x => x.Id).SelectList(list => list.SelectGroup((x => x.Id)));
                result.Return = query.List<int>().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        private IQueryOver<OrdenInspeccion, OrdenInspeccion> GetQuery(Consulta_OrdenInspeccion consulta)
        {
            var query = GetSession().QueryOver<OrdenInspeccion>();
            var joinRequerimiento = query.JoinQueryOver<RequerimientoPorOrdenInspeccion>(x => x.RequerimientosPorOrdenInspeccion).JoinQueryOver<Requerimiento>(x => x.Requerimiento).Where(x => x.FechaBaja == null);

            //Areas
            if (consulta.IdArea.HasValue)
            {
                var joinArea= joinRequerimiento.JoinQueryOver<CerrojoArea>(x => x.AreaResponsable);
                joinArea.Where(x => x.Id == consulta.IdArea.Value);
            }

            //Zonas
            if (consulta.IdZona.HasValue)
            {
                var idsBarrios = GetSession().QueryOver<BarrioPorZona>().Where(x => x.Zona.Id == consulta.IdZona.Value).Select(x => x.Barrio.Id).List<int>().ToList();
                var joinDomicilio = joinRequerimiento.JoinQueryOver<Domicilio>(x => x.Domicilio).JoinQueryOver<Barrio>(x => x.Barrio);
                joinDomicilio.Where(x => x.Id.IsIn(idsBarrios));
            }

            //Numero
            if (consulta.Numero != null && consulta.Numero != "")
            {
                query.Where(x => x.Numero.IsLike(consulta.Numero, MatchMode.Exact));
            }

            //Año
            if (consulta.Año.HasValue)
            {
                query.Where(x => x.Año == consulta.Año.Value);
            }

            //Estados
            if (consulta.EstadosKeyValue != null && consulta.EstadosKeyValue.Count != 0)
            {
                query.JoinQueryOver<EstadoOrdenInspeccionHistorial>(x => x.Estados).Where(x => x.Ultimo == true).JoinQueryOver<EstadoOrdenInspeccion>(x => x.Estado).Where(x => x.KeyValue.IsIn(consulta.EstadosKeyValue));
            }

            //Fechas
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                query.Where(x => x.FechaAlta.Value.Date >= consulta.FechaDesde.Value.Date && x.FechaAlta.Value.Date <= consulta.FechaHasta.Value.Date);
            }

            if (consulta.Marcado.HasValue)
            {
                query.Where(x=>x.Marcado==consulta.Marcado.Value);
            }

            //Dado de baja
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

        public Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>> GetResultadoTablaByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>>();

            if (ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_OrdenInspeccion>();
                resultadoTabla.Data = new List<ResultadoTabla_OrdenInspeccion>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_OrdenInspeccion>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoTabla_OrdenInspeccion>();

                var sb = new StringBuilder();
                var sql = @"      
                            SELECT TOP " + @limite + @"
                            o.Id as Id, 
                            o.FechaAlta as FechaAlta, 
                                 (o.Numero + '/' + CONVERT(varchar, o.Anio)) as Numero, 

                            er.Nombre as EstadoNombre,
                            cast(er.KeyValue as int) as EstadoKeyValue,
                            erh.Id as EstadoId,
                            er.Color as EstadoColor,
                            DATEDIFF(DAY, erh.FechaAlta , GETDATE()) as Dias,

                            o.Descripcion as Descripcion

                            from OrdenInspeccion o
                            
                            inner join EstadoOrdenInspeccionHistorial erh on erh.IdOrdenInspeccion = o.Id and erh.Ultimo = 1
                            inner join EstadoOrdenInspeccion er on erh.IdEstado = er.Id
												
                            inner join (
 	                        select -1 as Id2 ";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                      as x on o.Id = x.Id2
	                    WHERE erh.Ultimo = 1 AND (erh.Ultimo = 1 or erh.Ultimo is null)
	                    ORDER BY Dias DESC
	                 ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_OrdenInspeccion)));
                var resultado = query.List<ResultadoTabla_OrdenInspeccion>().ToList();
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

        public Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>> GetUltimas(int cantidad, Consulta_OrdenInspeccion consulta)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_OrdenInspeccion>>();

            try
            {
                var query = GetQuery(consulta);
                query.Select(x => x.Id);
                query.OrderBy(x => x.FechaAlta).Desc();
                var ultimos = query.Take(cantidad).List<int>().ToList();

                return GetResultadoTablaByIds(5, ultimos);
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
                return result;
            }
        }

    }
}
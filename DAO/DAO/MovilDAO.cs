using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate;
using Model.Consultas;
using System.Diagnostics;
using System.Text.RegularExpressions;
using NHibernate.Criterion;
using Model.Resultados;
using NHibernate.Transform;
using System.Text;
using Model.Utiles;


namespace DAO.DAO
{
    public class MovilDAO : BaseDAO<Movil>
    {
        private static MovilDAO instance;

        public static MovilDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MovilDAO();
                }
                return instance;
            }
        }


        private IQueryOver<Movil, Movil> GetQuery(Consulta_Movil consulta)
        {
            var query = GetSession().QueryOver<Movil>();

            //Estados
            if (consulta.Estados != null && consulta.Estados.Count != 0)
            {
                query.JoinQueryOver<EstadoMovilHistorial>(x => x.Estados).Where(x => x.Ultimo == true).JoinQueryOver<EstadoMovil>(x => x.Estado).Where(x => x.KeyValue.IsIn(consulta.Estados));
            }

            //Areas
            if (consulta.IdArea.HasValue && consulta.IdArea > 0)
            {
                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id == consulta.IdArea);
            }
            
            //OT
            if (consulta.IdOT.HasValue && consulta.IdOT > 0)
            {
                query.JoinQueryOver<MovilPorOrdenTrabajo>(x => x.OrdenesTrabajo).Where(x => x.OrdenTrabajo.Id == consulta.IdOT && x.FechaBaja==null);
            }

            //Tipo
            if (consulta.IdTipo.HasValue && consulta.IdTipo > 0)
            {
                query.JoinQueryOver<TipoMovil>(x => x.Tipo).Where(x => x.Id == consulta.IdTipo);
            }

            //Tiene flota?
            if (consulta.Flota.HasValue)
            {
                if (consulta.Flota.Value)
                {
                    query.Where(x => x.FlotaActiva != null);
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

        public Result<List<Movil>> GetByFilters(Consulta_Movil consulta)
        {
            var result = new Result<List<Movil>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = query.List().Distinct().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<int>> GetIdsByFilters(Consulta_Movil consulta)
        {
            var result = new Result<List<int>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = query.Select(x => x.Id).List<int>().Distinct().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<int> GetCantidadByFilters(Consulta_Movil consulta)
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


        public Result<ResultadoTabla<ResultadoTabla_Movil>> GetResultadoTablaByIds(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_Movil>>();

            if (ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_Movil>();
                resultadoTabla.Data = new List<ResultadoTabla_Movil>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_Movil>();
                resultadoTabla.CantidadMaxima = limite;

                var data = new List<ResultadoTabla_Movil>(); ;

                var sb = new StringBuilder();
                var sql = @"
                    SELECT TOP " + @limite + @"
	                    m.Id as Id,
                    m.NumeroInterno as NumeroInterno, 
                    m.IdTipoMovil as TipoMovilId,
                    t.Nombre as TipoMovilNombre,
                    m.Dominio as Dominio,
                    m.Marca as Marca,
                    m.Modelo as Modelo,
                    m.FechaAlta as FechaAlta,
                    m.FechaBaja as FechaBaja,
                    m.Observaciones as Observaciones,
										itv.FechaVencimientoITV as VencimientoITV,
										DATEDIFF(DAY, GETDATE(), itv.FechaVencimientoITV) as DiasITV,
										tuv.FechaVencimientoTUV as VencimientoTUV,
										DATEDIFF(DAY, GETDATE(), tuv.FechaVencimientoTUV) as DiasTUV,
										estado.IdEstado as IdEstado,
										estado.NombreEstado as NombreEstado

                    from Movil m
                    left join (select IdMovil, FechaVencimientoITV, ROW_NUMBER() OVER (PARTITION BY IdMovil ORDER BY FechaAlta desc) as Ultimo from ITVPorMovil ) itv on itv.IdMovil=m.Id and itv.Ultimo=1
										left join (select IdMovil, FechaVencimientoTUV, ROW_NUMBER() OVER (PARTITION BY IdMovil ORDER BY FechaAlta desc) as Ultimo from TUVPorMovil ) tuv on tuv.IdMovil=m.Id and tuv.Ultimo=1
										left join (select emh.IdMovil, emh.IdEstado, e.Nombre as NombreEstado, ROW_NUMBER() OVER (PARTITION BY emh.IdMovil ORDER BY emh.FechaAlta desc) as Ultimo from EstadoMovilHistorial emh inner join EstadoMovil e on e.Id=emh.IdEstado ) estado on estado.IdMovil=m.Id and estado.Ultimo=1
                    inner join TipoMovil t on t.Id = m.IdTipoMovil        
	                inner join (
 	                select -1 as Id2";
                sb.Append(sql);
                foreach (var id in ids)
                {
                    sb.Append(" union all select " + id + " ");
                }
                sb.Append(@") 
	                as x on m.Id = x.Id2
	                ORDER BY m.FechaAlta DESC
	                ");

                IQuery query = GetSession().CreateSQLQuery(sb.ToString());
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(ResultadoTabla_Movil)));
                var resultado = query.List<ResultadoTabla_Movil>().ToList();
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

        public Result<bool> HayMovilesConTipo(int idTipo)
        {
            var result = new Result<bool>();

            try
            {
                var query = GetQuery(new Consulta_Movil()
                {
                    IdTipo = idTipo,
                    DadosDeBaja = false
                });

                result.Return = query.List().ToList().Count != 0;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_HistoricoEstados>> GetDetalleHistorialEstadosById(int id)
        {
            var resultado = new Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_HistoricoEstados>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Movil_Detalle_HistoricoEstados @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Model.Resultados.Resultado_Movil.Resultado_Movil_HistoricoEstados>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Model.Resultados.Resultado_Movil.Resultado_Movil_HistoricoEstados>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_Nota>> GetDetalleNotasById(int id)
        {
            var resultado = new Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_Nota>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Movil_Detalle_Notas @id=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Model.Resultados.Resultado_Movil.Resultado_Movil_Nota>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Model.Resultados.Resultado_Movil.Resultado_Movil_Nota>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }


        public Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_Reparacion>> GetDetalleReparacionesById(int id)
        {
            var resultado = new Result<List<Model.Resultados.Resultado_Movil.Resultado_Movil_Reparacion>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec Movil_Detalle_Reparaciones @idMovil=:id");
                query.SetResultTransformer(Transformers.AliasToBean<Model.Resultados.Resultado_Movil.Resultado_Movil_Reparacion>());
                query.SetInt32("id", id);
                resultado.Return = query.List<Model.Resultados.Resultado_Movil.Resultado_Movil_Reparacion>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

    }
}

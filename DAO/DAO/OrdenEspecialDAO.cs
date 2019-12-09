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

namespace DAO.DAO
{
    public class OrdenEspecialDAO : BaseDAO<OrdenAtencionCritica>
    {
        private static OrdenEspecialDAO instance;

        public static OrdenEspecialDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new OrdenEspecialDAO();
                return instance;
            }
        }
        
        public Result<ResultadoTabla<ResultadoTabla_OrdenAtencionCritica>> GetResultadoTabla(int limite, Consulta_OrdenAtencionCritica consulta)
        {
            var query = GetQuery(consulta);
            var ids = query.Select(x => x.Id).List<int>().ToList();
            return GetResultadoTabla(limite, ids);
        }

        private IQueryOver<OrdenAtencionCritica, OrdenAtencionCritica> GetQuery(Consulta_OrdenAtencionCritica consulta)
        {
            var query = GetSession().QueryOver<OrdenAtencionCritica>();

            var joinRequerimientoXOrden = query.JoinQueryOver<RequerimientoPorOrdenEspecial>(x => x.RequerimientosPorOrdenEspecial);
            var joinRequerimiento = joinRequerimientoXOrden.JoinQueryOver<Requerimiento>(x=>x.Requerimiento);
            var joinMotivo = joinRequerimiento.JoinQueryOver<Motivo>(x => x.Motivo);
            var joinArea = joinMotivo.JoinQueryOver<CerrojoArea>(x => x.Area);
            var joinServicio = joinMotivo.JoinQueryOver<Tema>(x => x.Tema).JoinQueryOver<Servicio>(x => x.Servicio);

            //Estados
            if (consulta.EstadosKeyValue != null && consulta.EstadosKeyValue.Count != 0)
            {
                var joinUltimoEstado = query.JoinQueryOver<EstadoOrdenEspecialHistorial>(x => x.Estados).Where(x => x.Ultimo == true);
                joinUltimoEstado.JoinQueryOver<EstadoOrdenEspecial>(x => x.Estado).Where(x => x.KeyValue.IsIn(consulta.EstadosKeyValue));
            }

            //Area
            if (consulta.IdsArea != null && consulta.IdsArea.Count != 0)
            {
                joinArea.Where(x => x.Id.IsIn(consulta.IdsArea));
            }

            //Area
            if (consulta.IdArea != null )
            {
                joinArea.Where(x => x.Id==consulta.IdArea);
            }


            //Servicio
            if (consulta.IdsServicio != null && consulta.IdsServicio.Count != 0)
            {
                joinServicio.Where(x => x.Id.IsIn(consulta.IdsServicio));
            }

            //Motivo
            if (consulta.IdsMotivo != null && consulta.IdsMotivo.Count != 0)
            {
                joinMotivo.Where(x => x.Id.IsIn(consulta.IdsMotivo));
            }

            //Fecha desde y hasta
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                query.Where(x => x.FechaAlta.Value.Date >= consulta.FechaDesde.Value.Date).Where(x => x.FechaAlta.Value.Date <= consulta.FechaHasta.Value.Date);
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

        public Result<ResultadoTabla<ResultadoTabla_OrdenAtencionCritica>> GetResultadoTabla(int limite, List<int> ids)
        {
            var result = new Result<ResultadoTabla<ResultadoTabla_OrdenAtencionCritica>>();

            if (ids.Count == 0)
            {
                var resultadoTabla = new ResultadoTabla<ResultadoTabla_OrdenAtencionCritica>();
                resultadoTabla.Data = new List<ResultadoTabla_OrdenAtencionCritica>();
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
                return result;
            }

            try
            {
                var idsString = "(" + string.Join(",", ids) + ")";

                var sql = @"
                     SELECT TOP " + limite + @" 
                        oac.Id as Id,
                        oac.FechaAlta as FechaAlta,
                                                
                        s.Id as ServicioId,                        
                        s.Nombre as ServicioNombre,
                        
                        m.Id as MotivoId,
                        m.Nombre as MotivoNombre,
                                                
                        r.Id as IdRequerimiento,
                        (r.Numero + '/' + CONVERT(varchar, r.Anio)) as RequerimientoNumero,
                        
                        e.Id as EstadoId,
                        e.Nombre as EstadoNombre,
                        e.Color as EstadoColor,
                        e.KeyValue as EstadoKeyValue,

                        oac.Descripcion as Descripcion

                        FROM
                        OrdenEspecial oac 
                        inner join RequerimientoPorOrdenEspecial rxoac on rxoac.IdOrdenEspecial=oac.Id
                        inner join Requerimiento r on rxoac.IdRequerimiento=r.Id
                        INNER JOIN EstadoOrdenEspecialHistorial eh ON eh.IdOrdenEspecial = oac.Id
                        INNER JOIN EstadoOrdenEspecial e ON eh.IdEstado = e.Id
                        INNER JOIN Motivo m ON r.IdMotivo = m.Id
						INNER JOIN Tema t ON t.Id = m.IdTema                        
						INNER JOIN Servicio s on s.Id  = t.IdServicio
						INNER JOIN CerrojoArea a ON m.IdAreaCerrojo = a.Id
                        where eh.Ultimo = 1 AND 
                
                        oac.Id in " + idsString + @" 
                        ORDER BY oac.FechaAlta DESC";

                var query = GetSession().CreateSQLQuery(sql);
                var data = query.SetResultTransformer(Transformers.AliasToBean<ResultadoTabla_OrdenAtencionCritica>()).List<ResultadoTabla_OrdenAtencionCritica>().ToList();

                var resultadoTabla = new ResultadoTabla<ResultadoTabla_OrdenAtencionCritica>();
                resultadoTabla.Data = data;
                resultadoTabla.CantidadMaxima = limite;
                result.Return = resultadoTabla;
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
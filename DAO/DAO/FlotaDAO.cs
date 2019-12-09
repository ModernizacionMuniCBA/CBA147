using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using Model.Consultas;
using NHibernate;

namespace DAO.DAO
{
    public class FlotaDAO : BaseDAO<Flota>
    {
        private static FlotaDAO instance;

        public static FlotaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FlotaDAO();
                }
                return instance;
            }
        }

        private IQueryOver<Flota, Flota> GetQuery(Consulta_Flota consulta)
        {
            var query = GetSession().QueryOver<Flota>();

            //Area
            if (consulta.IdArea != null && consulta.IdArea!=0)
            {
                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id==consulta.IdArea);
            }

            //Estados
            if (consulta.Estados != null && consulta.Estados.Count != 0)
            {
                query.JoinQueryOver<EstadoFlotaHistorial>(x => x.Estados).Where(x => x.Ultimo == true).JoinQueryOver<EstadoFlota>(x => x.Estado).Where(x => x.KeyValue.IsIn(consulta.Estados));
            }

            //OT
            if (consulta.IdOT.HasValue && consulta.IdOT > 0)
            {
                query.JoinQueryOver<FlotaPorOrdenTrabajo>(x => x.OrdenesTrabajo).Where(x => x.OrdenTrabajo.Id == consulta.IdOT && x.FechaBaja == null);
            }


            //OT
            if (consulta.IdMovil.HasValue)
            {
                query.Where(x => x.Movil.Id == consulta.IdMovil);
            }

            if (consulta.Hoy.HasValue && consulta.Hoy.Value)
            {
                query.Where(x => x.FechaAlta.Value.Day == DateTime.Now.Day);
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

        public Result<List<Flota>> GetByFilters(Consulta_Flota consulta)
        {
            var result = new Result<List<Flota>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = new List<Flota>(query.List());
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

        public Result<int> GetCantidadByFilters(Consulta_Flota consulta)
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

        public Result<List<int>> GetIdsByFilters(Consulta_Flota consulta)
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
    }
}

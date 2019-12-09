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
    public class BarrioPorZonaDAO : BaseDAO<BarrioPorZona>
    {
        private static BarrioPorZonaDAO instance;

        public static BarrioPorZonaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BarrioPorZonaDAO();
                }
                return instance;
            }
        }


        private NHibernate.IQueryOver<BarrioPorZona, BarrioPorZona> GetQuery(Consulta_BarrioPorZona consulta)
        {
            var query = GetSession().QueryOver<BarrioPorZona>();
            var joinZona = query.JoinQueryOver<Zona>(x => x.Zona).Where(x => x.FechaBaja == null);

            if (consulta.IdsBarrio != null && consulta.IdsBarrio.Count != 0)
            {
                query.Where(x => x.Barrio.Id.IsIn(consulta.IdsBarrio));
            }

            if (consulta.IdsZona != null && consulta.IdsZona.Count != 0)
            {
                query.Where(x => x.Zona.Id.IsIn(consulta.IdsZona));
            }

            if (consulta.IdsArea != null && consulta.IdsArea.Count != 0)
            {
                joinZona.Where(x => x.Area.Id.IsIn(consulta.IdsArea));
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

        public Result<List<int>> GetIdsByFilters(Consulta_BarrioPorZona consulta)
        {

            var result = new Result<List<int>>();

            try
            {
                var query = GetQuery(consulta);
                query.Select(x => x.Id);
                result.Return = query.List<int>().ToList();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<Resultado_BarrioPorZona>> GetByFilters(Consulta_BarrioPorZona consulta)
        {
            var result = new Result<List<Resultado_BarrioPorZona>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = Resultado_BarrioPorZona.ToList(query.List().ToList());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<bool> ValidarBarrio(int? idZona, int idBarrio, int idArea)
        {
            var result = new Result<bool>();

            try
            {
                var sql = @"
                    select bxz.Id from BarrioPorZona bxz 
                    inner join Zona z on z.Id = bxz.IdZona
                    inner join CerrojoArea a on a.Id = z.IdAreaCerrojo
                    inner join Barrio b on b.Id = bxz.IdBarrio
                    where (b.IdCatastro = " + idBarrio + " and a.Id = " + idArea + " and bxz.FechaBaja is null and z.FechaBaja is null )";

                if (idZona.HasValue)
                {
                    sql += " and bxz.IdZona != " + idZona.Value;
                }

                var query = GetSession().CreateSQLQuery(sql);

                var resultado = query.List<int>().ToList();

                if (resultado.Count() == 0)
                {
                    result.Return = true;
                }
                else
                {
                    result.Return = false;
                }

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<List<int>> GetIdsBarrioByZona(int idZona)
        {
            var resultado = new Result<List<int>>();
            try
            {
                var sql = @"Select distinct b.IdCatastro from BarrioPorZona bxz
                    inner join Barrio b on b.Id = bxz.IdBarrio
                    where b.FechaBaja is null and bxz.FechaBaja is null and bxz.IdZona = " + idZona;

                resultado.Return = GetSession().CreateSQLQuery(sql).List<int>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;

            //            var result = new Result<List<int>>();

            //            try
            //            {
            //                var sql = @"
            //                    select b.IdCatastro from BarrioPorZona bxz 
            //                    inner join Barrio b on b.Id = bxz.IdBarrio
            //                    inner join Zona z on z.Id = bxz.IdZona
            //                    where bxz.FechaBaja is null and z.FechaBaja is null and bxz.IdZona = " + idZona;

            //                var query = GetSession().CreateSQLQuery(sql);
            //                result.Return = query.List<int>().ToList();
            //            }
            //            catch (Exception e)
            //            {
            //                result.AddErrorInterno(e);
            //            }

            //            return result;
        }

        public Result<List<int>> GetIdsBarrioByArea(int idArea)
        {
            var resultado = new Result<List<int>>();
            try
            {
                var query = GetQuery(new Model.Consultas.Consulta_BarrioPorZona()
                {
                    IdsArea = new List<int>() { idArea },
                    DadosDeBaja = false
                });
                resultado.Return = query.Select(x => x.Barrio.IdCatastro).List<int>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }


        public Result<List<int>> GetIdsBarriosYaSeleccionados(int? idZona, int idArea)
        {
            var resultado = new Result<List<int>>();
            try
            {
                var sql = @"
                    Select b.IdCatastro from BarrioPorZona bxz
                    inner join Barrio b on b.Id = bxz.IdBarrio
                    inner join Zona z on z.Id = bxz.IdZona
                    inner join CerrojoArea a on a.Id = z.IdAreaCerrojo
                    where a.Id = " + idArea + " and a.FechaBaja is null and z.FechaBaja is null and b.FechaBaja is null and bxz.FechaBaja is null";
                if (idZona.HasValue)
                {
                    sql += " and bxz.IdZona <> " + idZona.Value;
                }

                resultado.Return = GetSession().CreateSQLQuery(sql).List<int>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
    }
}

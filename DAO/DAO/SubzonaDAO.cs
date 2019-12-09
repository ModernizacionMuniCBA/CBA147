using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using Model.Consultas;
using NHibernate;
using Model.Resultados;


namespace DAO.DAO
{
    public class SubzonaDAO : BaseDAO<Subzona>
    {
        private static SubzonaDAO instance;

        public static SubzonaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SubzonaDAO();
                }
                return instance;
            }
        }

        private IQueryOver<Subzona, Subzona> GetQuery(Consulta_Subzona consulta)
        {
            var query = GetSession().QueryOver<Subzona>();

            //Nombre
            if (!string.IsNullOrEmpty(consulta.Nombre))
            {
                query.Where(x => x.Nombre.Contains(consulta.Nombre));
            }

            //Area
            if (consulta.AreaId.HasValue)
            {
                query.JoinQueryOver<Zona>(x => x.Zona).JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id == consulta.AreaId.Value);
            }

            //Zona
            if (consulta.ZonaId.HasValue)
            {
                query.JoinQueryOver<Zona>(x => x.Zona).Where(x => x.Id == consulta.ZonaId.Value);
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

        //Original
        public Result<List<Resultado_Subzona>> GetByFilters(Consulta_Subzona consulta)
        {
            var result = new Result<List<Resultado_Subzona>>();

            try
            {
                var query = GetQuery(consulta);
                result.Return = Resultado_Subzona.ToList(query.List().ToList());
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
        
        public Result<int> GetCantidadDuplicados(int? id, string nombre, bool? dadosDeBaja, int? idZona)
        {
            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<Subzona>();

                if (id.HasValue)
                {
                    query.Where(x => x.Id != id);
                }
                query.Where(x => x.Nombre == nombre && x.Zona.Id == idZona && x.FechaBaja==null);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
    }
}

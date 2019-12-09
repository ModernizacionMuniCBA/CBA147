using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using NHibernate;
using NHibernate.Transform;






namespace DAO.DAO
{
    public class ServicioDAO : BaseDAO<Servicio>
    {
        private static ServicioDAO instance;

        public static ServicioDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServicioDAO();
                }
                return instance;
            }
        }

        //Original
        public Result<List<Servicio>> GetByFilters(List<Enums.TipoMotivo> tiposMotivo, bool? dadosDeBaja)
        {
            var result = new Result<List<Servicio>>();

            try
            {
                var query = GetSession().QueryOver<Servicio>();

                if (tiposMotivo != null && tiposMotivo.Count > 0)
                {
                    query.JoinQueryOver<Tema>(x => x.Temas).JoinQueryOver<Motivo>(x => x.Motivos).Where(z => z.Tipo.IsIn(tiposMotivo) && z.FechaBaja == null);
                }

                //Traigo los Dados de baja O los activos
                if (dadosDeBaja == true)
                {
                    query.Where(x => x.FechaBaja != null);
                }
                else
                {
                    query.Where(x => x.FechaBaja == null);
                }

                result.Return = new List<Servicio>(query.List().Distinct());
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

        public Result<int> GetCantidadDuplicados(int? id, string nombre, bool? dadosDeBaja)
        {
            var result = new Result<int>();
            try
            {
                var query = GetSession().QueryOver<Servicio>();

                if (id.HasValue)
                {
                    query.Where(x => x.Id != id);
                }

                if (dadosDeBaja.HasValue && !dadosDeBaja.Value)
                {
                    query.Where(x => x.FechaBaja == null);
                }
                query.Where(x => x.Nombre == nombre);
                result.Return = query.RowCount();
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<Servicio> GetByMotivo(int idMotivo, bool? dadosDeBaja)
        {
            var result = new Result<Servicio>();

            try
            {
                var query = GetSession().QueryOver<Servicio>();

                query.JoinQueryOver<Tema>(x => x.Temas).JoinQueryOver<Motivo>(x => x.Motivos).Where(x => x.Id == idMotivo);

                //Dados de baja
                if (dadosDeBaja.HasValue)
                {
                    if (dadosDeBaja.Value)
                    {
                        query.Where(x => x.FechaBaja != null);
                    }
                    else
                    {
                        query.Where(x => x.FechaBaja == null);
                    }
                }

                result.Return = query.SingleOrDefault();
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

        public Result<List<int>> GetIdsAreasById(int idServicio)
        {
            var resultado = new Result<List<int>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec AreasPorServicio @idServicio=:idServicio");

                query.SetInt32("idServicio", idServicio);
                resultado.Return = query.List<int>().ToList();
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }
    }
}

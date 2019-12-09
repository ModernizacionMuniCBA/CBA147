using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using NHibernate.Criterion;
using Model.Resultados;
using NHibernate.Transform;
using NHibernate;
using Model.Consultas;

namespace DAO.DAO
{
    public class MotivoDAO : BaseDAO<Motivo>
    {
        private static MotivoDAO instance;

        public static MotivoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MotivoDAO();
                }
                return instance;
            }
        }

        public Result<List<Motivo>> GetByFilters(Consulta_Motivo consulta)
        {
            var result = new Result<List<Motivo>>();

            try
            {
                var query = GetSession().QueryOver<Motivo>();

                //Area
                if (consulta.IdArea.HasValue)
                {
                    query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id == consulta.IdArea.Value && x.FechaBaja==null);
                }

                //Servicio
                if (consulta.IdServicio.HasValue)
                {
                    query.JoinQueryOver<Tema>(x => x.Tema).JoinQueryOver<Servicio>(x => x.Servicio).Where(x => x.Id == consulta.IdServicio.Value && x.FechaBaja == null);
                }

                //Urgente
                if (consulta.Urgente.HasValue)
                {
                    query.Where(x => x.Urgente == consulta.Urgente.Value );
                }

                //Tipos
                if (consulta.Tipos.Count>0)
                {
                    query.Where(x => x.Tipo.IsIn(consulta.Tipos));
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
                result.Return = new List<Motivo>(query.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<Motivo>> GetDeServicio(int idServicio, bool? urgentes, Enums.TipoMotivo? tipo, bool? dadosDeBaja)
        {
            var result = new Result<List<Motivo>>();

            try
            {
                var query = GetSession().QueryOver<Motivo>();

                //Servicio
                query.JoinQueryOver<Tema>(x => x.Tema).JoinQueryOver<Servicio>(x => x.Servicio).Where(x => x.Id == idServicio);

                //Dados de baja
                if (urgentes.HasValue)
                {
                    query.Where(x => x.Urgente == urgentes);
                }

                //internos
                if (tipo.HasValue)
                {
                    query.Where(x => x.Tipo == tipo.Value);
                }


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
                result.Return = new List<Motivo>(query.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }

        public Result<List<Motivo>> GetByArea(int idArea, Enums.TipoMotivo? tipo, bool? dadosDeBaja)
        {
            var result = new Result<List<Motivo>>();

            try
            {
                var query = GetSession().QueryOver<Motivo>();

                //Servicio
                query.JoinQueryOver<CerrojoArea>(x => x.Area).Where(x => x.Id == idArea);

                if (tipo.HasValue)
                {
                    query.Where(x => x.Tipo == tipo);
                }

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
                result.Return = new List<Motivo>(query.List());
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }


        public Result<List<Resultado_ServicioAreaMotivo>> GetInfo(List<int> idsAreas)
        {
            var resultado = new Result<List<Resultado_ServicioAreaMotivo>>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec ServicioAreaMotivoConsulta");
                query.SetResultTransformer(Transformers.AliasToBean<Resultado_ServicioAreaMotivo>());
                var data = query.List<Resultado_ServicioAreaMotivo>().ToList();
                data = data.Where(x => idsAreas.Contains(x.AreaId)).ToList();
                resultado.Return = data;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<Resultado_Servicio> GetServicioByIdArea(int idArea)
        {
            var resultado = new Result<Resultado_Servicio>();

            try
            {
                var ids = GetSession().CreateSQLQuery(@"
                    select top 1 s.Id from Motivo m inner join CerrojoArea a on a.Id = m.IdAreaCerrojo 
                    inner join Tema t on t.Id = m.IdTema
                    inner join Servicio s on s.Id = t.IdServicio
                    where t.FechaBaja is null and m.FechaBaja is null and a.Id = " + idArea).List<int>();
                if (ids.Count == 0)
                {
                    resultado.Return = null;
                }
                else
                {
                    var resultadoConsulta = ServicioDAO.Instance.GetByIdObligatorio(ids[0]);
                    if (!resultadoConsulta.Ok)
                    {
                        resultado.Copy(resultadoConsulta.Errores);
                        return resultado;
                    }

                    resultado.Return = new Resultado_Servicio(resultadoConsulta.Return);
                }
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<bool> Equals(Motivo obj)
        {
            var result = new Result<bool>();

            try
            {
                var query = GetSession().QueryOver<Motivo>();
                query.Where(x => x.Nombre.IsLike(obj.Nombre.ToUpper().Trim()));
                query.Where(x=>x.Area==obj.Area);
                query.Where(x => x.Id != obj.Id);
                query.Where(x => x.FechaBaja == null);
                result.Return = query.List().Count != 0;
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e.InnerException);
                return result;
            }
        }
    }
}

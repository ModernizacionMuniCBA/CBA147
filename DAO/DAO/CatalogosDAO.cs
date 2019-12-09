using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using System.Text;
using Model.Resultados;
using NHibernate;
using Model.Consultas;
using Model.Resultados.Estadisticas;

namespace DAO.DAO
{
    public class CatalogoDAO : BaseDAO<BaseEntity>
    {
        private static CatalogoDAO instance;

        public static CatalogoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CatalogoDAO();
                }
                return instance;
            }
        } 

        public Result<List<Resultado_CatalogoUsuarios>> GetDatosCatalogoUsuarios(int idArea)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_CatalogoUsuarios>>();
                      
            try
            {
                var sql = @"
                        select
                        u.Apellido,
                        u.Nombre,
                        u.Dni,
                        u.Username Usuario,
                        r.Nombre Rol,
                        u.Email, 
	                    u.TelefonoCelular Telefono,
                        case
                            when (amb.Id = 1 or amb.Id is null) then 'Área operativa'
                            else amb.Nombre
                            end Ubicacion
                        from cerrojo.dbo.RolPorAreaPorUsuario rxaxu
                        join cerrojo.dbo.Rol r on r.Id = rxaxu.IdRol
                        join cerrojo.dbo.Area a on a.Id = rxaxu.IdArea
                        join cerrojo.dbo.Usuario u on u.Id = rxaxu.IdUser
                        left join cerrojo.dbo.AmbitoTrabajoPorUsuarioPorAplicacion aua on aua.IdUser = u.Id
                        left join cerrojo.dbo.AmbitoTrabajo amb on (amb.Id = aua.IdAmbitoTrabajo)
                        where
                        a.Id != 1368
                        and rxaxu.FechaBaja is null
                        and r.FechaBaja is null
                        and a.FechaBaja is null
                        and u.FechaBaja is null
                        and u.Id not in (24, 383, 317, 318, 340, 18363,33908,29107,29108,29106,29111,29112,29110,29104,29105,29101,29099,29100,29095)                        
		                and a.Id = " + idArea+ @"
                        order by
                        u.Apellido,
                        u.Nombre;
                        ";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_CatalogoUsuarios)));
   
                var resultado = query.List<Resultado_CatalogoUsuarios>().ToList();
                var data = new List<Resultado_CatalogoUsuarios>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);
                }
                result.Return = data;
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
        public Result<List<Resultado_CatalogoMotivos>> GetDatosCatalogoMotivos(int idArea)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_CatalogoMotivos>>();

            try
            {
                var sql = @"
                        select m.Nombre, m.Urgente, m.Tipo, m.Principal, m.Prioridad, c.Id as idCategoria, c.Nombre Categoria
                        from Motivo m
                        join cerrojo.dbo.Area a on a.Id = m.IdAreaCerrojo
                        left join CategoriaMotivoArea c on m.IdCategoriaMotivoArea = c.Id 
                        where
                        a.Id != 1368
                        and a.FechaBaja is null
                        and m.FechaBaja is null
                        and a.Id = " + idArea + @"
                        ORDER BY a.Nombre desc
                        ";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_CatalogoMotivos)));

                var resultado = query.List<Resultado_CatalogoMotivos>().ToList();
                var data = new List<Resultado_CatalogoMotivos>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);
                }
                result.Return = data;
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
        public Result<List<Resultado_CatalogoTareas>> GetDatosCatalogoTareas(int idArea)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_CatalogoTareas>>();

            try
            {
                var sql = @"
                        select txa.Nombre, txa.Observaciones
                        from TareaPorArea txa
                        join cerrojo.dbo.Area a on a.Id = txa.IdAreaCerrojo
                        where
                        a.FechaBaja is null
                        and txa.FechaBaja is null
                        and a.Id = " + idArea + @"
                        ";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_CatalogoTareas)));

                var resultado = query.List<Resultado_CatalogoTareas>().ToList();
                var data = new List<Resultado_CatalogoTareas>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);
                }
                result.Return = data;
                return result;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
        public Result<int> GetCantidadUsuariosByIdArea(int idArea)
        {
            var result = new Result<int>();

            try
            {
                var sql = @"
                        select count(*)
                        from cerrojo.dbo.RolPorAreaPorUsuario rxaxu
                        join cerrojo.dbo.Rol r on r.Id = rxaxu.IdRol
                        join cerrojo.dbo.Area a on a.Id = rxaxu.IdArea
                        join cerrojo.dbo.Usuario u on u.Id = rxaxu.IdUser
                        left join cerrojo.dbo.AmbitoTrabajoPorUsuarioPorAplicacion aua on aua.IdUser = u.Id
                        left join cerrojo.dbo.AmbitoTrabajo amb on (amb.Id = aua.IdAmbitoTrabajo)
                        where
                        a.Id != 1368
                        and rxaxu.FechaBaja is null
                        and r.FechaBaja is null
                        and a.FechaBaja is null
                        and u.FechaBaja is null
                        and u.Id not in (24, 383, 317, 318, 340, 18363,33908,29107,29108,29106,29111,29112,29110,29104,29105,29101,29099,29100,29095)                        
		                and a.Id = " + idArea + @"
                        ";

                var query = GetSession().CreateSQLQuery(sql);
                var resultado = query.List<int>()[0];
                result.Return = resultado;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
        public Result<int> GetCantidadMotivosByIdArea(int idArea)
        {
            var result = new Result<int>();

            try
            {
                var sql = @"
                        select count(*)
                        from Motivo m
                        join cerrojo.dbo.Area a on a.Id = m.IdAreaCerrojo
                        where
                        a.Id != 1368
                        and a.FechaBaja is null
                        and m.FechaBaja is null
                        and a.Id = " + idArea + @"
                        ";

                var query = GetSession().CreateSQLQuery(sql);
                var resultado = query.List<int>()[0];
                result.Return = resultado;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
        public Result<int> GetCantidadTareasByIdArea(int idArea)
        {
            var result = new Result<int>();

            try
            {
                var sql = @"
                        select count(*)
                        from TareaPorArea txa
                        join cerrojo.dbo.Area a on a.Id = txa.IdAreaCerrojo
                        where
                        a.FechaBaja is null
                        and txa.FechaBaja is null
                        and a.Id = " + idArea + @"
                        ";

                var query = GetSession().CreateSQLQuery(sql);
                var resultado = query.List<int>()[0];
                result.Return = resultado;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }
            return result;
        }
    }
}

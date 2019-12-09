using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using Model.Resultados;
using Model.Consultas;
using NHibernate;


namespace DAO.DAO
{
    public class PermisoEstadoOrdenTrabajoDAO : BaseDAO<PermisoEstadoOrdenTrabajo>
    {
        private static PermisoEstadoOrdenTrabajoDAO instance;

        public static PermisoEstadoOrdenTrabajoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PermisoEstadoOrdenTrabajoDAO();
                }
                return instance;
            }
        }


        public Result<List<Resultado_PermisoOrdenTrabajoAcceso>> GetPermisos()
        {
            var resultado = new Result<List<Resultado_PermisoOrdenTrabajoAcceso>>();

            try
            {
                var permisos = GetSession().QueryOver<PermisoEstadoOrdenTrabajo>().Where(x => x.FechaBaja == null).List().ToList();
                var estados = GetSession().QueryOver<EstadoOrdenTrabajo>().Where(x => x.FechaBaja == null).List().ToList();
                var permisosPorEstado = GetSession().QueryOver<PermisoEstadoOrdenTrabajoPorEstado>().Where(x => x.FechaBaja == null).List().ToList();

                var lista = new List<Resultado_PermisoOrdenTrabajoAcceso>();
                foreach (var p in permisos)
                {
                    foreach (var e in estados)
                    {
                        var entity = new Resultado_PermisoOrdenTrabajoAcceso();
                        entity.EstadoOrdenTrabajo = e.KeyValue;
                        entity.Permiso = p.KeyValue;
                        entity.TienePermiso = permisosPorEstado.Any(x => x.Permiso.KeyValue == p.KeyValue && x.EstadoOrdenTrabajo.KeyValue == e.KeyValue && x.FechaBaja == null);

                        lista.Add(entity);
                    }
                }

                resultado.Return = lista;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }


        public Result<List<Enums.EstadoOrdenTrabajo>> GetEstadosKeyValueByPermiso(Enums.PermisoEstadoOrdenTrabajo permiso)
        {
            var resultado = new Result<List<Enums.EstadoOrdenTrabajo>>();
            try
            {
                var query = GetSession().QueryOver<PermisoEstadoOrdenTrabajoPorEstado>();
                query.Where(x=> x.FechaBaja==null);
                var join = query.JoinQueryOver<PermisoEstadoOrdenTrabajo>(x=>x.Permiso);
                join.Where(x => x.KeyValue == permiso);
                var res = query.List().ToList();
                var list = new List<Enums.EstadoOrdenTrabajo>();
                foreach (var a in res)
                {
                    list.Add(a.EstadoOrdenTrabajo.KeyValue);
                }
                list = list.Distinct().ToList();
                resultado.Return = list;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }

            return resultado;

        }
        public Result<bool> TienePermiso(Enums.EstadoOrdenTrabajo estado, Enums.PermisoEstadoOrdenTrabajo permiso)
        {
            var resultado = new Result<bool>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec EstadoOrdenTrabajoPermiso @keyValueEstado=:estado, @keyValuePermiso=:permiso");
                query.SetInt32("estado", (int)estado);
                query.SetInt32("permiso", (int)permiso);

                resultado.Return = query.List<int>().ToList()[0] == 1;
            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }


    }
}

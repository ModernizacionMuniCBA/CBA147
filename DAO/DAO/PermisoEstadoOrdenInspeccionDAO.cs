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
    public class PermisoEstadoOrdenInspeccionDAO : BaseDAO<PermisoEstadoOrdenInspeccion>
    {
        private static PermisoEstadoOrdenInspeccionDAO instance;

        public static PermisoEstadoOrdenInspeccionDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PermisoEstadoOrdenInspeccionDAO();
                }
                return instance;
            }
        }


        public Result<List<Resultado_PermisoOrdenInspeccionAcceso>> GetPermisos()
        {
            var resultado = new Result<List<Resultado_PermisoOrdenInspeccionAcceso>>();

            try
            {
                var permisos = GetSession().QueryOver<PermisoEstadoOrdenInspeccion>().Where(x => x.FechaBaja == null).List().ToList();
                var estados = GetSession().QueryOver<EstadoOrdenInspeccion>().Where(x => x.FechaBaja == null).List().ToList();
                var permisosPorEstado = GetSession().QueryOver<PermisoEstadoOrdenInspeccionPorEstado>().Where(x => x.FechaBaja == null).List().ToList();

                var lista = new List<Resultado_PermisoOrdenInspeccionAcceso>();
                foreach (var p in permisos)
                {
                    foreach (var e in estados)
                    {
                        var entity = new Resultado_PermisoOrdenInspeccionAcceso();
                        entity.EstadoOrdenInspeccion = e.KeyValue;
                        entity.Permiso = p.KeyValue;
                        entity.TienePermiso = permisosPorEstado.Any(x => x.Permiso.KeyValue == p.KeyValue && x.EstadoOrdenInspeccion.KeyValue == e.KeyValue && x.FechaBaja == null);

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


        //public Result<List<Enums.EstadoOrdenTrabajo>> GetEstadosKeyValueByPermiso(Enums.PermisoEstadoOrdenTrabajo permiso)
        //{
        //    var resultado = new Result<List<Enums.EstadoOrdenTrabajo>>();
        //    try
        //    {
        //        var query = GetSession().QueryOver<PermisoEstadoOrdenTrabajoPorEstado>();
        //        query.Where(x=> x.FechaBaja==null);
        //        var join = query.JoinQueryOver<PermisoEstadoOrdenTrabajo>(x=>x.Permiso);
        //        join.Where(x => x.KeyValue == permiso);
        //        var res = query.List().ToList();
        //        var list = new List<Enums.EstadoOrdenTrabajo>();
        //        foreach (var a in res)
        //        {
        //            list.Add(a.EstadoOrdenTrabajo.KeyValue);
        //        }
        //        list = list.Distinct().ToList();
        //        resultado.Return = list;
        //    }
        //    catch (Exception e)
        //    {
        //        resultado.AddErrorInterno(e);
        //    }

        //    return resultado;

        //}

        public Result<bool> TienePermiso(Enums.EstadoOrdenInspeccion estado, Enums.PermisoEstadoOrdenInspeccion permiso)
        {
            var resultado = new Result<bool>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec EstadoOrdenInspeccionPermiso @keyValueEstado=:estado, @keyValuePermiso=:permiso");
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

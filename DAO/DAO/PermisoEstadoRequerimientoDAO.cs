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
    public class PermisoEstadoRequerimientoDAO : BaseDAO<PermisoEstadoRequerimiento>
    {
        private static PermisoEstadoRequerimientoDAO instance;

        public static PermisoEstadoRequerimientoDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PermisoEstadoRequerimientoDAO();
                }
                return instance;
            }
        }


        public Result<List<Resultado_PermisoRequerimientoAcceso>> GetPermisos()
        {
            var resultado = new Result<List<Resultado_PermisoRequerimientoAcceso>>();

            try
            {
                var permisos = GetSession().QueryOver<PermisoEstadoRequerimiento>().Where(x => x.FechaBaja == null).List().ToList();
                var estados = GetSession().QueryOver<EstadoRequerimiento>().Where(x => x.FechaBaja == null).List().ToList();
                var permisosPorEstado = GetSession().QueryOver<PermisoEstadoRequerimientoPorEstado>().Where(x => x.FechaBaja == null).List().ToList();

                var lista = new List<Resultado_PermisoRequerimientoAcceso>();
                foreach (var p in permisos)
                {
                    foreach (var e in estados)
                    {
                        var entity = new Resultado_PermisoRequerimientoAcceso();
                        entity.EstadoRequerimiento = e.KeyValue;
                        entity.Permiso = p.KeyValue;
                        entity.TienePermiso = permisosPorEstado.Any(x => x.Permiso.KeyValue == p.KeyValue && x.EstadoRequerimiento.KeyValue == e.KeyValue && x.FechaBaja == null);

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


        public Result<List<Enums.EstadoRequerimiento>> GetEstadosKeyValueByPermiso(Enums.PermisoEstadoRequerimiento permiso)
        {
            var resultado = new Result<List<Enums.EstadoRequerimiento>>();
            try
            {
                var query = GetSession().QueryOver<PermisoEstadoRequerimientoPorEstado>();
                query.Where(x=> x.FechaBaja==null);
                var join = query.JoinQueryOver<PermisoEstadoRequerimiento>(x=>x.Permiso);
                join.Where(x => x.KeyValue == permiso);
                var res = query.List().ToList();
                var list = new List<Enums.EstadoRequerimiento>();
                foreach (var a in res)
                {
                    list.Add(a.EstadoRequerimiento.KeyValue);
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
        public Result<bool> TienePermiso(Enums.EstadoRequerimiento estado, Enums.PermisoEstadoRequerimiento permiso)
        {
            var resultado = new Result<bool>();

            try
            {
                IQuery query = GetSession().CreateSQLQuery("exec EstadoRequerimientoPermiso @keyValueEstado=:estado, @keyValuePermiso=:permiso");
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

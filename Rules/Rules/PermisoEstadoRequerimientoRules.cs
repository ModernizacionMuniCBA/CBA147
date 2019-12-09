using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;

namespace Rules.Rules
{
    public class PermisoEstadoRequerimientoRules : BaseRules<PermisoEstadoRequerimiento>
    {

        private readonly PermisoEstadoRequerimientoDAO dao;

        public PermisoEstadoRequerimientoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = PermisoEstadoRequerimientoDAO.Instance;
        }


        public Result<List<Resultado_PermisoRequerimientoAcceso>> GetPermisos()
        {
            return dao.GetPermisos();
        }

        public Result<List<Enums.EstadoRequerimiento>> GetEstadosKeyValueByPermiso(Enums.PermisoEstadoRequerimiento permiso)
        {
            return dao.GetEstadosKeyValueByPermiso(permiso);
        }


        public Result<bool> SetPermisos(List<Resultado_PermisoRequerimientoAcceso> permisosNuevos)
        {
            var resultado = new Result<bool>();
            var resultadoTransaccion = dao.Transaction(() =>
            {

                var listadoEstados = dao.GetSession().QueryOver<EstadoRequerimiento>().Where(x => x.FechaBaja == null).List().ToList();
                var listadoPermisos = dao.GetSession().QueryOver<PermisoEstadoRequerimiento>().Where(x => x.FechaBaja == null).List().ToList();

                var lista = dao.GetSession().QueryOver<PermisoEstadoRequerimientoPorEstado>().Where(x => x.FechaBaja == null).List();

                var rules = new BaseRules<PermisoEstadoRequerimientoPorEstado>(getUsuarioLogueado());
                foreach (var p in lista)
                {
                    p.FechaBaja = DateTime.Now;
                    var resultadoUpdate = rules.Update(p);
                    if (!resultadoUpdate.Ok)
                    {
                        resultado.Copy(resultadoUpdate.Errores);
                        return false;
                    }
                }

                foreach (var p in permisosNuevos)
                {
                    if (p.TienePermiso)
                    {
                        var entity = new PermisoEstadoRequerimientoPorEstado();
                        entity.EstadoRequerimiento = listadoEstados.Where(x => x.KeyValue == p.EstadoRequerimiento).FirstOrDefault();
                        entity.Permiso = listadoPermisos.Where(x => x.KeyValue == p.Permiso).FirstOrDefault();
                        var resultadoInsert = rules.Insert(entity);
                        if (!resultadoInsert.Ok)
                        {
                            resultado.Copy(resultadoInsert.Errores);
                            return false;
                        }
                    }
                }

                resultado.Return = true;
                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorPublico("Error procesando la solicitud");
            }

            return resultado;
        }

        public Result<bool> TienePermiso(Enums.EstadoRequerimiento estado, Enums.PermisoEstadoRequerimiento permiso)
        {
            return dao.TienePermiso(estado, permiso);
        }

    }
}

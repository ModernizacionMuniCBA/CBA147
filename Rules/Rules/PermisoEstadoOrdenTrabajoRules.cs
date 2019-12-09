using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;

namespace Rules.Rules
{
    public class PermisoEstadoOrdenTrabajoRules : BaseRules<PermisoEstadoOrdenTrabajo>
    {

        private readonly PermisoEstadoOrdenTrabajoDAO dao;

        public PermisoEstadoOrdenTrabajoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = PermisoEstadoOrdenTrabajoDAO.Instance;
        }


        public Result<List<Resultado_PermisoOrdenTrabajoAcceso>> GetPermisos()
        {
            return dao.GetPermisos();
        }

        public Result<List<Enums.EstadoOrdenTrabajo>> GetEstadosKeyValueByPermiso(Enums.PermisoEstadoOrdenTrabajo permiso)
        {
            return dao.GetEstadosKeyValueByPermiso(permiso);
        }


        public Result<bool> SetPermisos(List<Resultado_PermisoOrdenTrabajoAcceso> permisosNuevos)
        {
            var resultado = new Result<bool>();
            var resultadoTransaccion = dao.Transaction(() =>
            {

                var listadoEstados = dao.GetSession().QueryOver<EstadoOrdenTrabajo>().Where(x => x.FechaBaja == null).List().ToList();
                var listadoPermisos = dao.GetSession().QueryOver<PermisoEstadoOrdenTrabajo>().Where(x => x.FechaBaja == null).List().ToList();

                var lista = dao.GetSession().QueryOver<PermisoEstadoOrdenTrabajoPorEstado>().Where(x => x.FechaBaja == null).List();

                var rules = new BaseRules<PermisoEstadoOrdenTrabajoPorEstado>(getUsuarioLogueado());
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
                        var entity = new PermisoEstadoOrdenTrabajoPorEstado();
                        entity.EstadoOrdenTrabajo = listadoEstados.Where(x => x.KeyValue == p.EstadoOrdenTrabajo).FirstOrDefault();
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

        public Result<bool> TienePermiso(Enums.EstadoOrdenTrabajo estado, Enums.PermisoEstadoOrdenTrabajo permiso)
        {
            return dao.TienePermiso(estado, permiso);
        }

    }
}

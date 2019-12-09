using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Comandos;
using Model.Resultados;

namespace Rules.Rules
{
    public class RubroMotivoRules  : BaseRules<RubroMotivo>
    {

        private readonly RubroMotivoDAO dao;

        public RubroMotivoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = RubroMotivoDAO.Instance;
        }

        public Result<ResultadoTabla_RubroMotivo> Insertar(Comando_RubroMotivo comando)
        {
            var result = new Result<ResultadoTabla_RubroMotivo>();

            dao.Transaction(() =>
            {
                try
                {
                    var resultGrupo = new GrupoRubroMotivoRules(getUsuarioLogueado()).GetById(comando.IdGrupo);
                    if (!resultGrupo.Ok)
                    {
                        result.Copy(resultGrupo.Errores);
                        return false;
                    }

                    var motivoPorCategoriaRules = new MotivoPorRubroMotivoRules(getUsuarioLogueado());
                    var resultMotivosAsignadosGrupo = motivoPorCategoriaRules.GetByIdGrupo(comando.IdGrupo);
                    if (!resultMotivosAsignadosGrupo.Ok)
                    {
                        result.Copy(resultMotivosAsignadosGrupo.Errores);
                        return false;
                    }

                    if (resultMotivosAsignadosGrupo.Return.Count != 0)
                    {
                        //de los motivos que ya han sido asignados a este grupo, agarro los que van a ir a esta categoria y los elimino
                        var idsMotivosPorBorrar = resultMotivosAsignadosGrupo.Return.Select(z => z.Id).Intersect(comando.IdsMotivos);
                        foreach (int id in idsMotivosPorBorrar)
                        {
                            var resultDelete = motivoPorCategoriaRules.DeleteById(id);
                            if (!resultDelete.Ok)
                            {
                                result.Copy(resultDelete.Errores);
                                return false;
                            }
                        }
                    }

                    //insert la categoria
                    var categoria = new RubroMotivo();
                    categoria.Grupo = resultGrupo.Return;
                    categoria.Nombre = comando.Nombre;
                    categoria.Observaciones = comando.Observaciones;

                    var resultInsert = Insert(categoria);
                    if (!resultInsert.Ok)
                    {
                        result.Copy(resultInsert.Errores);
                        return false;
                    }

                    var motivoRules = new MotivoRules(getUsuarioLogueado());
                    var motivosPorCategoria = new List<MotivoPorRubroMotivo>();

                    //motivos de la categoria
                    foreach (int id in comando.IdsMotivos)
                    {
                        var mxc = new MotivoPorRubroMotivo();
                        var resultM = motivoRules.GetById(id);
                        if (!resultM.Ok)
                        {
                            result.Copy(resultM.Errores);
                            return false;
                        }

                        mxc.Motivo = resultM.Return;
                        mxc.CategoriaMotivo = resultInsert.Return;

                        var resultMxc = motivoPorCategoriaRules.Insert(mxc);
                        if (!resultMxc.Ok)
                        {
                            result.Copy(resultMxc.Errores);
                            return false;
                        }

                    }

                    result.Return = new ResultadoTabla_RubroMotivo(resultInsert.Return);
                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.Message);
                    result.AddErrorPublico("Error en la inserción del empleado.");
                    return false;
                }
            });

            return result;
        }

        public Result<ResultadoTabla_RubroMotivo> Editar(Comando_RubroMotivo comando)
        {
            var result = new Result<ResultadoTabla_RubroMotivo>();

            dao.Transaction(() =>
            {
                try
                {
                    var resultCategoria= GetById(comando.Id.Value);
                    if (!resultCategoria.Ok)
                    {
                        result.Copy(resultCategoria.Errores);
                        return false;
                    }

                    var motivoPorRubroRules = new MotivoPorRubroMotivoRules(getUsuarioLogueado());
                    //elimino los motivos anteriores de la categoria
                    foreach (MotivoPorRubroMotivo mot in resultCategoria.Return.Motivos)
                    {
                        var resultDelete = motivoPorRubroRules.Delete(mot);
                        if (!resultDelete.Ok)
                        {
                            result.Copy(resultDelete.Errores);
                            return false;
                        }
                    }

                    //busco los motivos ya asignados al grupo
                    var resultMotivosAsignadosGrupo = motivoPorRubroRules.GetByIdGrupo(comando.IdGrupo);
                    if (!resultMotivosAsignadosGrupo.Ok)
                    {
                        result.Copy(resultMotivosAsignadosGrupo.Errores);
                        return false;
                    }

                    if (resultMotivosAsignadosGrupo.Return.Count != 0)
                    {
                        //de los motivos que ya han sido asignados a este grupo, agarro los que van a ir a esta categoria y los elimino
                        var idsMotivosGrupo=resultMotivosAsignadosGrupo.Return.Select(z => z.Motivo.Id).ToList();
                        var idsMotivosPorBorrar = idsMotivosGrupo.Intersect(comando.IdsMotivos).ToList();
                        foreach (int id in idsMotivosPorBorrar)
                        {
                            var maxg = resultMotivosAsignadosGrupo.Return.Where(x => x.Motivo.Id == id).FirstOrDefault();
                            var resultDelete = motivoPorRubroRules.Delete(maxg);
                            if (!resultDelete.Ok)
                            {
                                result.Copy(resultDelete.Errores);
                                return false;
                            }
                        }
                    }

                    //insert el rubro
                    var rubro = resultCategoria.Return;
                    rubro.Nombre = comando.Nombre;
                    rubro.Observaciones = comando.Observaciones;

                    var resultUpdate = Update(rubro);
                    if (!resultUpdate.Ok)
                    {
                        result.Copy(resultUpdate.Errores);
                        return false;
                    }

                    var motivoRules = new MotivoRules(getUsuarioLogueado());
                    var motivosPorCategoria = new List<MotivoPorRubroMotivo>();

                    //motivos de la categoria
                    foreach (int id in comando.IdsMotivos)
                    {
                        var mxc = new MotivoPorRubroMotivo();
                        var resultM = motivoRules.GetById(id);
                        if (!resultM.Ok)
                        {
                            result.Copy(resultM.Errores);
                            return false;
                        }

                        mxc.Motivo = resultM.Return;
                        mxc.CategoriaMotivo = resultUpdate.Return;

                        var resultMxc = motivoPorRubroRules.Insert(mxc);
                        if (!resultMxc.Ok)
                        {
                            result.Copy(resultMxc.Errores);
                            return false;
                        }

                        motivosPorCategoria.Add(resultMxc.Return);
                    }

                    result.Return = new ResultadoTabla_RubroMotivo(resultUpdate.Return);
                    return true;
                }
                catch (Exception e)
                {
                    result.AddErrorInterno(e.Message);
                    result.AddErrorPublico("Error en la inserción del empleado.");
                    return false;
                }
            });

            return result;
        }

        public Result<ResultadoTabla_RubroMotivo> DarDeBaja(int id)
        {
            var result = new Result<ResultadoTabla_RubroMotivo>();

            var r = GetById(id);
            if (!r.Ok)
            {
                result.AddErrorPublico("Error procesando la solicitud.");
                return result;
            }

            var categoria = r.Return;
            categoria.FechaBaja = DateTime.Now;

            var resultInsert = base.Update(categoria);
            if (!resultInsert.Ok)
            {
                result.Copy(resultInsert.Errores);
                return result;
            }

            //devuelvo el resultado del requerimiento creado
            var r_seccion = new ResultadoTabla_RubroMotivo(resultInsert.Return);
            result.Return = r_seccion;
            return result;
        }

        /* Dar de alta */
        public Result<ResultadoTabla_RubroMotivo> DarDeAlta(int id)
        {
            var result = new Result<ResultadoTabla_RubroMotivo>();

            var r = GetById(id);
            if (!r.Ok)
            {
                result.AddErrorPublico("Error procesando la solicitud.");
                return result;
            }

            var rubro = r.Return;
            rubro.FechaBaja = null;

            var resultInsert = base.Update(rubro);
            if (!resultInsert.Ok)
            {
                result.Copy(resultInsert.Errores);
                return result;
            }

            //devuelvo el resultado del rubro creado
            result.Return = new ResultadoTabla_RubroMotivo(resultInsert.Return); 
            return result;
        }

        /*Categorias del grupo*/
        public Result<List<RubroMotivo>> GetRubrosByIdGrupo(int idGrupo)
        {
            return dao.GetRubrosByIdGrupo(idGrupo);
        }
    }
}

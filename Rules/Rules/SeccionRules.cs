using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Comandos;
using Model.Consultas;

namespace Rules.Rules
{
    public class SeccionRules : BaseRules<Seccion>
    {
        private readonly SeccionDAO dao;

        public SeccionRules(UsuarioLogueado data)
            : base(data)
        {
            dao = SeccionDAO.Instance;
        }

        /* override */
        protected override void CorregirDatos(Seccion entity)
        {
            base.CorregirDatos(entity);
            entity.Nombre = entity.Nombre.ToUpper();
        }

        public override Result<int> BuscarCantidadDuplicados(Seccion entity)
        {
            var result = new Result<int>();

            int? id = null;
            if (entity.Id != 0)
            {
                id = entity.Id;
            }
            var resultConsulta = dao.GetCantidadDuplicados(id, entity.Nombre, entity.Area.Id);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta);
                return result;
            }

            result.Return = resultConsulta.Return;
            return result;
        }

        public override string MensajeDuplicado(Seccion entity)
        {
            return "Ya existe una sección con ese nombre";
        }

        public override Result<Seccion> ValidateDatosNecesarios(Seccion entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Nombre
            if (string.IsNullOrEmpty(entity.Nombre))
            {
                result.AddErrorPublico("Debe ingresar el nombre");
            }

            return result;
        }

        /* Insert */
        public Result<Resultado_Seccion> Insertar(Comando_Seccion comando)
        {
            var result = new Result<Resultado_Seccion>();
            var resultValidar = Validar(comando);
            if (!resultValidar.Ok)
            {
                result.Copy(resultValidar.Errores);
                return result;
            }

            dao.Transaction(() =>
            {
                var seccion = new Seccion();
                var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById((int)comando.IdArea);
                seccion.Observaciones = comando.Observaciones;
                seccion.Nombre = comando.Nombre;
                seccion.Area = resultArea.Return;

                var resultInsert = base.Insert(seccion);
                if (!resultInsert.Ok)
                {
                    result.Copy(resultInsert.Errores);
                    return false;
                }

                if (comando.IdsEmpleados != null)
                {
                    foreach (int id in comando.IdsEmpleados)
                    {
                        //consulto el empleado
                        var resultEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).GetByIdObligatorio(id);
                        if (!resultEmpleado.Ok)
                        {
                            result.Copy(resultEmpleado.Errores);
                            return false;
                        }

                        //consulto si ya pertenece a una sección
                        if (resultEmpleado.Return.Seccion != null)
                        {
                            result.AddErrorPublico("El empleado ya pertenece a una sección.");


                            return false;
                        }

                        resultEmpleado.Return.Seccion = resultInsert.Return;
                        var resultUpdateEmpleado = new EmpleadoPorAreaRules(getUsuarioLogueado()).Update(resultEmpleado.Return);
                        if (!resultUpdateEmpleado.Ok)
                        {
                            result.Copy(resultUpdateEmpleado.Errores);
                            return false;
                        }

                        //devuelvo el resultado del requerimiento creado
                        var r_seccion = new Resultado_Seccion(resultInsert.Return);
                        result.Return = r_seccion;
                    }
                }

                return true;
            });

            return result;
        }

        public Result<Seccion> Validar(Comando_Seccion comando)
        {
            var result = new Result<Seccion>();
            if (getUsuarioLogueado().Areas.Where(x => x.Id == comando.IdArea).Count() == 0)
            {
                result.AddErrorPublico("No tiene permisos para agregar secciones a esa área.");
                return result;
            }

            return result;
        }

        /* Update*/
        public Result<Resultado_Seccion> Update(Comando_Seccion comando)
        {
            var result = new Result<Resultado_Seccion>();
            var resultValidar = Validar(comando);
            if (!resultValidar.Ok)
            {
                result.Copy(resultValidar.Errores);
                return result;
            }

            var seccion = new Seccion();
            if (!comando.Id.HasValue)
            {
                result.AddErrorPublico("Error al actualizar la sección.");
                return result;
            }

            dao.Transaction(() =>
            {
                var resultConsulta = GetById((int)comando.Id);
                if (!resultConsulta.Ok)
                {
                    result.AddErrorPublico("Error procesando la solicitud.");
                    return false;
                }

                seccion = resultConsulta.Return;
                seccion.Observaciones = comando.Observaciones;
                seccion.Nombre = comando.Nombre;

                var resultUpdate = base.Update(seccion);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultUpdate.Errores);
                    return false;
                }

                var empleadoPorAreaRules = new EmpleadoPorAreaRules(getUsuarioLogueado());
                //borro todas las relaciones con empleados
                var empleadosBorrar = seccion.GetEmpleados().Select(x => x).Where(x => !comando.IdsEmpleados.Contains(x.Id));
                foreach (EmpleadoPorArea empleado in empleadosBorrar)
                {
                    empleado.Seccion = null;
                    var resultDelete = empleadoPorAreaRules.Update(empleado);
                    if (!resultDelete.Ok)
                    {
                        result.Copy(resultDelete.Errores);
                        return false;
                    }
                }

                if (comando.IdsEmpleados != null)
                {
                    foreach (int id in comando.IdsEmpleados)
                    {
                        var emp = seccion.GetEmpleados().Select(x => x).Where(x => x.Id == id).ToList();
                        if (emp.Count() != 0)
                        {
                            continue;
                        }

                        var resultEmpleado = empleadoPorAreaRules.GetByIdObligatorio(id);
                        if (!resultEmpleado.Ok)
                        {
                            result.Copy(resultEmpleado.Errores);
                            return false;
                        }

                        //consulto si ya pertenece a una sección
                        if (resultEmpleado.Return.Seccion != null)
                        {
                            result.AddErrorPublico("El empleado ya pertenece a una sección.");


                            return false;
                        }

                        var empleado = resultEmpleado.Return;
                        empleado.Seccion = resultUpdate.Return;
                        var resultInsertEmpleado = empleadoPorAreaRules.Update(empleado);
                        if (!resultInsertEmpleado.Ok)
                        {
                            result.Copy(resultInsertEmpleado.Errores);
                            return false;
                        }
                    }
                }

                //devuelvo el resultado del requerimiento creado
                var r_seccion = new Resultado_Seccion(resultUpdate.Return);
                result.Return = r_seccion;
                return true;
            });

            return result;
        }

        /* Delete */
        public Result<Resultado_Seccion> DarDeBaja(int id)
        {
            var result = new Result<Resultado_Seccion>();

            var r = GetById(id);
            if (!r.Ok)
            {
                result.AddErrorPublico("Error procesando la solicitud.");
                return result;
            }

            var seccion = r.Return;
            var resultValidar = ValidarDarDeBaja(seccion);
            if (!resultValidar.Ok)
            {
                result.Copy(resultValidar.Errores);
                return result;
            }

            seccion.FechaBaja = DateTime.Now;

            var resultInsert = base.Update(seccion);
            if (!resultInsert.Ok)
            {
                result.Copy(resultInsert.Errores);
                return result;
            }

            //le quito la seccion a los empleados que tengo
            foreach (EmpleadoPorArea empleado in seccion.Empleados){
                empleado.Seccion=null;
                var resultUpdate = new EmpleadoPorAreaRules(getUsuarioLogueado()).Update(empleado);
                if (!resultUpdate.Ok)
                {
                    result.Copy(resultInsert.Errores);
                    return result;
                }
            }
            

            //devuelvo el resultado del requerimiento creado
            var r_seccion = new Resultado_Seccion(resultInsert.Return);
            result.Return = r_seccion;
            return result;
        }

        public Result<Seccion> ValidarDarDeBaja(Seccion comando)
        {
            var result = new Result<Seccion>();

            if (getUsuarioLogueado().Areas.Where(x => x.Id == comando.Area.Id).Count() == 0)
            {
                result.AddErrorPublico("No tiene permisos para agregar secciones a esa área.");
                return result;
            }

            return result;
        }

        /* Dar de alta */
        public Result<Resultado_Seccion> DarDeAlta(int id)
        {
            var result = new Result<Resultado_Seccion>();

            var r = GetById(id);
            if (!r.Ok)
            {
                result.AddErrorPublico("Error procesando la solicitud.");
                return result;
            }

            var seccion = r.Return;
            var resultValidar = ValidarDarDeBaja(seccion);
            if (!resultValidar.Ok)
            {
                result.Copy(resultValidar.Errores);
                return result;
            }

            seccion.FechaBaja = null;

            var resultInsert = base.Update(seccion);
            if (!resultInsert.Ok)
            {
                result.Copy(resultInsert.Errores);
                return result;
            }

            //devuelvo el resultado del requerimiento creado
            var r_seccion = new Resultado_Seccion(resultInsert.Return);
            result.Return = r_seccion;
            return result;
        }

        /* Consultas*/
        public Result<List<Resultado_Seccion>> GetByFilters(Consulta_Seccion consulta)
        {
            var result = new Result<List<Resultado_Seccion>>();
            consulta.DadosDeBaja = false;
            var resultConsulta = dao.GetByFilters(consulta);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = Resultado_Seccion.ToList(resultConsulta.Return);
            return result;
        }

        public Result<List<Resultado_Seccion>> GetByArea(int idArea)
        {
            return GetByFilters(new Consulta_Seccion(null, false, idArea));
        }

        public Result<List<int>> GetIdsByArea(int idArea)
        {
            return dao.GetIdsByFilters(new Consulta_Seccion(null, false, idArea));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Model.Consultas;
using Model.Comandos;
using NHibernate.Criterion;


namespace Rules.Rules
{
    public class ZonaRules : BaseRules<Zona>
    {
        private readonly ZonaDAO dao;

        public ZonaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ZonaDAO.Instance;
        }

        public override Result<int> BuscarCantidadDuplicados(Zona entity)
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

        public override string MensajeDuplicado(Zona entity)
        {
            return "Ya existe una zona con el nombre: " + entity.Nombre;
        }

        public override Result<Zona> ValidateDatosNecesarios(Zona entity)
        {
            var result = base.ValidateDatosNecesarios(entity);

            //Nombre
            if (string.IsNullOrEmpty(entity.Nombre))
            {
                result.AddErrorPublico("Debe ingresar el nombre");
            }

            return result;
        }


        public Result<Resultado_Zona> Insertar(Comando_Zona comando)
        {
            var resultado = new Result<Resultado_Zona>();

            var resultadoArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetByIdObligatorio(comando.IdArea);
            if (!resultadoArea.Ok)
            {
                resultado.Errores.Copy(resultadoArea.Errores);
                return resultado;
            }


            var resultadoTransaccion = dao.Transaction(() =>
            {
                var zona = new Zona();
                zona.Nombre = comando.Nombre;
                zona.Area = resultadoArea.Return;

                var resultadoColor = dao.GetColorDisponible(comando.IdArea);
                if (!resultadoColor.Ok)
                {
                    resultado.Copy(resultadoColor.Errores);
                    return false;
                }
                if (!resultadoColor.Return.HasValue)
                {
                    resultado.AddErrorPublico("No hay colores disponibles");
                    return false;
                }

                zona.Color = new BaseDAO<Color>().GetByIdObligatorio(resultadoColor.Return.Value).Return;

                var resultadoInsertar = base.Insert(zona);
                if (!resultadoInsertar.Ok)
                {
                    resultado.Errores.Copy(resultadoInsertar.Errores);
                    return false;
                }

                if (comando.IdsBarrios == null || comando.IdsBarrios.Count() == 0)
                {
                    resultado.AddErrorPublico("Seleccione algun barrio");
                    return false;
                }

                foreach (var id in comando.IdsBarrios)
                {
                    var resultadoBarrio = new BarrioRules(getUsuarioLogueado()).GetByIdCatastro(id);
                    if (!resultadoBarrio.Ok)
                    {
                        resultado.Copy(resultadoBarrio.Errores);
                        return false;
                    }


                    if (resultadoBarrio.Return == null)
                    {
                        resultado.AddErrorPublico("El barrio indicado no existe");
                        return false;
                    }

                    var resultadoValidar = new BarrioPorZonaRules(getUsuarioLogueado()).ValidarBarrio(null, id, comando.IdArea);
                    if (!resultadoValidar.Return)
                    {
                        resultado.AddErrorPublico("El barrio " + resultadoBarrio.Return.Nombre + " ya se encuentra en otra zona del area " + resultadoInsertar.Return.Area.Nombre);
                        return false;
                    }

                    BarrioPorZona bxz = new BarrioPorZona();
                    bxz.Barrio = resultadoBarrio.Return;
                    bxz.Zona = resultadoInsertar.Return;
                    var resultadoInsertarBarrio = new BarrioPorZonaRules(getUsuarioLogueado()).Insert(bxz);
                    if (!resultadoInsertarBarrio.Ok)
                    {
                        resultado.Copy(resultadoInsertarBarrio.Errores);
                        return false;
                    }
                }


                resultado.Return = new Resultado_Zona(resultadoInsertar.Return);
                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorInterno("Error en la transaccion");
            }

            return resultado;
        }

        public Result<Resultado_Zona> Actualizar(Comando_Zona comando)
        {
            var resultado = new Result<Resultado_Zona>();


            var resultadoTransaccion = dao.Transaction(() =>
            {
                var resultadoZona = GetByIdObligatorio(comando.Id.Value);
                if (!resultadoZona.Ok)
                {
                    resultado.Copy(resultadoZona.Errores);
                    return false;
                }

                var zona = resultadoZona.Return;
                zona.Nombre = comando.Nombre;

                var resultadoInsertar = base.Update(zona);
                if (!resultadoInsertar.Ok)
                {
                    resultado.Errores.Copy(resultadoInsertar.Errores);
                    return false;
                }

                if (comando.IdsBarrios == null || comando.IdsBarrios.Count() == 0)
                {
                    resultado.AddErrorPublico("Seleccione algun barrio");
                    return false;
                }

                //Doy de baja todos los barrios anteriores
                var barriosAnteriores = dao.GetSession().QueryOver<BarrioPorZona>().Where(x => x.Zona.Id == zona.Id && x.FechaBaja == null).List().ToList();
                foreach (var b in barriosAnteriores)
                {
                    b.FechaBaja = DateTime.Now;
                    var resultadoUpdate = new BarrioPorZonaRules(getUsuarioLogueado()).Update(b);
                    if (!resultadoUpdate.Ok)
                    {
                        resultado.Copy(resultadoUpdate.Errores);
                        return false;
                    }
                }

                //Inserto los barrios nuevos
                foreach (var id in comando.IdsBarrios)
                {
                    var resultadoBarrio = new BarrioRules(getUsuarioLogueado()).GetByIdCatastro(id);
                    if (!resultadoBarrio.Ok)
                    {
                        resultado.Copy(resultadoBarrio.Errores);
                        return false;
                    }


                    if (resultadoBarrio.Return == null)
                    {
                        resultado.AddErrorPublico("El barrio indicado no existe");
                        return false;
                    }

                    var resultadoValidar = new BarrioPorZonaRules(getUsuarioLogueado()).ValidarBarrio(zona.Id, id, zona.Area.Id);
                    if (!resultadoValidar.Return)
                    {
                        resultado.AddErrorPublico("El barrio " + resultadoBarrio.Return.Nombre + " ya se encuentra en otra zona del area " + resultadoInsertar.Return.Area.Nombre);
                        return false;
                    }

                    BarrioPorZona bxz = new BarrioPorZona();
                    bxz.Barrio = resultadoBarrio.Return;
                    bxz.Zona = resultadoInsertar.Return;
                    var resultadoInsertarBarrio = new BarrioPorZonaRules(getUsuarioLogueado()).Insert(bxz);
                    if (!resultadoInsertarBarrio.Ok)
                    {
                        resultado.Copy(resultadoInsertarBarrio.Errores);
                        return false;
                    }
                }


                resultado.Return = new Resultado_Zona(resultadoInsertar.Return);
                return true;
            });

            if (!resultadoTransaccion)
            {
                resultado.AddErrorInterno("Error en la transaccion");
            }

            return resultado;
        }

        public Result<Resultado_Zona> DarDeBaja(int id)
        {
            var result = new Result<Resultado_Zona>();

            var resultadoBusqueda = GetByIdObligatorio(id);
            if (!resultadoBusqueda.Ok)
            {
                result.Copy(resultadoBusqueda.Errores);
                return result;
            }

            resultadoBusqueda.Return.FechaBaja = DateTime.Now;

            //Actualizo
            var resultadoUpdate = base.Update(resultadoBusqueda.Return);
            if (!resultadoUpdate.Ok)
            {
                result.Copy(resultadoUpdate.Errores);
                return result;
            }

            //devuelvo el resultado del movil 
            result.Return = new Resultado_Zona(resultadoUpdate.Return);
            return result;
        }

        public Result<Resultado_Zona> DarDeAlta(int id)
        {
            var resultado = new Result<Resultado_Zona>();

            var resultadoBusqueda = GetById(id);
            if (!resultadoBusqueda.Ok)
            {
                resultado.Copy(resultadoBusqueda.Errores);
                return resultado;
            }

            if (resultadoBusqueda.Return == null)
            {
                resultado.AddErrorPublico("La entidad no existe");
                return resultado;
            }

            var zona = resultadoBusqueda.Return;
            var resultadoConsulta = new BarrioPorZonaRules(getUsuarioLogueado()).GetByFilters(new Consulta_BarrioPorZona()
            {
                IdsArea = new List<int>() { zona.Area.Id },
                IdsBarrio = zona.BarriosPorZona.Where(x => x.FechaBaja == null).Select(x => x.Barrio.Id).ToList()
            });

            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return.Count != 0)
            {
                resultado.AddErrorPublico("Algun barrio de su zona ya forma parte de una zona activa");
                return resultado;
            }

            zona.FechaBaja = null;

            //Actualizo
            var resultadoUpdate = base.Update(zona);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            //devuelvo el resultado del movil 
            resultado.Return = new Resultado_Zona(resultadoUpdate.Return);
            return resultado;
        }

        public Result<List<Resultado_Zona>> GetByFilters(Consulta_Zona consulta)
        {
            var result = new Result<List<Resultado_Zona>>();
            consulta.DadosDeBaja = false;
            var resultConsulta = dao.GetByFilters(consulta);
            if (!resultConsulta.Ok)
            {
                result.Copy(resultConsulta.Errores);
                return result;
            }

            result.Return = Resultado_Zona.ToList(resultConsulta.Return);
            return result;
        }

        public Result<ResultadoTabla<ResultadoTabla_Zona>> GetResultadoTablaByFilters(Consulta_Zona consulta)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_Zona>>();
            var resultadoIds = dao.GetIdsByFilters(consulta);
            if (!resultadoIds.Ok)
            {
                resultado.Copy(resultadoIds.Errores);
                return resultado;
            }

            return dao.GetResultadoTablaByIds(LIMITE_CANTIDAD_TABLA, resultadoIds.Return);
        }

        int LIMITE_CANTIDAD_TABLA = 5000;
        public Result<ResultadoTabla_Zona> GetResultadoTablaById(int id)
        {
            var resultado = new Result<ResultadoTabla_Zona>();

            var resultadoConsulta = GetResultadoTablaByIds(new List<int>() { id });
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            if (resultadoConsulta.Return == null || resultadoConsulta.Return.Data.Count == 0)
            {
                resultado.Return = null;
                return resultado;
            }

            resultado.Return = resultadoConsulta.Return.Data[0];
            return resultado;
        }

        public Result<ResultadoTabla<ResultadoTabla_Zona>> GetResultadoTablaByIds(List<int> ids)
        {
            return dao.GetResultadoTablaByIds(LIMITE_CANTIDAD_TABLA, ids);
        }


    }
}

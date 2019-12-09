using System;
using System.Linq;
using Model;
using Model.Entities;
using DAO.DAO;
using System.Collections.Generic;
using Model.Resultados;
using Model.Comandos;

namespace Rules.Rules
{
    public class TareaPorAreaRules : BaseRules<TareaPorArea>
    {

        private readonly TareaPorAreaDAO dao;

        public TareaPorAreaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = TareaPorAreaDAO.Instance;
        }
        
        //public Result<List<Resultado_Funcion>> GetByIdArea(int idArea)
        //{
        //    var resultado = new Result<List<Resultado_Funcion>>();
        //    var list = new List<int>();
        //    list.Add(idArea);

        //    var result = dao.GetByIdsArea(list, false);
        //    if (!result.Ok)
        //    {
        //        resultado.AddErrorPublico("Error consultando las funciones");
        //        resultado.Copy(result.Errores);
        //        return resultado;
        //    }

        //    resultado.Return = Resultado_Funcion.ToList(result.Return);
        //    return resultado;
        //}

        //public Result<List<Resultado_Funcion>> GetByMisAreas(bool? dadosDeBaja)
        //{
        //    var resultado = new Result<List<Resultado_Funcion>>();
        //    var result = dao.GetByIdsArea(getUsuarioLogueado().IdsAreas, dadosDeBaja);
        //    if (!result.Ok)
        //    {
        //        resultado.AddErrorPublico("Error consultando las funciones");
        //        resultado.Copy(result.Errores);
        //        return resultado;
        //    }

        //    resultado.Return = Resultado_Funcion.ToList(result.Return);
        //    return resultado;
        //}
        public Result<ResultadoTabla_TareaPorArea> Insertar(Comando_TareaPorArea comando)
        {
            var result = new Result<ResultadoTabla_TareaPorArea>();

            if (getUsuarioLogueado().IdsAreas.Select(x => x == comando.IdArea).Count() == 0)
            {
                result.AddErrorPublico("Usted no tiene permisos para agregar tareas a ésta área.");
                return result;
            }

            var tarea = new TareaPorArea();
            tarea.Nombre = comando.Nombre.ToUpper();
            tarea.Observaciones = comando.Observaciones;

            var resultArea = new _CerrojoAreaRules(getUsuarioLogueado()).GetById(comando.IdArea);
            if (!resultArea.Ok)
            {
                result.AddErrorPublico("Error al consultar alguno de los datos");
                return result;
            }
            tarea.Area = resultArea.Return;

            var resultValidar = Equals(tarea);
            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al insertar la tarea");
                return result;
            }

            if (resultValidar.Return)
            {
                result.AddErrorPublico("Ya existe una tarea con el nombre '" + tarea.Nombre + "'");
                return result;
            }

            var resultInsert = Insert(tarea);
            if (!resultInsert.Ok)
            {
                result.AddErrorPublico("Error al insertar la tarea");
                return result;
            }

            result.Return = new ResultadoTabla_TareaPorArea(resultInsert.Return);
            return result;
        }

        public Result<bool> Equals(TareaPorArea obj)
        {
            return dao.Equals(obj);
        }

        //Acciones
        public Result<bool> DarDeBaja(int id)
        {
            var result = new Result<bool>();

            var resultConsulta = GetByIdObligatorio(id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar la tarea");
                return result;
            }

            var resultUpdate = Delete(resultConsulta.Return);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar la tarea");
                return result;
            }

            result.Return = true;
            return result;
        }

        public Result<bool> DarDeAlta(int id)
        {
            var result = new Result<bool>();

            var resultConsulta = GetById(id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar la tarea");
                return result;
            }

            resultConsulta.Return.FechaBaja = null;
            var resultUpdate = Update(resultConsulta.Return);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar la tarea");
                return result;
            }

            result.Return = true;
            return result;
        }

        public Result<bool> EditarNombre(Comando_TareaPorArea_Editar comando)
        {
            var resultado = new Result<bool>();

            //Lo busco
            var consulta = GetByIdObligatorio(comando.IdTarea);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;
            ot.Nombre = comando.Valor;

            var resultadoUpdate = base.Update(ot);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        public Result<bool> EditarDescripcion(Comando_TareaPorArea_Editar comando)
        {
            var resultado = new Result<bool>();

            //Lo busco
            var consulta = GetByIdObligatorio(comando.IdTarea);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            var ot = consulta.Return;
            ot.Observaciones = comando.Valor;

            var resultadoUpdate = base.Update(ot);
            if (!resultadoUpdate.Ok)
            {
                resultado.Copy(resultadoUpdate.Errores);
                return resultado;
            }

            resultado.Return = true;
            return resultado;
        }

        //Detalles
        public Result<Resultado_TareaPorArea> GetDetalleById(int id)
        {
            var resultado = new Result<Resultado_TareaPorArea>();

            //Lo busco
            var consulta = GetById(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            resultado.Return = new Resultado_TareaPorArea(consulta.Return);
            return resultado;
        }

        public Result<ResultadoTabla_TareaPorArea> GetResultadoTablaById(int id)
        {
            var resultado = new Result<ResultadoTabla_TareaPorArea>();

            //Lo busco
            var consulta = GetById(id);
            if (!consulta.Ok)
            {
                resultado.Copy(consulta.Errores);
                return resultado;
            }

            resultado.Return = new ResultadoTabla_TareaPorArea(consulta.Return);
            return resultado;
        }

        //Requerimiento
        public Result<List<ResultadoTabla_TareaPorArea>> GetByIdRequerimientoYArea(int idRequerimiento)
        {
            var resultado = new Result<List<ResultadoTabla_TareaPorArea>>();

            //Lo busco
            var resultRequerimiento = new RequerimientoRules(getUsuarioLogueado()).GetById(idRequerimiento);
            if (!resultRequerimiento.Ok)
            {
                resultado.Copy(resultRequerimiento.Errores);
                return resultado;
            }

            var resultPorArea = GetByIdArea(resultRequerimiento.Return.AreaResponsable.Id);
            if (!resultPorArea.Ok)
            {
                resultado.Copy(resultPorArea.Errores);
                return resultado;
            }

            resultado = resultPorArea;

            foreach (TareaPorAreaPorRequerimiento t in resultRequerimiento.Return.getTareas())
            {
                var filtrar = resultPorArea.Return.Where(x => x.Id == t.Tarea.Id);
                if (filtrar.Count() == 0)
                {
                    resultado.Return.Add(new ResultadoTabla_TareaPorArea(t.Tarea));
                }
            }


            return resultado;
        }

        public Result<List<ResultadoTabla_TareaPorArea>> GetByIdRequerimiento(int idRequerimiento)
        {
            var resultado = new Result<List<ResultadoTabla_TareaPorArea>>();

            //Lo busco
            var resultRequerimiento = new RequerimientoRules(getUsuarioLogueado()).GetById(idRequerimiento);
            if (!resultRequerimiento.Ok)
            {
                resultado.Copy(resultRequerimiento.Errores);
                return resultado;
            }

            resultado.Return=new List<ResultadoTabla_TareaPorArea>();
            foreach (TareaPorAreaPorRequerimiento t in resultRequerimiento.Return.getTareas()) 
            {
                resultado.Return.Add(new ResultadoTabla_TareaPorArea(t.Tarea));
            }

            return resultado;
        }

        public Result<List<ResultadoTabla_TareaPorArea>> GetByIdArea(int p)
        {
            var resultado = new Result<List<ResultadoTabla_TareaPorArea>>();
            var list = new List<int>();
            list.Add(p);
            var resultTareas = dao.GetByIdsArea(list, false);
            if (!resultTareas.Ok)
            {
                resultado.Copy(resultTareas.Errores);
                return resultado;
            }

            resultado.Return = ResultadoTabla_TareaPorArea.ToList(resultTareas.Return);
            return resultado;
        }

        public Result<int> GetCantidadByArea(int idArea)
        {
            var resultado = new Result<int>();
            var resultTareas = dao.GetCantidadByArea(idArea);
            if (!resultTareas.Ok)
            {
                resultado.Copy(resultTareas.Errores);
                return resultado;
            }

            resultado.Return = resultTareas.Return;
            return resultado;
        }
    }
}

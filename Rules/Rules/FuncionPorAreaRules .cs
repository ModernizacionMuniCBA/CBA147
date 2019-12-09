using System;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using System.Collections.Generic;
using Model.Comandos;

namespace Rules.Rules
{
    public class FuncionPorAreaRules : BaseRules<FuncionPorArea>
    {
  
        private readonly FuncionPorAreaDAO dao;

        public FuncionPorAreaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = FuncionPorAreaDAO.Instance;
        }

        public Result<List<Resultado_Funcion>> GetByIdArea(int idArea)
        {
            var resultado = new Result<List<Resultado_Funcion>>();
            var list = new List<int>();
            list.Add(idArea);

            var result = dao.GetByIdsArea(list, false);
            if (!result.Ok)
            {
                resultado.AddErrorPublico("Error consultando las funciones");
                resultado.Copy(result.Errores);
                return resultado;
            }

            resultado.Return = Resultado_Funcion.ToList(result.Return);
            return resultado;
        }

        public Result<List<Resultado_Funcion>> GetByMisAreas(bool? dadosDeBaja)
        {
            var resultado = new Result<List<Resultado_Funcion>>();
            var result = dao.GetByIdsArea(getUsuarioLogueado().IdsAreas, dadosDeBaja);
            if (!result.Ok)
            {
                resultado.AddErrorPublico("Error consultando las funciones");
                resultado.Copy(result.Errores);
                return resultado;
            }

            resultado.Return = Resultado_Funcion.ToList(result.Return);
            return resultado;
        }
        public Result<Resultado_Funcion> Insert(Comando_Funcion comando)
        {
            var result = new Result<Resultado_Funcion>();

            var funcion = new FuncionPorArea();
            funcion.Nombre = comando.Nombre.ToUpper();

            if (getUsuarioLogueado().IdsAreas.Select(x => x == comando.IdArea).Count()==0)
            {
                result.AddErrorPublico("Usted no tiene permisos para agregar funciones a ésta área.");
                return result;
            }

            var resultArea= new _CerrojoAreaRules(getUsuarioLogueado()).GetById(comando.IdArea);
            if (!resultArea.Ok)
            {
                result.AddErrorPublico("Error al consultar alguno de los datos");
                return result;
            }
            funcion.Area = resultArea.Return;

            var resultValidar = Equals(funcion);
            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al insertar la funcion");
                return result;
            }

            if (resultValidar.Return)
            {
                result.AddErrorPublico("Ya existe una función con el nombre '" + funcion.Nombre + "'");
                return result;
            }

            var resultInsert = Insert(funcion);
            if (!resultInsert.Ok)
            {
                result.AddErrorPublico("Error al insertar la funcion");
                return result;
            }

            result.Return = new Resultado_Funcion(resultInsert.Return);
            return result;
        }

        public Result<Resultado_Funcion> Update(Comando_Funcion comando)
        {
            var result = new Result<Resultado_Funcion>();

            var resultConsulta = new FuncionPorAreaRules(getUsuarioLogueado()).GetById((int)comando.Id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar la funcion");
                return result;
            }

            var funcion = resultConsulta.Return;
            funcion.Nombre = comando.Nombre.ToUpper();

            var resultValidar = Equals(funcion);

            if (!resultValidar.Ok)
            {
                result.AddErrorPublico("Error al actualizar la funcion");
                return result;
            }

            var resultUpdate = Insert(funcion);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar la funcion");
                return result;
            }

            result.Return = new Resultado_Funcion(resultUpdate.Return);
            return result;
        }

        public Result<bool> Equals(FuncionPorArea obj)
        {
            return dao.Equals(obj);
        }

        public Result<Resultado_Funcion> DarDeBaja(Comando_Funcion comando)
        {
            var result = new Result<Resultado_Funcion>();

            var resultConsulta = GetByIdObligatorio((int)comando.Id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar la función");
                return result;
            }

            var resultUpdate = Delete(resultConsulta.Return);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar la función");
                return result;
            }

            result.Return = new Resultado_Funcion(resultUpdate.Return);
            return result;
        }

        public Result<Resultado_Funcion> DarDeAlta(Comando_Funcion comando)
        {
            var result = new Result<Resultado_Funcion>();

            var resultConsulta = GetById((int)comando.Id);
            if (!resultConsulta.Ok)
            {
                result.AddErrorPublico("Error al actualizar la función");
                return result;
            }

            resultConsulta.Return.FechaBaja = null;
            var resultUpdate = Update(resultConsulta.Return);
            if (!resultUpdate.Ok)
            {
                result.AddErrorPublico("Error al actualizar la función");
                return result;
            }

            result.Return = new Resultado_Funcion(resultUpdate.Return);
            return result;
        }
    }
}

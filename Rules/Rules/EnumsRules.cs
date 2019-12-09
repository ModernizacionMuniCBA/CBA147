using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;

namespace Rules.Rules
{
    public class EnumsRules
    {

        public EnumsRules(UsuarioLogueado data)
        {

       }

        public Result<List<Resultado_Enums>> GetAll(Type tipo)
        {

            var resultado = new Resultado_Enums();
            var values = Enum.GetValues(tipo);
            var lista = new List<Resultado_Enums>();
            
            foreach(var item in values){
                resultado = new Resultado_Enums();
                resultado.KeyValue = (int)item;
                resultado.Nombre = item.ToString();
                lista.Add(resultado);
            }

            var result=new  Result<List<Resultado_Enums>>();
            result.Return=lista;
            return result;
        }
    }
}

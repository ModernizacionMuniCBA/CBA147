using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_EdificioMunicipal 
    {
        public string Nombre { get; set; }
        public int Id { get; set; }

        public int IdCategoria { get; set; }
        public ResultadoTabla_EdificioMunicipal()
            : base()
        {

        }

        public ResultadoTabla_EdificioMunicipal(EdificioMunicipal entity)
        {
            if (entity == null)
            {
                return;
            }
            Id = entity.Id;
            Nombre = entity.Nombre;
            IdCategoria = entity.Categoria.Id;
          
        }

        public static List<ResultadoTabla_EdificioMunicipal> ToList(List<EdificioMunicipal> list)
        {
            return list.Select(x => new ResultadoTabla_EdificioMunicipal(x)).ToList();
        }
    }
}

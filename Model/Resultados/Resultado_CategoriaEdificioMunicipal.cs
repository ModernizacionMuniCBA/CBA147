using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]   
    public class Resultado_CategoriaEdificioMunicipal 
    {
        public string Nombre { get; set; }
        public int Id{get;set;}
        public Resultado_CategoriaEdificioMunicipal()
            : base()
        {

        }

        public Resultado_CategoriaEdificioMunicipal(CategoriaEdificioMunicipal entity)
        {
            if (entity == null)
            {
                return;
            }
            Id = entity.Id;
            Nombre = entity.Nombre;
    
        }

        public static List<Resultado_CategoriaEdificioMunicipal> ToList(List<CategoriaEdificioMunicipal> list)
        {
            return list.Select(x => new Resultado_CategoriaEdificioMunicipal(x)).ToList();
        }
    }
}

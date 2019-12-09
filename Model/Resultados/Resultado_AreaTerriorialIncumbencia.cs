using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_AreaTerriorialIncumbencia 
    {
        public string Nombre { get; set; }
        public string Poligono{ get; set; }
        public int Id{ get; set; }


        public Resultado_AreaTerriorialIncumbencia():base()
        {

        }

        public Resultado_AreaTerriorialIncumbencia(TerritorioIncumbencia area)
        {
            if (area == null)
            {
                return;
            }

            Id = area.Id;
            Nombre = area.Nombre;
            Poligono = area.Poligono;
        }

        public static List<Resultado_AreaTerriorialIncumbencia> ToList(List<TerritorioIncumbencia> list)
        {
            return list.Select(x => new Resultado_AreaTerriorialIncumbencia(x)).ToList();
        }

    }
}

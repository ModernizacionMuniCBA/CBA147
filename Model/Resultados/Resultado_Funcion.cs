using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Funcion 
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdArea { get; set; }
        public string AreaNombre { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }

        public Resultado_Funcion()
        {

        }
        public Resultado_Funcion(FuncionPorArea entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
            IdArea = entity.Area.Id;
            AreaNombre = entity.Area.Nombre;
            FechaAlta = entity.FechaAlta;
            FechaBaja = entity.FechaBaja;
        }
        public static List<Resultado_Funcion> ToList(List<FuncionPorArea> list)
        {
            return list.Select(x => new Resultado_Funcion(x)).ToList();
        }
    }
}

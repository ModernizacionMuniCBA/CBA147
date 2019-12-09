using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Seccion : Resultado_Base<Seccion>
    {
        public string Nombre { get; set; }
        public int IdArea { get; set; }
        public string AreaNombre { get; set; }
        public List<ResultadoTabla_Empleado> Empleados { get; set; }

        public Resultado_Seccion()
            : base()
        {

        }

        public Resultado_Seccion(Seccion entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            IdArea = entity.Area.Id;
            AreaNombre = entity.Area.Nombre;
            if (entity.Empleados != null)
               Empleados = ResultadoTabla_Empleado.ToList( entity.GetEmpleados());
        }
        public static List<Resultado_Seccion> ToList(List<Seccion> list)
        {
            return list.Select(x => new Resultado_Seccion(x)).ToList();
        }
    }
}

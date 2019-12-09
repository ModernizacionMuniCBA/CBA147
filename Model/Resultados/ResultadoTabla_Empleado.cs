using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_Empleado
    {

        public int Id { get; set; }
        public int? IdUsuarioCerrojoEmpleado { get; set; }
        public DateTime? FechaAlta { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public List<int> IdsFunciones{ get; set; }
        public int? IdSeccion { get; set; }
        public string NombreSeccion { get; set; }
        public bool SexoMasculino { get; set; }

        public ResultadoTabla_Empleado() {

}

        public ResultadoTabla_Empleado(EmpleadoPorArea e)  {
            Id = e.Id;
            IdUsuarioCerrojoEmpleado = e.UsuarioEmpleado.Id;
            FechaAlta = e.FechaAlta;
            Nombre = e.UsuarioEmpleado.Nombre;
            Apellido = e.UsuarioEmpleado.Apellido;
            Dni = e.UsuarioEmpleado.Dni;
            IdsFunciones = e.FuncionesPorEmpleado.Select(x=>x.Funcion.Id).ToList();
            if (e.Seccion != null) { 
            IdSeccion = e.Seccion.Id;
            NombreSeccion = e.Seccion.Nombre;
            SexoMasculino = e.UsuarioEmpleado.SexoMasculino;
            }
        }

        public static List<ResultadoTabla_Empleado> ToList(List<EmpleadoPorArea> list)
        {
            return list.Select(x => new ResultadoTabla_Empleado(x)).ToList();
        }
    }
}

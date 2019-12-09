using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_EmpleadoPanel
    {

        public int Id { get; set; }
        public int? IdUsuarioCerrojoEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string Funciones { get; set; }
        public int? IdSeccion { get; set; }
        public string NombreSeccion { get; set; }
        public string Cargo { get; set; }
        public string IdentificadorFotoPersonal { get; set; }
        public string IdentificadorFotoRegistroCivil { get; set; }
        public bool SexoMasculino { get; set; }
        //Orden Trabajo
        public string NumeroOrdenTrabajo { get; set; }
        public int? IdOrdenTrabajo { get; set; }
        public int CantidadDiasOrdenTrabajo { get; set; }
        public string NombreEstadoOrdenTrabajo { get; set; }
        public DateTime FechaComienzoOrdenTrabajo { get; set; }
        public DateTime FechaComienzoTrabajo
        {
            get
            {
                return FechaComienzoOrdenTrabajo;
            }
            set
            {
                FechaComienzoOrdenTrabajo = value;
                CantidadDiasOrdenTrabajo = (int)((DateTime.Now - FechaComienzoOrdenTrabajo).TotalDays);
            }
        }

        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public Enums.EstadoEmpleado EstadoKeyValue { get; set; }
        public int Dias { get; set; }


        public ResultadoTabla_EmpleadoPanel()
        {

        }
    }
}

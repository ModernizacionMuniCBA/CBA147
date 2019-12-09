using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class ResultadoTabla_Flota : Resultado_Base<Flota>
    {
        public string Nombre { get; set; }
        public int IdArea { get; set; }
        public string AreaNombre { get; set; }
        public List<Resultado_EmpleadoPorArea> Empleados { get; set; }
        public Resultado_Movil Movil { get; set; }

        //Orden Trabajo
        public string NumeroOrdenTrabajo { get; set; }
        public int? IdOrdenTrabajo { get; set; }
        public string NombreEstadoOrdenTrabajo { get; set; }

        //public int CantidadDiasOrdenTrabajo { get; set; }
        //public DateTime FechaComienzoOrdenTrabajo { get; set; }
        //public DateTime FechaComienzoTrabajo
        //{
        //    get
        //    {
        //        return FechaComienzoOrdenTrabajo;
        //    }
        //    set
        //    {
        //        FechaComienzoOrdenTrabajo = value;
        //        CantidadDiasOrdenTrabajo = (int)((DateTime.Now - FechaComienzoOrdenTrabajo).TotalDays);
        //    }
        //}

        //Estado
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public Enums.EstadoFlota EstadoKeyValue { get; set; }

        public ResultadoTabla_Flota()
            : base()
        {

        }

        public ResultadoTabla_Flota(Flota entity)
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
                Empleados = Resultado_EmpleadoPorArea.ToList(entity.GetEmpleados().Select(x=>x.Empleado).ToList());

            if (entity.Movil != null)
                Movil = new Resultado_Movil(entity.Movil);
            EstadoId = entity.GetUltimoEstado().Id;
            EstadoNombre = entity.GetUltimoEstado().Nombre;
            EstadoColor = entity.GetUltimoEstado().Color;
            EstadoKeyValue = entity.GetUltimoEstado().KeyValue;

            var otActiva = entity.GetOrdenTrabajoActiva();
            if (otActiva != null)
            {
                IdOrdenTrabajo = otActiva.OrdenTrabajo.Id;
                NumeroOrdenTrabajo = otActiva.OrdenTrabajo.Numero;
                NombreEstadoOrdenTrabajo = otActiva.OrdenTrabajo.GetUltimoEstado().Estado.Nombre;
            }
        }

        public static List<Resultado_Flota> ToList(List<Flota> list)
        {
            return list.Select(x => new Resultado_Flota(x)).ToList();
        }
    }
}

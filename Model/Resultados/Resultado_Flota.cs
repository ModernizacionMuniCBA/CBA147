using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Flota : Resultado_Base<Flota>
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

        //Historico de estados
        public List<Resultado_Empleado_HistoricoEstados> Estados { get; set; }

        public Resultado_Flota()
            : base()
        {

        }

        public Resultado_Flota(Flota entity)
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
                NumeroOrdenTrabajo = otActiva.OrdenTrabajo.Numero + "/" + otActiva.OrdenTrabajo.Año;
                NombreEstadoOrdenTrabajo = otActiva.OrdenTrabajo.GetUltimoEstado().Estado.Nombre;
            }

            Estados = Resultado_Empleado_HistoricoEstados.ToList(entity.Estados);
        }

        public static List<Resultado_Flota> ToList(List<Flota> list)
        {
            return list.Select(x => new Resultado_Flota(x)).ToList();
        }

        [Serializable]
        public class Resultado_Empleado_HistoricoEstados
        {

            //Estado
            public int? EstadoKeyValue { get; set; }
            public string EstadoNombre { get; set; }
            public string EstadoColor { get; set; }
            public DateTime? EstadoFecha { get; set; }
            public string EstadoObservaciones { get; set; }

            //Usuario
            public int? UsuarioId { get; set; }
            public string UsuarioNombre { get; set; }
            public string UsuarioApellido { get; set; }
            public string UsuarioUsuario { get; set; }

            public Resultado_Empleado_HistoricoEstados(EstadoFlotaHistorial e)
            {
                EstadoKeyValue = (int)e.Estado.KeyValue;
                EstadoNombre = e.Estado.Nombre;
                EstadoColor = e.Estado.Color;
                EstadoFecha = e.FechaAlta;
                EstadoObservaciones = e.Estado.Observaciones;
                UsuarioId = e.Usuario.Id;
                UsuarioNombre = e.Usuario.Nombre;
                UsuarioApellido = e.Usuario.Apellido;
                UsuarioUsuario = e.Usuario.Username;

            }

            public static List<Resultado_Empleado_HistoricoEstados> ToList(IList<EstadoFlotaHistorial> list)
            {
                return list.Select(x => new Resultado_Empleado_HistoricoEstados(x)).OrderByDescending(x=>x.EstadoFecha).ToList();
            }

        }

    }
}

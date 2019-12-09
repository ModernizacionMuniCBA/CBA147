using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EmpleadoPorArea
    {

        public int Id { get; set; }

        //Usuario
        public int? IdUsuarioCerrojoEmpleado { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public string Cargo { get; set; }
        public string IdentificadorFotoPersonal { get; set; }
        public string IdentificadorFotoRegistroCivil { get; set; }
        public bool SexoMasculino { get; set; }

        //Area
        public string NombreArea { get; set; }
        public int IdArea { get; set; }

        //Orden Trabajo
        public string NumeroOrdenTrabajo { get; set; }
        public int? IdOrdenTrabajo { get; set; }

        //Seccion
        public string NombreSeccion { get; set; }
        public int IdSeccion { get; set; }

        //Estado
        public virtual string NombreEstado { get; set; }
        public virtual int IdEstado { get; set; }
        public virtual string ColorEstado { get; set; }

        //Historico de estados
        public List<Resultado_Empleado_HistoricoEstados> Estados { get; set; }

        //Funciones
        public List<Resultado_FuncionPorEmpleado> Funciones { get; set; }
        //Ordenes Trabajo
        public List<Resultado_OrdenTrabajoPorEmpleado> OrdenesTrabajo { get; set; }


        // Modificacion
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public string UsuarioModificacionNombre { get; set; }
        public string UsuarioModificacionApellido { get; set; }
        public string UsuarioModificacionUsername { get; set; }

        public DateTime? FechaBaja { get; set; }

        public Resultado_EmpleadoPorArea() { }

        public Resultado_EmpleadoPorArea(EmpleadoPorArea empleado)
        {
            Id = empleado.Id;
            FechaAlta = (DateTime)empleado.FechaAlta;
            if (empleado.GetFunciones() != null)
                Funciones = Resultado_FuncionPorEmpleado.ToList(empleado.GetFunciones());

            //Usuario
            IdUsuarioCerrojoEmpleado = empleado.UsuarioEmpleado.Id;
            Dni = empleado.UsuarioEmpleado.Dni;
            Apellido = empleado.UsuarioEmpleado.Apellido;
            Nombre = empleado.UsuarioEmpleado.Nombre;
            IdentificadorFotoPersonal = empleado.UsuarioEmpleado.IdentificadorFotoPersonal;
            IdentificadorFotoRegistroCivil = empleado.UsuarioEmpleado.IdentificadorFotoRegistroCivil;
            Cargo = empleado.UsuarioEmpleado.Cargo;
            SexoMasculino = empleado.UsuarioEmpleado.SexoMasculino;

            //Area
            NombreArea = empleado.Area.Nombre;
            IdArea = empleado.Area.Id;

            //Seccion
            if (empleado.Seccion != null) { 
            NombreSeccion = empleado.Seccion.Nombre;
            IdSeccion = empleado.Seccion.Id;
            }

            //Modificacion
            FechaModificacion = empleado.FechaModificacion;
            UsuarioModificacionId = empleado.Usuario.Id;
            UsuarioModificacionNombre = empleado.Usuario.Nombre;
            UsuarioModificacionApellido = empleado.Usuario.Apellido;
            UsuarioModificacionUsername = empleado.Usuario.Username;

            FechaBaja = empleado.FechaBaja;

            var estado = empleado.UltimoEstado();
            if (estado != null)
            {
                NombreEstado = estado.Nombre;
                IdEstado = estado.Id;
                ColorEstado = estado.Color;
            }

            var ot = empleado.GetOrdenTrabajoActiva();
            if (ot != null)
            {
                IdOrdenTrabajo = ot.OrdenTrabajo.Id;
                NumeroOrdenTrabajo = ot.OrdenTrabajo.Numero + "/" + ot.OrdenTrabajo.Año;
            }
        }
        public static List<Resultado_EmpleadoPorArea> ToList(List<EmpleadoPorArea> list)
        {
            return list.Select(x => new Resultado_EmpleadoPorArea(x)).ToList();
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

            public Resultado_Empleado_HistoricoEstados() : base() { }

        }

        [Serializable]
        public class Resultado_FuncionPorEmpleado
        {
            public int FuncionId { get; set; }
            public string FuncionNombre { get; set; }

            public Resultado_FuncionPorEmpleado()
            {

            }
            public Resultado_FuncionPorEmpleado(FuncionPorEmpleado f)
            {
                FuncionId = f.Funcion.Id;
                FuncionNombre = f.Funcion.Nombre;
            }

            public static List<Resultado_FuncionPorEmpleado> ToList(IList<FuncionPorEmpleado> list)
            {
                return list.Select(x => new Resultado_FuncionPorEmpleado(x)).ToList();
            }
        }
        [Serializable]
        public class Resultado_OrdenTrabajoPorEmpleado
        {

            //Estado
            public int? OrdenTrabajoId { get; set; }
            public string OrdenTrabajoNumero { get; set; }
            public string EstadoColor { get; set; }
            public DateTime? EstadoFecha{ get; set; }
            public string EstadoObservaciones { get; set; }

            //Usuario
            public int? UsuarioId { get; set; }
            public string UsuarioNombre { get; set; }
            public string UsuarioApellido { get; set; }
            public string UsuarioUsuario { get; set; }

            public Resultado_OrdenTrabajoPorEmpleado() : base() { }

        }
    }
}

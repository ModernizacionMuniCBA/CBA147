using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenTrabajoDetalle
    {
        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public string Descripcion { get; set; }

        //Area
        public int AreaId { get; set; }
        public string AreaNombre { get; set; }

        //Ambito
        public int AmbitoId { get; set; }
        public string AmbitoNombre { get; set; }

        //Estado
        public int? EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public int? EstadoKeyValue { get; set; }
        public DateTime? EstadoFecha { get; set; }
        public string EstadoColor { get; set; }
        public string EstadoObservaciones { get; set; }
        public int? EstadoUsuarioId { get; set; }
        public string EstadoUsuarioNombre { get; set; }
        public string EstadoUsuarioApellido { get; set; }
        public string EstadoUsuarioUsuario { get; set; }

        //Recursos
        public string RecursoPersonal { get; set; }
        public string RecursoFlota { get; set; }
        public string RecursoMaterial { get; set; }
        public string RecursoZona { get; set; }
        public string RecursoSeccion { get; set; }


        //Usuario Creador
        public int? UsuarioCreadorId { get; set; }
        public string UsuarioCreadorNombre { get; set; }
        public string UsuarioCreadorApellido { get; set; }
        public string UsuarioCreadorUsername { get; set; }
        public bool UsuarioCreadorSexoMasculino { get; set; }
        public string UsuarioCreadorIdentificadorFotoPersonal { get; set; }


        //Usuario Modificacion
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public string UsuarioModificacionNombre { get; set; }
        public string UsuarioModificacionApellido { get; set; }
        public string UsuarioModificacionUsername { get; set; }
        public bool UsuarioModificacionSexoMasculino { get; set; }
        public string UsuarioModificacionIdentificadorFotoPersonal { get; set; }


        public virtual IList<Resultado_OrdenTrabajoDetalle_EstadoHistorial> Estados { get; set; }
        public virtual IList<Resultado_OrdenTrabajoDetalle_Requerimiento> Requerimientos { get; set; }
        public virtual IList<Resultado_OrdenTrabajoDetalle_Nota> Notas { get; set; }
        public virtual IList<Resultado_OrdenTrabajoDetalle_Movil> Moviles { get; set; }
        public virtual IList<Resultado_OrdenTrabajoDetalle_Empleado> Empleados { get; set; }
        public virtual IList<Resultado_OrdenTrabajoDetalle_Barrio> Barrios { get; set; }
        public virtual IList<Resultado_OrdenTrabajoDetalle_Flota> Flotas { get; set; }
        public virtual int SeccionId { get; set; }
        public virtual string SeccionNombre{ get; set; }

        public Resultado_OrdenTrabajoDetalle() : base() { }

    }

    [Serializable]
    public class Resultado_OrdenTrabajoDetalle_EstadoHistorial
    {
        public int EstadoKeyValue { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public DateTime? EstadoFecha { get; set; }
        public string EstadoObservaciones { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioUsuario { get; set; }
    }

    [Serializable]
    public class Resultado_OrdenTrabajoDetalle_Requerimiento
    {
        public virtual int Id { get; set; }
        public virtual string Numero { get; set; }
        public virtual int Año { get; set; }
        public virtual int CpcId { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string DomicilioLatitud { get; set; }
        public virtual string DomicilioDireccion { get; set; }
        public virtual string DomicilioObservaciones { get; set; }
        public virtual string DomicilioLongitud { get; set; }
        public virtual string CpcNombre { get; set; }
        public virtual int CpcNumero { get; set; }
        public virtual int BarrioId { get; set; }
        public virtual string BarrioNombre { get; set; }
        public virtual int? MotivoId { get; set; }
        public virtual string MotivoNombre { get; set; }

        //Estado
        public int? EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public int? EstadoKeyValue { get; set; }
        public DateTime? EstadoFecha { get; set; }
        public string EstadoColor { get; set; }
        public string EstadoObservaciones { get; set; }
    }

    [Serializable]
    public class Resultado_OrdenTrabajoDetalle_Nota
    {
        public virtual string Observaciones { get; set; }
        public virtual DateTime? Fecha { get; set; }
        public virtual int UsuarioId { get; set; }
        public virtual int? RequerimientoId { get; set; }
        public virtual string RequerimientoNumero { get; set; }
        public virtual int RequerimientoAño { get; set; }
        public virtual string UsuarioNombre { get; set; }
        public virtual string UsuarioApellido { get; set; }
        public virtual string UsuarioUsername { get; set; }
        public virtual string UsuarioIdentificadorFotoPersonal { get; set; }
        public virtual bool UsuarioSexoMasculino { get; set; }
    }

    [Serializable]
    public class Resultado_OrdenTrabajoDetalle_Movil
    {
        public virtual int Id { get; set; }
        public virtual string TipoNombre { get; set; }
        public virtual string NumeroInterno { get; set; }
        public virtual string Marca { get; set; }
        public virtual string Modelo { get; set; }
        public virtual int? MovilId { get; set; }
    }

    [Serializable]
    public class Resultado_OrdenTrabajoDetalle_Empleado
    {
        public virtual int Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Apellido { get; set; }
        public virtual int? Dni { get; set; }
        public virtual int EmpleadoId { get; set; }
        public virtual bool UsuarioSexoMasculino { get; set; }
        public virtual string UsuarioIdentificadorFotoPersonal { get; set; }
        public virtual string NombreSeccion { get; set; }
        public virtual int? IdSeccion { get; set; }

    }


    [Serializable]
    public class Resultado_OrdenTrabajoDetalle_Flota
    {
        public virtual int Id { get; set; }
        public virtual int FlotaId { get; set; }
        public virtual string FlotaNombre { get; set; }
        public virtual string MovilTipoNombre { get; set; }
        public virtual string MovilNumeroInterno { get; set; }
        public virtual int MovilId { get; set; }
    
        public virtual IList<Resultado_OrdenTrabajoDetalle_Empleado> Empleados { get; set; }
    }

    [Serializable]
    public class Resultado_OrdenTrabajoDetalle_Barrio
    {
        public virtual int Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual int ZonaId { get; set; }
        public virtual string ZonaNombre { get; set; }
        public virtual string ZonaColor { get; set; }
    }

    [Serializable]
    public class Resultado_OrdenTrabajoDetalle_Zona
    {
        public virtual int Id { get; set; }
        public virtual string Nombre { get; set; }
        
    }
}

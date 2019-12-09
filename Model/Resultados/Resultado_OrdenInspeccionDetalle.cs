using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenInspeccionDetalle
    {
        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public string Descripcion { get; set; }

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

        //Usuario Creador
        public int? UsuarioCreadorId { get; set; }
        public string UsuarioCreadorNombre { get; set; }
        public string UsuarioCreadorApellido { get; set; }
        public string UsuarioCreadorUsuario { get; set; }

        //Usuario Modificacion
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public string UsuarioModificacionNombre { get; set; }
        public string UsuarioModificacionApellido { get; set; }
        public string UsuarioModificacionUsuario { get; set; }

        public virtual IList<Resultado_OrdenInspeccionDetalle_EstadoHistorial> Estados { get; set; }
        public virtual IList<Resultado_OrdenInspeccionDetalle_Requerimiento> Requerimientos { get; set; }
        public virtual IList<Resultado_OrdenInspeccionDetalle_Nota> Notas { get; set; }
        public virtual IList<Resultado_OrdenInspeccionDetalle_Barrio> Barrios { get; set; }

        public Resultado_OrdenInspeccionDetalle() : base() { }

    }

    [Serializable]
    public class Resultado_OrdenInspeccionDetalle_EstadoHistorial
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
    public class Resultado_OrdenInspeccionDetalle_Requerimiento
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
    public class Resultado_OrdenInspeccionDetalle_Nota
    {
        public virtual string Observaciones { get; set; }
        public virtual DateTime? Fecha { get; set; }
        public virtual int UsuarioId { get; set; }
        public virtual int? RequerimientoId { get; set; }
        public virtual string RequerimientoNumero { get; set; }
        public virtual int RequerimientoAño { get; set; }
        public virtual string UsuarioNombre { get; set; }
        public virtual string UsuarioApellido { get; set; }
        public virtual string UsuarioUsuario { get; set; }

    }

    [Serializable]
    public class Resultado_OrdenInspeccionDetalle_Barrio
    {
        public virtual int Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual int ZonaId { get; set; }
        public virtual string ZonaNombre { get; set; }
        public virtual string ZonaColor { get; set; }
    }
}

using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_RequerimientoDetalle2
    {
        public int? Id { get; set; }
        public string Numero { get; set; }
        public int? Año { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string Descripcion { get; set; }


        //Motivo Servicio Area
        public int? MotivoId { get; set; }
        public string MotivoNombre { get; set; }
        public int? MotivoTipo { get; set; }
        public int? ServicioId { get; set; }
        public string ServicioNombre { get; set; }
        public int? AreaId { get; set; }
        public string AreaNombre { get; set; }


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


        //Indicadores
        public Enums.PrioridadRequerimiento Prioridad { get; set; }
        public bool Peligroso { get; set; }
        public bool Marcado { get; set; }
        public bool Favorito { get; set; }
        public bool Inspeccionado { get; set; }

        //OT
        public int? OrdenTrabajoId { get; set; }
        public string OrdenTrabajoNumero { get; set; }
        public int? OrdenTrabajoAño { get; set; }

        //Domicilio
        public int? DomicilioId { get; set; }
        public string DomicilioDireccion { get; set; }
        public string DomicilioLatitud { get; set; }
        public string DomicilioLongitud { get; set; }
        public string DomicilioBarrioNombre { get; set; }
        public string DomicilioCpcNombre { get; set; }
        public int? DomicilioCpcNumero { get; set; }
        public string DomicilioObservaciones { get; set; }
        public int? DomicilioDistancia { get; set; }
        public bool? DomicilioSugerido { get; set; }


        //Origen
        public int? OrigenId { get; set; }
        public string OrigenNombre { get; set; }


        //Informacion Organica
        public int? InformacionOrganicaSecretariaId { get; set; }
        public string InformacionOrganicaSecretariaNombre { get; set; }
        public int? InformacionOrganicaDireccionId { get; set; }
        public string InformacionOrganicaDireccionNombre { get; set; }
        public string InformacionOrganicaDireccionTelefono { get; set; }
        public string InformacionOrganicaDireccionResponsable { get; set; }
        public string InformacionOrganicaDireccionEmail { get; set; }
        public string InformacionOrganicaDireccionDomicilio { get; set; }

        //Usuarios Referentes
        public IList<Resultado_RequerimientoDetalle2_UsuarioReferente> UsuariosReferentes {get;set;}

        //ReferenteProvisorio
        public string ReferenteProvisorioNombre { get; set; }
        public string ReferenteProvisorioApellido { get; set; }
        public string ReferenteProvisorioTelefono { get; set; }
        public int ReferenteProvisorioDni { get; set; }
        public bool ReferenteProvisorioGeneroMasculino { get; set; }
        public string ReferenteProvisorioObservaciones { get; set; }

        //Usuario Creador
        public int? UsuarioCreadorId { get; set; }
        public string UsuarioCreadorNombre { get; set; }
        public string UsuarioCreadorApellido { get; set; }
        public string UsuarioCreadorUsername { get; set; }
        public string UsuarioCreadorIdentificadorFotoPersonal { get; set; }
        public bool UsuarioCreadorSexoMasculino { get; set; }


        //Usuario Modificacion
        public int? UsuarioModificacionId { get; set; }
        public string UsuarioModificacionNombre { get; set; }
        public string UsuarioModificacionApellido { get; set; }
        public string UsuarioModificacionUsername { get; set; }
        public string UsuarioModificacionIdentificadorFotoPersonal { get; set; }
        public bool UsuarioModificacionSexoMasculino { get; set; }


        //Adjuntos
        public int? CantidadFotos { get; set; }
        public int? CantidadDocumentos { get; set; }

        public Resultado_RequerimientoDetalle2() : base() { }


        public List<Resultado_RequerimientoDetalle2_Comentario> Comentarios { get; set; }
        public List<Resultado_RequerimientoDetalle2_HistoricoEstados> Estados { get; set; }
        public List<Resultado_RequerimientoDetalle2_HistoricoOrdenesTrabajo> OrdenesDeTrabajo { get; set; }
        //public List<Resultado_RequerimientoDetalle2_HistoricoOrdenesInspeccion> OrdenesDeInspeccion { get; set; }
        public List<Resultado_RequerimientoDetalle2_Tarea> Tareas { get; set; }
        public List<Resultado_RequerimientoDetalle2_CampoDinamico> CamposDinamicos { get; set; }

    }

    [Serializable]
    public class Resultado_RequerimientoDetalle2_UsuarioReferente{
        
        //Usuario Referente
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string IdentificadorFotoPersonal { get; set; }
        public bool SexoMasculino { get; set; }
        public string Email { get; set; }
        public int Dni { get; set; }
        public string Observaciones { get; set; }

        public Resultado_RequerimientoDetalle2_UsuarioReferente() : base() { }
    }

    [Serializable]
    public class Resultado_RequerimientoDetalle2_Comentario
    {

        //Comentario
        public string Observaciones { get; set; }
        public DateTime? Fecha { get; set; }
        public int? OrdenTrabajoId { get; set; }
        public string OrdenTrabajoNumero { get; set; }
        public int? OrdenTrabajoAño { get; set; }

        //Usuario
        public int? UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string UsuarioUsername { get; set; }
        public string UsuarioIdentificadorFotoPersonal{ get; set; }
        public bool UsuarioSexoMasculino { get; set; }

        public Resultado_RequerimientoDetalle2_Comentario() : base() { }

    }

    [Serializable]
    public class Resultado_RequerimientoDetalle2_HistoricoEstados
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

        public Resultado_RequerimientoDetalle2_HistoricoEstados() : base() { }

    }

    [Serializable]
    public class Resultado_RequerimientoDetalle2_HistoricoOrdenesTrabajo
    {

        public int? Id { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaCierre { get; set; }

        public string AreaNombre { get; set; }
        public int AreaId { get; set; }

        //Estado
        public int? EstadoKeyValue { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }

        public Resultado_RequerimientoDetalle2_HistoricoOrdenesTrabajo() : base() { }

    }

    //[Serializable]
    //public class Resultado_RequerimientoDetalle2_HistoricoOrdenesInspeccion
    //{

    //    public int? Id { get; set; }
    //    public string Numero { get; set; }
    //    public int Año { get; set; }
    //    public DateTime? FechaAlta { get; set; }
    //    public DateTime? FechaCierre { get; set; }


    //    //Estado
    //    public int? EstadoKeyValue { get; set; }
    //    public string EstadoNombre { get; set; }
    //    public string EstadoColor { get; set; }

    //    public Resultado_RequerimientoDetalle2_HistoricoOrdenesInspeccion() : base() { }

    //}

    [Serializable]
    public class Resultado_RequerimientoDetalle2_Tarea
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }


        public Resultado_RequerimientoDetalle2_Tarea() : base() { }

    }

    [Serializable]
    public class Resultado_RequerimientoDetalle2_CampoDinamico
    {

        public int Id { get; set; }
        public int IdCampoPorMotivo { get; set; }
        public int IdTipoCampoPorMotivo { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public int Orden { get; set; }
        public string Grupo { get; set; }

        public Resultado_RequerimientoDetalle2_CampoDinamico() : base() { }

    }
}

using System;
using System.Linq;

namespace Model
{
    public class Enums
    {
        // SISI
        public enum EstudiosAlcanzados
        {
            PrimarioIncompleto = 1,
            PrimarioCompleto,
            SecundarioIncompleto,
            SecundarioCompleto,
            UniversitarioIncompleto,
            UniversitarioCompleto,
            PosgradoIncompleto,
            PosgradoCompleto
        }

        //public enum Ocupacion
        //{
        //    Estudiante=1,
        //    RelacionDeDependencia,
        //    Desocupado
        //}


        public enum Mail
        {
            Preinscripcion = 1,
            Entrevista = 2
        }

        // CBA147
        public enum Sexo
        {
            MASCULINO = 1,
            FEMENINO = 2
        }

        public enum TipoDocumento
        {
            DNI = 1,
            LC = 2,
            LE = 3,
            PASAPORTE = 4
        }

        public enum TipoDispositivo
        {
            PHONE = 1,
            TABLET = 2,
            DESCKTOP = 3
        }

        public enum TipoMotivo
        {
            GENERAL = 1,
            INTERNO = 2,
            PRIVADO = 3
        }

        public enum EsfuerzoMotivo
        {
            LEVE = 1,
            MODERADO = 2,
            ALTO = 3,
            FUERTE = 4,
            MAXIMO=5
        }

        public enum TipoCampoPorMotivo
        {
            TEXTOLARGO = 1,
            TEXTOCORTO= 2,
            NUMERO = 3,
            FECHA = 4,
            SINO=5,
            LEYENDA=6,
            SELECTOR=7
        }

        public enum PrioridadRequerimiento
        {
            NORMAL = 1,
            MEDIA = 2,
            ALTA = 3
        }

        public enum EstadoRequerimiento
        {
            NUEVO = 1,
            INSPECCION = 9,
            PENDIENTE = 2,
            ENPROCESO = 4,
            CANCELADO = 5,
            //SUSPENDIDO = 6,
            COMPLETADO = 7,
            CERRADO = 8,
            SINDATOS = -1
        }

        public enum EstadoOrdenTrabajo
        {
            NUEVO=1,
            ENPROCESO = 2,
            COMPLETADO = 3,
            CANCELADO = 4
        }

        public enum EstadoOrdenEspecial
        {
            ENPROCESO = 1,
            COMPLETADO = 2
        }

        public enum EstadoOrdenInspeccion
        {
            ENPROCESO = 1,
            COMPLETADO = 2,
            CANCELADO = 3
        }
        public enum EstadoFlota
        {
            DISPONIBLE = 1,
            OCUPADO=3,
            TURNOTERMINADO= 4
        }

        public enum EstadoEmpleado
        {
            DISPONIBLE = 1,
            ENLICENACIA,
            OCUPADO,
            INACTIVO,
            ENFLOTA = 5
        }
        public enum EstadoMovil
        {
            DISPONIBLE = 1,
            ENTALLER,
            ENDEPOSITO,
            INACTIVO,
            ENFLOTA,
            OCUPADO=6
        }
        public enum CondicionMovil
        {
            MALA = 1,
            REGULAR = 2,
            BUENA = 3,
            EXCELENTE = 4
        }

        public enum TipoCombustible
        {
            NAFTA = 1,
            GAS,
            DIÉSEL,
            ELECTRICIDAD,
            HÍBRIDO
        }

        public enum TipoRequerimiento
        {
            RECLAMO = 1,
            AGRADECIMIENTO = 2,
            CONSULTA = 3,
            SUGERENCIA = 4
        }

        public enum TipoReporte
        {
            LISTADOREQUERIMIENTOS = 1
        }

        public enum TipoArchivo
        {
            DOCUMENTO = 1,
            IMAGEN = 2
        }

        public enum PermisoEstadoRequerimiento
        {
            EditarFavorito = 1,
            EditarUbicacion = 2,
            EditarEstado = 3,
            Cancelar = 4,
            EditarPrioridad = 5,
            EditarMarcado = 6,
            EditarReferente = 7,
            EditarComentarios = 8,
            EditarMotivo = 9,
            AgregarEnOrdenDeTrabajo = 10,
            AgregarEnOrdenDeAtencionCritica = 11,
            EditarDocumentos = 12,
            EditarRelevamientoDeOficio = 13,
            EditarObservaciones = 14,
            BandejaDeUrgentes_PosibleFiltro = 15,
            BandejaDeUrgentes_CheckeadoPorDefault = 20,
            VerEnBandejaDeEntrada = 16,
            VerEnCambioEstado = 18,
            VerEnCerrarOrdenDeTrabajo = 19,
            VerEnRequerimientoInternoNuevo = 21,
            AgregarEnOrdenDeInspeccion = 22,
            VerEnCerrarOrdenDeInspeccion = 23,
            AgregarTareas = 24, EditarCamposDinamicos = 25

        }
        public enum PermisoEstadoOrdenTrabajo
        {
            EditarDescripcion = 1,
            AgregarRequerimiento = 2,
            QuitarRequerimiento = 3,
            EditarRecursos = 4,
            AgregarNota = 5,
            EditarMoviles = 6,
            Cerrar = 7,
            Cancelar = 8,
            CambiarSeccion = 9,
            EditarEmpleados = 10,
            EditarFlotas=11
        }

        public enum PermisoEstadoOrdenInspeccion
        {
            EditarDescripcion = 1,
            AgregarRequerimiento = 2,
            QuitarRequerimiento = 3,
            AgregarNota = 4,
            Cerrar = 5,
            Cancelar = 6
        }
    }
}

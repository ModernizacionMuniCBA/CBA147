using Model;
using Model.Entities;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoExterno_Requerimiento
    {

        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public Enums.EstadoRequerimiento EstadoKeyValue { get; set; }

        public int AreaId { get; set; }

        public string Descripcion { get; set; }
        public string BarrioNombre { get; set; }
        public string CpcNombre { get; set; }
        public int CpcNumero { get; set; }
        public string DomicilioObservaciones { get; set; }
        public string DomicilioDireccion { get; set; }
        public string DomicilioLatitud { get; set; }
        public string DomicilioLongitud { get; set; }
        public int? UsuarioCreadorId { get; set; }
        public string UsuarioCreadorNombre { get; set; }
        public string UsuarioCreadorApellido { get; set; }
        public string UsuarioCreadorTelefonoFijo { get; set; }
        public string UsuarioCreadorTelefonoCelular { get; set; }
        public string UsuarioCreadorEmail { get; set; }
        public string UsuarioCreadorGenero{ get; set; }

        public ResultadoExterno_Requerimiento() : base() { }

        public ResultadoExterno_Requerimiento(ResultadoWSExterno_Requerimiento requerimiento)
        {
            Id = requerimiento.Id;
            FechaAlta = requerimiento.FechaAlta;
            Numero = requerimiento.Numero;
            Año = requerimiento.Año;
            MotivoId = requerimiento.MotivoId;
            MotivoNombre = requerimiento.MotivoNombre;

            EstadoId = requerimiento.EstadoId;
            EstadoNombre = requerimiento.EstadoNombre;
            EstadoKeyValue = requerimiento.EstadoKeyValue;

            Descripcion = requerimiento.Descripcion;
            BarrioNombre = requerimiento.BarrioNombre;
            CpcNombre = requerimiento.CpcNombre;
            CpcNumero = requerimiento.CpcNumero;
            DomicilioObservaciones = requerimiento.DomicilioObservaciones;
            DomicilioDireccion = requerimiento.DomicilioDireccion;
            DomicilioLatitud = requerimiento.DomicilioLatitud;
            DomicilioLongitud = requerimiento.DomicilioLongitud;
            UsuarioCreadorId = requerimiento.UsuarioCreadorId;
            UsuarioCreadorNombre = requerimiento.UsuarioCreadorNombre;
            UsuarioCreadorApellido = requerimiento.UsuarioCreadorApellido;
            UsuarioCreadorTelefonoFijo = requerimiento.UsuarioCreadorTelefonoFijo;
            UsuarioCreadorTelefonoCelular = requerimiento.UsuarioCreadorTelefonoCelular;
            UsuarioCreadorEmail = requerimiento.UsuarioCreadorEmail;
            UsuarioCreadorGenero = requerimiento.UsuarioCreadorGenero;

            AreaId = requerimiento.AreaId;

        }

        public static List<ResultadoExterno_Requerimiento> ToList(List<ResultadoWSExterno_Requerimiento> items)
        {
            return items.Select(x => new ResultadoExterno_Requerimiento(x)).ToList();

        }

    }
}

using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Movil : Resultado_Base<Movil>
    {
        public virtual int IdArea { get; set; }
        public virtual string NombreArea { get; set; }

        //Datos Identificatorios
        public virtual int IdTipo { get; set; }
        public virtual int? Año { get; set; }
        public virtual string NombreTipo { get; set; }
        public virtual string Modelo { get; set; }
        public virtual string Marca { get; set; }
        public virtual string Dominio { get; set; }
        public virtual DateTime? FechaIncorporacion { get; set; }
        public virtual string FechaIncorporacionString { get; set; }
        public virtual string NumeroInterno { get; set; }

        //Caracteristicas
        public virtual string Carga { get; set; }
        public virtual int? Asientos { get; set; }

        //Tipo combustible
        public virtual string NombreTipoCombustible { get; set; }
        public virtual int IdTipoCombustible { get; set; }

        //Valuacion
        public virtual float Valuacion { get; set; }
        public virtual DateTime? FechaValuacion { get; set; }
        public virtual string FechaValuacionString { get; set; }
        public virtual string ObservacionesValuacion { get; set; }

        //Kilometraje
        public virtual int Kilometraje { get; set; }
        public virtual DateTime? FechaKilometraje { get; set; }
        public virtual string FechaKilometrajeString { get; set; }
        public virtual string ObservacionesKilometraje { get; set; }

        //ITV
        public virtual DateTime? FechaVencimientoITV { get; set; }
        public virtual DateTime? FechaUltimoITV { get; set; }
        public virtual string ObservacionesITV { get; set; }

        //TUV
        public virtual DateTime? FechaVencimientoTUV { get; set; }
        public virtual DateTime? FechaUltimoTUV { get; set; }
        public virtual string ObservacionesTUV { get; set; }

        //Estado
        public virtual string NombreEstado { get; set; }
        public virtual int IdEstado { get; set; }
        public virtual string ColorEstado { get; set; }

        //Historico de estados
        public List<Resultado_Movil_HistoricoEstados> Estados { get; set; }
        //Historico de Reparaciones
        public List<Resultado_Movil_Reparacion> Reparaciones { get; set; }

        //Notas
        public List<Resultado_Movil_Nota> Notas { get; set; }

        //Condicion
        public virtual string NombreCondicion { get; set; }
        public virtual int IdCondicion { get; set; }


        public virtual string Caracteristicas { get; set; }

        //Usuario Modificacion
        public int? UsuarioModificacionId { get; set; }
        public string UsuarioModificacionNombre { get; set; }
        public string UsuarioModificacionApellido { get; set; }
        public string UsuarioModificacionUsername { get; set; }


        public Resultado_Movil()
            : base()
        {

        }

        public Resultado_Movil(Movil entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            if (entity.Condicion.HasValue)
            {
                NombreCondicion = entity.Condicion.ToString();
                IdCondicion = (int)entity.Condicion.Value;
            }

            NombreTipo = entity.Tipo.Nombre;
            IdTipo = entity.Tipo.Id;
            Año = entity.Año;
            IdArea = entity.Area.Id;
            NombreArea = entity.Area.Nombre;
            Modelo = entity.Modelo;
            Marca = entity.Marca;
            Dominio = entity.Dominio;
            FechaIncorporacion = entity.FechaIncorporacion;
            var fechaIncorporacionString = Utils.DateToString(FechaIncorporacion);
            if (fechaIncorporacionString == null)
            {
                fechaIncorporacionString = "";
            }
            FechaIncorporacionString = fechaIncorporacionString;
            NumeroInterno = entity.NumeroInterno;

            //Caracteristicas
            if (entity.TipoCombustible.HasValue)
            {
                IdTipoCombustible = (int)entity.TipoCombustible;
                NombreTipoCombustible = entity.TipoCombustible.ToString();
            }
            Carga = entity.Carga;
            Asientos = entity.Asientos;


            Caracteristicas = entity.Caracteristicas;

            var val = entity.UltimaValuacion();
            if (val != null)
            {
                Valuacion = val.Valor;
                FechaValuacion = val.FechaValuacion;
                var fechaValuacionString = Utils.DateToString(FechaValuacion);
                if (fechaValuacionString == null)
                {
                    fechaValuacionString = "";
                }
                FechaValuacionString = fechaValuacionString;
                ObservacionesValuacion = val.Observaciones;
            }

            var km = entity.UltimoKilometraje();
            if (km != null)
            {
                Kilometraje = km.Kilometraje;
                FechaKilometraje = km.FechaKilometraje;
                var fechaKilometrajeString = Utils.DateToString(FechaKilometraje);
                if (fechaKilometrajeString == null)
                {
                    fechaKilometrajeString = "";
                }
                FechaKilometrajeString = fechaKilometrajeString;
                ObservacionesKilometraje = km.Observaciones;
            }

            var itv = entity.UltimoITV();
            if (itv != null)
            {
                FechaUltimoITV = itv.FechaUltimoITV;
                FechaVencimientoITV = itv.FechaVencimientoITV;
                ObservacionesITV = itv.Observaciones;
            }

            var tuv = entity.UltimoTUV();
            if (tuv != null)
            {
                FechaUltimoTUV = tuv.FechaUltimoTUV;
                FechaVencimientoTUV = tuv.FechaVencimientoTUV;
                ObservacionesTUV = tuv.Observaciones;
            }

            var estado = entity.UltimoEstado();
            if (estado != null)
            {
                NombreEstado = estado.Nombre;
                IdEstado = estado.Id;
                ColorEstado = estado.Color;
            }

            //Usuario Modificacion
            UsuarioModificacionApellido = entity.Usuario.Apellido;
            UsuarioModificacionNombre = entity.Usuario.Nombre;
            UsuarioModificacionUsername = entity.Usuario.Username;
            UsuarioModificacionId= entity.Usuario.Id;
        }

        public static List<Resultado_Movil> ToList(List<Movil> list)
        {
            return list.Select(x => new Resultado_Movil(x)).ToList();
        }

        [Serializable]
        public class Resultado_Movil_HistoricoEstados
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

            public Resultado_Movil_HistoricoEstados() : base() { }

        }

        [Serializable]
        public class Resultado_Movil_Nota
        {

            //Estado
            public int Id { get; set; }
            public string Contenido { get; set; }
            public bool Visto { get; set; }
            public DateTime FechaAlta { get; set; }

            //Usuario creador
            public int? UsuarioCreadorId { get; set; }
            public string UsuarioCreadorNombre { get; set; }
            public string UsuarioCreadorApellido { get; set; }
            public string UsuarioCreadorUsername { get; set; }

            //Usuario visto
            public int? UsuarioVistoId { get; set; }
            public string UsuarioVistoNombre { get; set; }
            public string UsuarioVistoApellido { get; set; }
            public string UsuarioVistoUsername { get; set; }

            public Resultado_Movil_Nota() : base() { }

        }
        [Serializable]
        public class Resultado_Movil_Reparacion
        {

            //Estado
            public int Id { get; set; }
            public DateTime FechaAlta { get; set; }
            public string Motivo { get; set; }
            public string Taller { get; set; }
            public int MontoReparacion { get; set; }
            public DateTime FechaReparacion { get; set; }
            public string Observaciones { get; set; }

            public Resultado_Movil_Reparacion() : base() { }

        }

    }
}

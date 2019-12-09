using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    [Serializable]
    public class Consulta_Requerimiento
    {
        public string Numero { get; set; }
        public int? Año { get; set; }
        public int IdAmbito { get; set; }
        public List<Enums.PrioridadRequerimiento> Prioridades { get; set; }
        public List<Enums.TipoMotivo> Tipos { get; set; }
        public List<Enums.EstadoRequerimiento> EstadosKeyValue { get; set; }
        public List<int> IdsArea { get; set; }
        public List<int> IdsServicio { get; set; }
        public List<int> IdsMotivo { get; set; }
        public List<int> IdsPersonaFisica { get; set; }
        public List<int> IdsUsuarioReferente { get; set; }
        public List<int> IdsUsuarioCreador { get; set; }
        public List<int> IdsBarrio { get; set; }
        public List<int> IdsBarrioCatastro { get; set; }
        public List<int> IdsOrigen { get; set; }
        public List<int> KeyValuesCPC { get; set; }
        public int IdATI { get; set; }
        public string Domicilio { get; set; }
        public int? Altura { get; set; }
        public List<int> IdsZona { get; set; }
        public List<int> IdsCategoria { get; set; }
        public int? Limite { get; set; }
        public bool? RelevamientoDeOficio { get; set; }
        public bool? OrdenAtencionCritica { get; set; }
        public bool? Inspeccionado { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? Mes { get; set; }
        public int? AñoDeMes { get; set; }
        public bool? DadosDeBaja { get; set; }
        public bool? Urgente { get; set; }


        public bool JoinDomicilio
        {
            get
            {
                return
                    (IdsBarrio != null && IdsBarrio.Count != 0) ||
                    (IdsZona != null && IdsZona.Count != 0) ||
                    (KeyValuesCPC != null && KeyValuesCPC.Count != 0) ||
                    (Altura.HasValue && Altura.Value != -1) ||
                    !string.IsNullOrEmpty(Domicilio);
            }
        }


        public Consulta_Requerimiento()
        {
            DadosDeBaja = false;
        }

        public void EstadoKeyValue(Enums.EstadoRequerimiento estado)
        {
            EstadosKeyValue = new List<Enums.EstadoRequerimiento>();
            EstadosKeyValue.Add(estado);
        }

        public Consulta_Requerimiento(Consulta_Requerimiento_Bandeja consulta)
        {
            Numero = consulta.Numero;
            Año = consulta.Año;
            Prioridades = consulta.Prioridades;
            EstadosKeyValue = consulta.EstadosKeyValue;
            IdsArea =  new List<int>(){consulta.IdArea};
            IdsServicio = consulta.IdsServicio;
            IdsMotivo = consulta.IdsMotivo;
            IdsPersonaFisica = consulta.IdsPersonaFisica;
            IdsUsuarioReferente = consulta.IdsUsuarioReferente;
            IdsUsuarioCreador = consulta.IdsUsuarioCreador;
            IdsBarrio = consulta.IdsBarrio;
            IdsBarrioCatastro = consulta.IdsBarrioCatastro;
            Domicilio = consulta.Domicilio;
            IdsOrigen = consulta.IdsOrigen;
            Tipos = consulta.Tipos;
            if (consulta.KeyValueCPC.HasValue && consulta.KeyValueCPC.Value != -1)
            {
                KeyValuesCPC = new List<int>() { consulta.KeyValueCPC.Value };
            }
            else
            {
                KeyValuesCPC = null;
            }
            Altura = consulta.Altura;
            IdsZona = consulta.IdsZona;
            IdsCategoria = consulta.IdsCategoria;
            IdATI = consulta.IdATI;
            RelevamientoDeOficio = consulta.RelevamientoDeOficio;
            Inspeccionado = consulta.Inspeccionado;
            FechaDesde = consulta.FechaDesde;
            FechaHasta = consulta.FechaHasta;
            DadosDeBaja = consulta.DadosDeBaja;
            Urgente = consulta.Urgente;
            Limite = consulta.Limite;
        }
    }
}

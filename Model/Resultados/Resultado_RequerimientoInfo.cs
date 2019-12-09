using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_RequerimientoInfo
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public string Servicio { get; set; }
        public string Area { get; set; }
        public string Motivo { get; set; }
        public string Cpc { get; set; }
        public string Barrio{ get; set; }
        public string Longitud { get; set; }
        public string Latitud { get; set; }
        public int CpcNumero { get; set; }
        public string EstadoNuevo { get; set; }
        public string EstadoProceso { get; set; }
        public string EstadoCompletado { get; set; }
        public string EstadoCancelado { get; set; }
        public string UltimoEstado { get; set; }
        public int OrigenId { get; set; }
        public string OrigenNombre { get; set; }
        public string Genero { get; set; }
        public string FechaNacimiento { get; set; }
        public string Sexo { get; set; }
    }
}

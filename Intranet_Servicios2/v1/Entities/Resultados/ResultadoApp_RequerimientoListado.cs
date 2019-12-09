using Model.Entities;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    public class ResultadoApp_RequerimientoListado
    {
        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }


        //Servicio
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }
        public string ServicioColor { get; set; }
        public string ServicioIcono { get; set; }

        //Motivo
        public int MotivoId { get; set; }
        public string MotivoNombre { get; set; }

        //Estado
        public int EstadoId { get; set; }
        public int EstadoKeyValue { get; set; }
        public int EstadoKeyValuePublico { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public DateTime EstadoFecha { get; set; }

        //Estado Publico
        public int EstadoPublicoId { get; set; }
        public int EstadoPublicoKeyValue { get; set; }
        public string EstadoPublicoNombre { get; set; }
        public string EstadoPublicoColor { get; set; }


        //Barrio
        public int BarrioId { get; set; }
        public string BarrioNombre { get; set; }


        //CPC

        public int CpcId { get; set; }
        public string CpcNombre { get; set; }
        public int CpcNumero { get; set; }


        public ResultadoApp_RequerimientoListado()
        {

        }


    }
}

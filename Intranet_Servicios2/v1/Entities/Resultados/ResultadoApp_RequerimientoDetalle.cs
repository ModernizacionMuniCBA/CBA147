using Model.Entities;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    public class ResultadoApp_RequerimientoDetalle
    {
        public virtual int Id { get; set; }
        public string Numero { get; set; }
        public int Año { get; set; }
        public string Descripcion { get; set; }

        //Servicio
        public string ServicioNombre { get; set; }
        public string ServicioIcono { get; set; }
        public string ServicioColor { get; set; }

        //Motivo
        public string MotivoNombre { get; set; }

        //Estado
        public int EstadoKeyValue { get; set; }
        public int EstadoKeyValuePublico { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public DateTime EstadoFecha { get; set; }
        public string EstadoObservaciones { get; set; }

        //Estado Publico
        public int EstadoPublicoKeyValue { get; set; }
        public string EstadoPublicoNombre { get; set; }
        public string EstadoPublicoColor { get; set; }

        //Ubicacion
        public string DomicilioDireccion { get; set; }
        public string DomicilioObservaciones { get; set; }
        public string DomicilioLatitud { get; set; }
        public string DomicilioLongitud { get; set; }
        public int DomicilioCpcNumero { get; set; }
        public string DomicilioCpcNombre { get; set; }
        public string DomicilioBarrioNombre { get; set; }

        //Informacion organica
        public string InformacionOrganicaAreaNombre { get; set; }
        public string InformacionOrganicaDireccionNombre { get; set; }
        public string InformacionOrganicaSecretariaNombre { get; set; }
        public string InformacionOrganicaEmail { get; set; }
        public string InformacionOrganicaDomicilio { get; set; }

        //Fotos
        public List<string> Fotos { get; set; }

        public ResultadoApp_RequerimientoDetalle()
        {

        }


    }
}

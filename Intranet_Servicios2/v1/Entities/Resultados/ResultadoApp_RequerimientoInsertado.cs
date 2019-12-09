using Model.Entities;
using Model.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    public class ResultadoApp_RequerimientoInsertado
    {
        public virtual int Id { get; set; }

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


        public ResultadoApp_RequerimientoInsertado()
        {

        }


        public ResultadoApp_RequerimientoInsertado(Requerimiento entity){
            Id = entity.Id;
            Numero = entity.Numero;
            Año = entity.Año;
            ServicioId = entity.Motivo.Tema.Servicio.Id;
            ServicioNombre = entity.Motivo.Tema.Servicio.Nombre;
            ServicioColor = entity.Motivo.Tema.Servicio.Color;
            ServicioIcono = entity.Motivo.Tema.Servicio.Icono;
            MotivoId = entity.Motivo.Id;
            MotivoNombre = entity.Motivo.Nombre;

        }
    }
}

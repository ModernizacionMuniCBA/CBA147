using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoApp_Domicilio
    {
        public virtual int Id{get;set;}
        public virtual ResultadoApp_Barrio Barrio { get; set; }
        public virtual ResultadoApp_Cpc Cpc { get; set; }
        public virtual string Latitud { get; set; }
        public virtual string Longitud { get; set; }
        public string Direccion { get; set; }
        public virtual int Distancia { get; set; }
        public bool Sugerido { get; set; }
        public string Observaciones { get; set; }

        public ResultadoApp_Domicilio()
            : base()
        {

        }

        public ResultadoApp_Domicilio(Domicilio entity)
        {
            if (entity == null) return;

            Id = entity.Id;
            Latitud = entity.Latitud;
            Longitud = entity.Longitud;
            Direccion = entity.Direccion;
            Distancia = entity.Distancia;
            Sugerido = entity.Sugerido;
            Observaciones = entity.Observaciones;

            if (entity.Barrio != null)
            {
                Barrio = new ResultadoApp_Barrio(entity.Barrio);
            }

            if (entity.Cpc != null)
            {
                Cpc = new ResultadoApp_Cpc(entity.Cpc);
            }
        }

        public static List<ResultadoApp_Domicilio> ToList(List<Domicilio> entities)
        {
            return entities.Select(x => new ResultadoApp_Domicilio(x)).ToList();
        }

    }
}

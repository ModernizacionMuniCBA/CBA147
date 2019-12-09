using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Domicilio : Resultado_Base<Domicilio>
    {
        public virtual Resultado_Barrio Barrio { get; set; }
        public virtual Resultado_Cpc Cpc { get; set; }
        public virtual string Latitud { get; set; }
        public virtual string Longitud { get; set; }
        public string Direccion { get; set; }
        public virtual int Distancia { get; set; }
        public bool Sugerido { get; set; }
        public string Nombre { get; set; }

        public Resultado_Domicilio()
            : base()
        {

        }

        public Resultado_Domicilio(Domicilio entity)
            : base(entity)
        {
            if (entity == null) return;

            Latitud = entity.Latitud;
            Longitud = entity.Longitud;
            Direccion = entity.Direccion;
            Distancia = entity.Distancia;
            Sugerido = entity.Sugerido;

            if (entity.Barrio != null)
            {
                Barrio = new Resultado_Barrio(entity.Barrio);
            }

            if (entity.Cpc != null)
            {
                Cpc = new Resultado_Cpc(entity.Cpc);
            }
        }

        public static List<Resultado_Domicilio> ToList(List<Domicilio> entities)
        {
            return entities.Select(x => new Resultado_Domicilio(x)).ToList();
        }

    }
}

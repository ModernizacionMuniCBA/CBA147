using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Domicilio : BaseEntity
    {
        public virtual Barrio Barrio { get; set; }
        public virtual Cpc Cpc { get; set; }
        public virtual string Latitud { get; set; }
        public virtual string Longitud { get; set; }
        public virtual string Direccion { get; set; }
        public virtual int Distancia { get; set; }
        public virtual bool Sugerido { get; set; }

        public Domicilio()
        {

        }

        public virtual string getDomicilioString(bool mostrarBarrio, bool mostrarCPC, bool mostrarObservaciones)
        {
            var ubicacion = "";

            if (mostrarBarrio && Barrio != null)
            {
                ubicacion += "Barrio: " + Utils.toTitleCase(Barrio.Nombre);
            }

            if (mostrarCPC && Cpc != null)
            {
                ubicacion += ", Cpc: " + Utils.toTitleCase(Cpc.Nombre);
            }

            if (mostrarObservaciones && !String.IsNullOrEmpty(Observaciones))
            {
                ubicacion += ", " + Utils.toTitleCase(Observaciones);
            }

            if (String.IsNullOrEmpty(ubicacion))
            {
                return "Sin Datos";
            }

            return ubicacion;
        }

    }
}

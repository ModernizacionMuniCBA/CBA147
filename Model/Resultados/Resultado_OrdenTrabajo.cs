using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenTrabajo : Resultado_Base<OrdenTrabajo>
    {

        public string Numero { get; set; }
        public string NumeroSolo { get; set; }
        public int Anio { get; set; }
        public string Descripcion { get; set; }
        public string NombreArea{ get; set; }
        public int IdArea { get; set; }
        public int? IdZona { get; set; }
        public string UserAgent { get; set; }
        public int TipoDispositivo { get; set; }
        public Resultado_EstadoOrdenTrabajoHistorial Estado { get; set; }

        public Resultado_OrdenTrabajo() : base() { }

        public Resultado_OrdenTrabajo(OrdenTrabajo entity)
            : base(entity)
        {
            if (entity == null) { return; }
            Numero = entity.GetNumero();
            NumeroSolo = entity.Numero;
            Anio = entity.Año;
            Descripcion = entity.Descripcion;
            IdArea = entity.Area.Id;
            if (entity.Zona != null)
            {
                IdZona = entity.Zona.Id;
            }
            else
            {
                IdZona = null;
            }
            UserAgent = entity.UserAgent;
            TipoDispositivo = (int)entity.TipoDispositivo;
            NombreArea = entity.Area.Nombre;
            
            Estado = new Resultado_EstadoOrdenTrabajoHistorial(entity.GetUltimoEstado());

        }

        public static List<Resultado_OrdenTrabajo> ToList(List<OrdenTrabajo> list)
        {

            return list.Select(x => new Resultado_OrdenTrabajo(x)).ToList();
        }
    }
}

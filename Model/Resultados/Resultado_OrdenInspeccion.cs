using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenInspeccion : Resultado_Base<OrdenInspeccion>
    {

        public string Numero { get; set; }
        public string NumeroSolo { get; set; }
        public int Anio { get; set; }
        public string Descripcion { get; set; }
        public int TipoDispositivo { get; set; }
        public string UserAgent { get; set; }
        public Resultado_EstadoOrdenInspeccionHistorial Estado { get; set; }

        public Resultado_OrdenInspeccion() : base() { }

        public Resultado_OrdenInspeccion(OrdenInspeccion entity)
            : base(entity)
        {
            if (entity == null) { return; }
            Numero = entity.GetNumero();
            NumeroSolo = entity.Numero;
            Anio = entity.Año;
            Descripcion = entity.Descripcion;
            UserAgent = entity.UserAgent;
            TipoDispositivo = (int)entity.TipoDispositivo;

            Estado = new Resultado_EstadoOrdenInspeccionHistorial(entity.GetUltimoEstado());

        }

        public static List<Resultado_OrdenTrabajo> ToList(List<OrdenTrabajo> list)
        {

            return list.Select(x => new Resultado_OrdenTrabajo(x)).ToList();
        }
    }

    [Serializable]
    public class Resultado_EstadoOrdenInspeccionHistorial : Resultado_Base<EstadoOrdenInspeccionHistorial>
    {
        public Resultado_EstadoOrdenInspeccion Estado { get; set; }

        public DateTime Fecha { get; set; }

        public Resultado_EstadoOrdenInspeccionHistorial()
            : base()
        {
        }

        public Resultado_EstadoOrdenInspeccionHistorial(EstadoOrdenInspeccionHistorial entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Estado = new Resultado_EstadoOrdenInspeccion(entity.Estado);
            Fecha = entity.Fecha;
        }

        public static List<Resultado_EstadoOrdenInspeccionHistorial> ToList(List<EstadoOrdenInspeccionHistorial> list)
        {
            return list.Select(x => new Resultado_EstadoOrdenInspeccionHistorial(x)).ToList();
        }
    }

}

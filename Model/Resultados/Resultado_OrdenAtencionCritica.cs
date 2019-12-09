using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrdenAtencionCritica : Resultado_Base<OrdenAtencionCritica>
    {
        public string Descripcion { get; set; }
        public int IdRequerimiento { get; set; }
        public int IdUsuarioCreador { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public DateTime? EstadoFecha { get; set; }

        public Resultado_OrdenAtencionCritica()
            : base()
        {

        }
        public Resultado_OrdenAtencionCritica(OrdenAtencionCritica entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Descripcion = entity.Descripcion;
            if (entity.RequerimientosPorOrdenEspecial != null && entity.RequerimientosPorOrdenEspecial.Count != 0)
                IdRequerimiento = entity.RequerimientosPorOrdenEspecial.FirstOrDefault().Requerimiento.Id;
            IdUsuarioCreador = entity.UsuarioCreador.Id;
            EstadoNombre = entity.GetUltimoEstado().Estado.Nombre;
            EstadoColor = entity.GetUltimoEstado().Estado.Color;

            EstadoFecha = entity.GetUltimoEstado().Estado.FechaAlta;

        }

        public static List<Resultado_OrdenAtencionCritica> ToList(List<OrdenAtencionCritica> list)
        {
            return list.Select(x => new Resultado_OrdenAtencionCritica(x)).ToList();
        }
    }
}

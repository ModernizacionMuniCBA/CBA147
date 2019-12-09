using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Requerimiento : Resultado_Base<Requerimiento>
    {
        public string Numero { get; set; }
        public int Año { get; set; }

        public string ServicioNombre { get; set; }
        public string MotivoNombre { get; set; }
        public int MotivoId { get; set; }
        public string AreaNombre { get; set; }
        public int AreaId { get; set; }
        public string CategoriaNombre { get; set; }
        public int CategoriaId { get; set; }
        public string Descripcion { get; set; }
        public bool Urgente { get; set; }

        public Resultado_Domicilio Domicilio { get; set; }
        public Resultado_EstadoRequerimientoHistorial Estado { get; set; }
        public bool? Favorito { get; set; }
        public bool Marcado { get; set; }

        public Resultado_Requerimiento() : base() { }

        public Resultado_Requerimiento(Requerimiento entity)
            : base(entity)
        {
            if (entity == null) { return; }

            Numero = entity.Numero;
            Año = entity.Año;

            ServicioNombre = entity.Motivo.Tema.Servicio.Nombre;
            MotivoNombre = entity.Motivo.Nombre;
            MotivoId = entity.Motivo.Id;
            if (entity.Motivo.Categoria != null)
            {
                CategoriaNombre = entity.Motivo.Categoria.Nombre;
                CategoriaId = entity.Motivo.Categoria.Id;
            }
            AreaNombre = entity.AreaResponsable.Nombre;
            AreaId = entity.AreaResponsable.Id;

            var primero = true;
            if (entity.Descripciones != null)
            {
                foreach (DescripcionPorRequerimiento d in entity.Descripciones)
                {
                    if (!primero) Descripcion += " /// ";
                    Descripcion += d.Descripcion;
                    primero = false;
                }
            }

            Urgente = entity.Motivo.Urgente;
            Domicilio = new Resultado_Domicilio(entity.Domicilio);
            Estado = new Resultado_EstadoRequerimientoHistorial(entity.GetUltimoEstado());
            Marcado = entity.Marcado;
        }

        public static List<Resultado_Requerimiento> ToList(List<Requerimiento> list)
        {

            return list.Select(x => new Resultado_Requerimiento(x)).ToList();
        }
    }
}

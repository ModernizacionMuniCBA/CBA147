using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Motivo : Resultado_Base<Motivo>
    {
        public string Nombre { get; set; }
        public Resultado_Servicio Servicio { get; set; }
        public int IdArea { get; set; }
        public string NombreArea { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { set; get; }
        public string Keywords { get; set; }
        public bool Urgente { get; set; }
        public bool Principal { get; set; }
        public Enums.PrioridadRequerimiento Prioridad { get; set; }
        public Enums.EsfuerzoMotivo Esfuerzo{ get; set; }
        public Enums.TipoMotivo Tipo { get; set; }
        public IList<Resultado_CampoPorMotivo> Campos { get; set; }

        public Resultado_Motivo()
            : base()
        {

        }

        public Resultado_Motivo(Motivo entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            Nombre = entity.Nombre;
            if (entity.Tema != null && entity.Tema.Servicio != null)
            {
                Servicio = new Resultado_Servicio(entity.Tema.Servicio);
            }
            IdArea = entity.Area.Id;
            NombreArea = entity.Area.Nombre;
            Keywords = entity.Keywords;
            if (entity.Categoria != null)
            {
                IdCategoria = entity.Categoria.Id;
                NombreCategoria = entity.Categoria.Nombre;
            }
            Urgente = entity.Urgente;
            Tipo = entity.Tipo;
            Principal = entity.Principal;
            Prioridad = entity.Prioridad;
            Esfuerzo = entity.Esfuerzo;
            if (entity.Campos != null)
            {
                Campos = Resultado_CampoPorMotivo.ToList(entity.getCampos().ToList());
            }
        }

        public static List<Resultado_Motivo> ToList(List<Motivo> list)
        {
            return list.Select(x => new Resultado_Motivo(x)).ToList();
        }
    }
}

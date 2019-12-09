using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Area : Resultado_Base<CerrojoArea>
    {
        public string Nombre { get; set; }
        public int IdCerrojo { get; set; }
        public string CodigoMunicipal { get; set; }
        public bool CrearOrdenEspecial { get; set; }

        public int IdAreaPadre { get; set; }
        public List<Resultado_Area> Subareas { get; set; }
        public List<Enums.TipoMotivo> TiposMotivoPorDefecto { get; set; }
        public Resultado_Area()
            : base()
        {

        }

        public Resultado_Area(CerrojoArea area)
            : base(area)
        {
            if (area == null)
            {
                return;
            }

            IdCerrojo = area.Id;
            Nombre = area.Nombre;
            CodigoMunicipal = area.CodigoMunicipal;
            CrearOrdenEspecial = area.CrearOrdenEspecial;
            if (area.AreaPadre != null)
            {
                IdAreaPadre = area.AreaPadre.Id;
            }
            if (area.Subareas != null && area.Subareas.Count != 0)
            {
                Subareas = Resultado_Area.ToList(area.Subareas.Where(z=>z.FechaBaja==null).ToList());
            }
            if (area.TiposMotivoPorDefecto != null && area.TiposMotivoPorDefecto.Count != 0)
            {
                TiposMotivoPorDefecto = area.TiposMotivoPorDefecto.Where(x=>x.FechaBaja==null && x.PorDefecto).Select(x=>x.TipoMotivoPorDefecto).ToList();
            }
        }
        public static List<Resultado_Area> ToList(List<CerrojoArea> list)
        {
            return list.Select(x => new Resultado_Area(x)).ToList();
        }

    }
}

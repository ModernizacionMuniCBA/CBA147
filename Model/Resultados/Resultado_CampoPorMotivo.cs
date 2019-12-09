using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_CampoPorMotivo
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public int IdTipoCampoPorMotivo { get; set; }
        public int KeyValueTipoCampo { get; set; }
        public string NombreTipoCampo { get; set; }
        public string Grupo { get; set; }
        public bool? Obligatorio { get; set; }
        public int? Orden { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FechaBaja { get; set; }
        public IList<string> Opciones { get; set; }

        public Resultado_CampoPorMotivo()
            : base()
        {

        }

        public Resultado_CampoPorMotivo(CampoPorMotivo campo)
        {
            if (campo == null)
            {
                return;
            }

            Id = campo.Id;
            FechaBaja = campo.FechaBaja;
            Nombre = campo.Nombre;
            IdTipoCampoPorMotivo = campo.Tipo.Id;
            KeyValueTipoCampo = (int)campo.Tipo.KeyValue;
            NombreTipoCampo = campo.Tipo.Nombre;
            Grupo = campo.Grupo;
            Obligatorio = campo.Obligatorio;
            Orden = campo.Orden;
            Observaciones = campo.Observaciones;
            if (campo.Opciones != null)
            {
                Opciones = campo.Opciones.Split('^');
            }
        }

        public static List<Resultado_CampoPorMotivo> ToList(List<CampoPorMotivo> list)
        {
            return list.Select(x => new Resultado_CampoPorMotivo(x)).ToList();
        }

    }
}

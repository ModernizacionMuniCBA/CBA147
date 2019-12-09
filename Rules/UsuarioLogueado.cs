using System;
using System.Collections.Generic;
using System.Linq;
using Model.Entities;
using Rules.Rules;
using Model.Resultados;
using Model.WSVecinoVirtual.Resultados;

namespace Rules
{
    [Serializable]
    public class UsuarioLogueado
    {
        public _Resultado_VecinoVirtualUsuario Usuario { get; set; }
        public string Token { get; set; }
        public List<Resultado_Area> Areas { get; set; }
        public List<int> IdsAreas
        {
            get
            {
                if (Areas == null) return new List<int>();
                return Areas.Select(x => x.Id).ToList();
            }
        }
        public Resultado_Ambito Ambito { get; set; }
        public bool EsAmbitoMunicipalidad()
        {
            return Ambito == null || Ambito.KeyValue == 0;
        }

        public bool EsAmbitoCPC()
        {
            return !EsAmbitoMunicipalidad();
        }

        public List<VVResultado_Menu> Menu { get; set; }
        public VVResultado_Permisos Rol { get; set; }
        public List<Resultado_Origen> OrigenesDisponibles { get; set; }
        public int? IdOrigenElegido { get; set; }

        public UsuarioLogueado()
        {
        }
    }
}

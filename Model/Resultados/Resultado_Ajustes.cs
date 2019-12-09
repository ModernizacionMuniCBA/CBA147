using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Ajustes : Resultado_Base<Ajustes>
    {
        public virtual string App { get; set; }

        public Resultado_Ajustes()
            : base()
        {
        }

        public Resultado_Ajustes(Ajustes entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            App = entity.App;
        }

        public static List<Resultado_Ajustes> ToList(List<Ajustes> list)
        {
            return list.Select(x => new Resultado_Ajustes(x)).ToList();
        }
    }
}

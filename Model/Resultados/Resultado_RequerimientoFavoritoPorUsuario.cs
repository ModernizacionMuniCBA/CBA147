using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_RequerimientoFavoritoPorUsuario : Resultado_Base<RequerimientoFavoritoPorUsuario>
    {
        public virtual int UserId{ get; set; }
        public virtual int RequerimientoId { get; set; }
        public virtual string RequerimientoNumero { get; set; }
        public virtual int RequerimientoAño { get; set; }

        public Resultado_RequerimientoFavoritoPorUsuario()
            : base()
        {

        }

        public Resultado_RequerimientoFavoritoPorUsuario(RequerimientoFavoritoPorUsuario entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            UserId = entity.Usuario.Id;
            RequerimientoId = entity.Requerimiento.Id;
            RequerimientoNumero = entity.Requerimiento.Numero;
            RequerimientoAño = entity.Requerimiento.Año;

        }

        public static List<Resultado_RequerimientoFavoritoPorUsuario> ToList(List<RequerimientoFavoritoPorUsuario> list)
        {
            return list.Select(x => new Resultado_RequerimientoFavoritoPorUsuario(x)).ToList();
        }
    }
}

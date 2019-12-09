using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class OrdenAtencionCritica : BaseEntity
    {

        public virtual string Descripcion { get; set; }
        public virtual _VecinoVirtualUsuario UsuarioCreador { get; set; }

        public virtual IList<EstadoOrdenEspecialHistorial> Estados { get; set; }

        public virtual EstadoOrdenEspecialHistorial GetUltimoEstado()
        {
            if (Estados == null) return null;
            return Estados.Where(x => x.Ultimo == true && x.FechaBaja == null).FirstOrDefault();
        }

        public virtual IList<RequerimientoPorOrdenEspecial> RequerimientosPorOrdenEspecial { get; set; }

    }
}

using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_OrigenPorUsuario : Resultado_Base<OrigenPorUsuario>
    {
        public virtual int UsuarioOrigenId { get; set; }
        public virtual string UsuarioOrigenNombre { get; set; }
        public virtual string UsuarioOrigenApellido { get; set; }
        public virtual int OrigenId { get; set; }
        public virtual string OrigenNombre { get; set; }

        public Resultado_OrigenPorUsuario()
            : base()
        {

        }

        public Resultado_OrigenPorUsuario(OrigenPorUsuario entity)
            : base(entity)
        {

            if (entity == null)
            {
                return;
            }

            if (entity.UsuarioOrigen != null)
            {
                UsuarioOrigenId = entity.UsuarioOrigen.Id;
                UsuarioOrigenNombre = entity.UsuarioOrigen.Nombre;
                UsuarioOrigenApellido = entity.UsuarioOrigen.Apellido;
            }
            if (entity.Origen != null)
            {
                OrigenId = entity.Origen.Id;
                OrigenNombre = entity.Origen.Nombre;
            }
        }

        public static List<Resultado_OrigenPorUsuario> ToList(List<OrigenPorUsuario> list)
        {
            return list.Select(x => new Resultado_OrigenPorUsuario(x)).ToList();
        }
    }
}

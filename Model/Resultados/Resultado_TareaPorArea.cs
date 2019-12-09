using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_TareaPorArea 
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string AreaNombre { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string Observaciones { get; set; }

        // Modificacion
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public string UsuarioModificacionNombre { get; set; }
        public string UsuarioModificacionApellido { get; set; }
        public string UsuarioModificacionUsername { get; set; }


        public Resultado_TareaPorArea()
            : base()
        {

        }

        public Resultado_TareaPorArea(TareaPorArea entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
            AreaNombre = entity.Area.Nombre;
            FechaBaja = entity.FechaBaja;
            Observaciones = entity.Observaciones;

            //Modificacion
            FechaModificacion = entity.FechaModificacion;
            UsuarioModificacionId = entity.Usuario.Id;
            UsuarioModificacionNombre = entity.Usuario.Nombre;
            UsuarioModificacionApellido = entity.Usuario.Apellido;
            UsuarioModificacionUsername = entity.Usuario.Username;
        }
        public static List<Resultado_TareaPorArea> ToList(List<TareaPorArea> list)
        {
            return list.Select(x => new Resultado_TareaPorArea(x)).ToList();
        }
    }
}

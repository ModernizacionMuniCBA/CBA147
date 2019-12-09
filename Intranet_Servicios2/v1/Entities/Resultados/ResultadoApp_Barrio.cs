using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoApp_Barrio
    {
        public virtual int Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual int IdCatastro { get; set; }

        public ResultadoApp_Barrio()
        {

        }

        public ResultadoApp_Barrio(Barrio entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
            IdCatastro = entity.IdCatastro;
        }

        public static List<ResultadoApp_Barrio> ToList(List<Barrio> entities)
        {
            return entities.Select(x => new ResultadoApp_Barrio(x)).ToList();
        }

    }
}

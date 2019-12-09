using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intranet_Servicios2.v1.Entities.Resultados
{
    [Serializable]
    public class ResultadoApp_Cpc
    {
        public virtual int Id { get; set; }
        public virtual int IdCatastro { get; set; }
        public virtual int Numero { get; set; }
        public virtual string Nombre { get; set; }

        public ResultadoApp_Cpc()
        {

        }

        public ResultadoApp_Cpc(Cpc entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            IdCatastro = entity.IdCatastro;
            Nombre = entity.Nombre;
            Numero = entity.Numero;
        }
        public static List<ResultadoApp_Cpc> ToList(List<Cpc> list)
        {
            return list.Select(x => new ResultadoApp_Cpc(x)).ToList();
        }
    }
}

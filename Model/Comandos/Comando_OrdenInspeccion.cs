using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_OrdenInspeccion
    {
        public virtual int? Id { get; set; }

        //Requerimientos
        public virtual List<int> IdRequerimientos { get; set; }
        //Descripcion
        public virtual string Descripcion { get; set; }
        public string UserAgent { get; set; }
        public Enums.TipoDispositivo TipoDispositivo { get; set; }
        public List<Comando_NotaOrdenTrabajo> Notas { get; set; }

        public Comando_OrdenInspeccion()
        {

        }
    }
}

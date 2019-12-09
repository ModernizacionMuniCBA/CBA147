using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_OrdenTrabajo
    {
        public virtual int? Id { get; set; }

        //Requerimientos
        public virtual List<Comando_RequerimientoPorOrdenTrabajo> Requerimientos { get; set; }

        public virtual Enums.EstadoRequerimiento? KeyValueEstadoRequerimiento { get; set; }
        public virtual Enums.EstadoOrdenTrabajo? KeyValueEstadoOrdenTrabajo { get; set; }

        public virtual List<int> IdRequerimientos { get; set; }
        //Seccion
        public virtual int? IdSeccion { get; set; }
        //Area
        public virtual int IdArea { get; set; }
        //Descripcion
        public virtual string Descripcion { get; set; }
        //Recursos
        public virtual Comando_Recursos Recursos { get; set; }
        public string UserAgent { get; set; }
        public Enums.TipoDispositivo TipoDispositivo { get; set; }
        public List<Comando_NotaOrdenTrabajo> Notas { get; set; }
        public List<int> Moviles { get; set; }
        public List<int> Personal { get; set; }
        public List<int> Flotas { get; set; }

        public Comando_OrdenTrabajo()
        {

        }
    }
}

using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{

    [Serializable]
    public class Comando_Motivo
    {
        public int? Id { get; set; }
        public int IdArea{get;set;}
        public int? IdServicio { get; set; }
        public int? IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {get;set;}
        public string Keywords {get;set;}
        public bool Urgente { get; set; }
        public bool Principal { get; set; }
        public Enums.PrioridadRequerimiento Prioridad { get; set; }
        public Enums.EsfuerzoMotivo Esfuerzo { get; set; }
        public Enums.TipoMotivo Tipo{ get; set; }
        public List<Comando_Motivo_Campo> Campos { get; set; }

        public Comando_Motivo()
        {

        }
    }

    public class Comando_Motivo_Campo
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public int IdTipoCampoPorMotivo { get; set; }
        public int IdMotivo { get; set; }
        public int? Orden { get; set; }
        public bool Obligatorio { get; set; }
        public string Observaciones { get; set; }
        public string Grupo { get; set; }
        public IList<string> Opciones { get; set; }

        public Comando_Motivo_Campo()
        {

        }
    }

}

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
    //[XmlRoot("Comando_Motivo", Namespace = "http://example.com/schemas/Comando_Movil")]
    public class Comando_Movil_Reparacion
    {
        public virtual int? Id { get; set; }
        public virtual int IdMovil { get; set; }
        public virtual string Motivo { get; set; }
        public virtual string Taller { get; set; }
        public virtual int MontoReparacion{ get; set; }
        public virtual DateTime FechaReparacion { get; set; }
        public virtual string Observaciones { get; set; }


        public Comando_Movil_Reparacion()
        {
       
        }

        //public Comando_Movil(Movil movil)
        //{
        //    if (movil.Id != 0)
        //    {
        //        this.Id = movil.Id;
        //    }
        //    this.IdArea = movil.Area.Id;
        //    this.IdServicio = movil.Tema.Servicio.Id;
        //    this.Nombre = movil.Nombre;
        //}
    }
}

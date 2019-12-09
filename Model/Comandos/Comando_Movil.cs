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
    public class Comando_Movil
    {
        public virtual int? Id { get; set; }
        public virtual int IdArea { get; set; }

        //Datos Identificatorios
        public virtual int IdTipo { get; set; }
        public virtual int? Año { get; set; }
        public virtual string Modelo { get; set; }
        public virtual string Marca { get; set; }
        public virtual string Dominio { get; set; }
        public virtual DateTime? FechaIncorporacion { get; set; }
        public virtual string NumeroInterno { get; set; }

        //Caracteristicas
        public virtual string Carga { get; set; }
        public virtual int? Asientos { get; set; }
        public virtual Enums.TipoCombustible? IdTipoCombustible { get; set; }


        //Datos adicionales
        public virtual float? Valuacion { get; set; }
        public virtual DateTime? FechaValuacion { get; set; }
        public virtual int? Kilometraje { get; set; }
        public virtual DateTime? FechaKilometraje { get; set; }
        public virtual DateTime? VencimientoITV { get; set; }
        public virtual DateTime? VencimientoTUV { get; set; }
        public virtual Enums.EstadoMovil? IdEstado { get; set; }
        public virtual Enums.CondicionMovil? IdCondicion { get; set; }
        public virtual string Caracteristicas { get; set; }

        public Comando_Movil()
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

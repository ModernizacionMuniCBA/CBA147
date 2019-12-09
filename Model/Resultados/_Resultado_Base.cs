using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Base<Entidad> where Entidad : BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime? FechaAlta { get; set; }
        public virtual string FechaAltaString { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        public virtual string FechaBajaString { get; set; }
        public virtual DateTime? FechaModificacion { get; set; }
        public virtual string FechaModificacionString { get; set; }
        public virtual string Observaciones { get; set; }
        public virtual int IdUsuario { get; set; }

        public Resultado_Base()
        {
        }

        public Resultado_Base(Entidad baseEntity)
        {
            if (baseEntity == null)
            {
                return;
            }

            Id = baseEntity.Id;

            FechaAlta = baseEntity.FechaAlta;
            var fechaAltaString = Utils.DateTimeToString(FechaAlta);
            if (fechaAltaString == null)
            {
                fechaAltaString = "";
            }
            FechaAltaString = fechaAltaString;

            FechaBaja = baseEntity.FechaBaja;
            var fechaBajaString = Utils.DateTimeToString(FechaBaja);
            if (fechaBajaString == null)
            {
                fechaBajaString = "";
            }
            FechaBajaString = fechaBajaString;

            FechaModificacion = baseEntity.FechaModificacion;
            var fechaModificacionString = Utils.DateTimeToString(FechaModificacion);
            if (fechaModificacionString == null)
            {
                fechaModificacionString = "";
            }
            FechaModificacionString = fechaModificacionString;

            Observaciones = baseEntity.Observaciones;

            if (baseEntity.Usuario != null)
            {
                IdUsuario = baseEntity.Usuario.Id;
            }
        }

        //public override bool Equals(object obj)
        //{
        //    var item = obj as Resultado_Base<BaseEntity>;
        //    if (Id == 0 || item.Id == 0) return false;
        //    return Id == item.Id;
        //}
        //public static List<Resultado_Base<Entidad>> Crear(List<Entidad> baseEntityList)
        //{
        //    var resultado = new List<Resultado_Base<Entidad>>();
        //    foreach (Entidad entity in baseEntityList)
        //    {
        //        resultado.Add(Resultado_Base<Entidad>.Crear(entity));
        //    }
        //    return resultado;
        //}

        //public static Resultado_Base<Entidad> Crear(Entidad baseEntity)
        //{
        //    var resultado = new Resultado_Base<Entidad>();
        //    resultado.Id = baseEntity.Id;

        //    resultado.FechaAlta = baseEntity.FechaAlta;
        //    var fechaAltaString = Utils.DateTimeToString(resultado.FechaAlta);
        //    if (fechaAltaString == null)
        //    {
        //        fechaAltaString = "";
        //    }
        //    resultado.FechaAltaString = fechaAltaString;

        //    resultado.FechaBaja = baseEntity.FechaBaja;
        //    var fechaBajaString = Utils.DateTimeToString(resultado.FechaBaja);
        //    if (fechaBajaString == null)
        //    {
        //        fechaBajaString = "";
        //    }
        //    resultado.FechaBajaString = fechaBajaString;

        //    resultado.FechaModificacion = baseEntity.FechaModificacion;
        //    var fechaModificacionString = Utils.DateTimeToString(resultado.FechaModificacion);
        //    if (fechaModificacionString == null)
        //    {
        //        fechaModificacionString = "";
        //    }
        //    resultado.FechaModificacionString = fechaModificacionString;

        //    resultado.Observaciones = baseEntity.Observaciones;

        //    if (baseEntity.Usuario != null)
        //    {
        //        resultado.IdUsuario = baseEntity.Usuario.Id;
        //    }

        //    return resultado;

        //}
    }


}

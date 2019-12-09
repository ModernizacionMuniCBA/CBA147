using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;

namespace Model
{

    [Serializable()]
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime? FechaAlta { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        public virtual DateTime? FechaModificacion { get; set; }
        public virtual string Observaciones { get; set; }
        //public virtual Usuario Usuario { get; set; }
        public virtual _VecinoVirtualUsuario Usuario { get; set; }

        //public override bool Equals(object obj)
        //{
        //    var item = obj as BaseEntity;
        //    if (Id == 0 || item.Id == 0) return false;
        //    return Id == item.Id;
        //}
    }
}

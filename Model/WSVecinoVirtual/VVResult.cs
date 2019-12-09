using System;
using System.Linq;

namespace Model.WSVecinoVirtual
{
    [Serializable]
    public class VVResult<Entity>
    {
        public Entity Return { get; set; }
        public string Error { get; set; }

        public bool Ok
        {
            get
            {
                return string.IsNullOrEmpty(Error);
            }
        }

    }
}

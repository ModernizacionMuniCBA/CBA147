using System;
using System.Linq;

namespace Internet_UI.Utils
{
    [Serializable]
    public class Resultado<Entity>
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
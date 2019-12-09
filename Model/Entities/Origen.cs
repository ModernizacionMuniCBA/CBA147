using System;
using System.Collections.Generic;
using System.Linq;


namespace Model.Entities
{
    public class Origen : BaseEntity
    {
        public virtual string Nombre { get; set; }
        public virtual string KeyAlias { get; set; }
        public virtual string KeySecret { get; set; }

        public Origen():base()
        {
            
        }

        public Origen(Comandos.Comando_Origen comando, string key, string keySecret)
        {
            this.Nombre = comando.Nombre;
            this.KeyAlias = key;
            this.KeySecret = keySecret;
        }

    }
}

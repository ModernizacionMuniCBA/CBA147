using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet_Servicios2.v1.Entities.Comandos
{
    public class ComandoExterno_RequerimientoCambiarEstado
    {
        public int IdRequerimiento { get; set; }
        public int KeyValueEstado { get; set; }
        public string Descripcion {get;set;}

        public ComandoExterno_RequerimientoCambiarEstado()
        {

        }
    }

}
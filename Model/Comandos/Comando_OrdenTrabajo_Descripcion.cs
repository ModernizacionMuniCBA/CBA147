﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    public class Comando_OrdenTrabajo_Descripcion
    {
        public int IdOrdenTrabajo { get; set; }
        public string Descripcion { get; set; }

        public Comando_OrdenTrabajo_Descripcion()
        {

        }
    }
}

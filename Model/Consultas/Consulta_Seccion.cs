﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Consultas
{
    public class Consulta_Seccion
    {
        public string Nombre { get; set; }
        public bool? DadosDeBaja { get; set; }
        public List<int> IdsArea { get; set; }

        public Consulta_Seccion()
        {
            DadosDeBaja = false;
        }

        public Consulta_Seccion(string nombre, bool? deBaja, int idArea)
        {
            Nombre = nombre;
            DadosDeBaja = deBaja;
            IdsArea = new List<int>();
            IdsArea.Add(idArea);
        }
    }
}

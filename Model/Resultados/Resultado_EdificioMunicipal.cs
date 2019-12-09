using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_EdificioMunicipal 
    {
        public string Nombre { get; set; }
        public int Id{get;set;}
        public Resultado_Domicilio Domicilio {get;set;}
        public int IdCategoria{get;set;}
        public Resultado_EdificioMunicipal()
        {

        }

        public Resultado_EdificioMunicipal(EdificioMunicipal entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Nombre = entity.Nombre;
            Domicilio = new Resultado_Domicilio(entity.Domicilio);
            IdCategoria = entity.Categoria.Id;
        }
    }
}

using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_Personal : Resultado_Base<Personal>
    {
        public int IdPersonaFisica { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroDocumento { get; set; }
        public int IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public int IdFuncion { get; set; }
        public string Funcion { get; set; }

        public Resultado_Personal()
            : base()
        {

        }

        public Resultado_Personal(Personal entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }

            var persona = entity.PersonaFisica;
            IdPersonaFisica = persona.Id;
            Nombre = persona.Nombre;
            Apellido = persona.Apellido;
            NumeroDocumento = persona.NroDoc;

            var tipoDoc = persona.TipoDocumento;
            IdTipoDocumento = tipoDoc.Id;
            TipoDocumento = tipoDoc.Nombre;
        }
    }
}

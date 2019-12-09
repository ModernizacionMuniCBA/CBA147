using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_InformacionOrganicaDireccion : Resultado_Base<InformacionOrganicaDireccion>
    {
        public string Nombre { get; set; }
        public string Responsable { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Domicilio { get; set; }
        public Resultado_InformacionOrganicaSecretaria Secretaria { get; set; }

        public Resultado_InformacionOrganicaDireccion()
            : base()
        {

        }

        public Resultado_InformacionOrganicaDireccion(InformacionOrganicaDireccion entity)
            : base(entity)
        {
            if (entity == null)
            {
                return;
            }


            this.Nombre = entity.Nombre;
            this.Telefono = entity.Telefono;
            this.Email = entity.Email;
            this.Responsable = entity.Responsable;
            this.Domicilio = entity.Domicilio;
            this.Secretaria = new Resultado_InformacionOrganicaSecretaria(entity.Secretaria);
        }

        public static List<Resultado_InformacionOrganicaDireccion> ToList(List<InformacionOrganicaDireccion> list)
        {
            return list.Select(x => new Resultado_InformacionOrganicaDireccion(x)).ToList();
        }

    }
}

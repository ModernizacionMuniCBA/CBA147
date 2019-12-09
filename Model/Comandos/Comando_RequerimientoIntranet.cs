using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Comandos
{
    [Serializable]
    [XmlRoot("Comando_RequerimientoIntranet", Namespace = "http://example.com/schemas/Comando_RequerimientoIntranet")]
    public class Comando_RequerimientoIntranet
    {
        public int? Id { get; set; }
        public int? IdRequerimientoUnir { get; set; }
        public bool RelevamientoInterno { get; set; }
        public int IdMotivo { get; set; }
        public string Descripcion { get; set; }

        public Comando_Domicilio Domicilio { get; set; }

        public int? IdUsuarioReferente { get; set; }
        public string ObservacionesUsuarioReferente{get;set;}

        public List<int> IdArchivos { get; set; }
        public List<Comando_Nota> Notas { get; set; }
        public List<Comando_CampoDinamico> CamposDinamicos { get; set; }

        public string UserAgent { get; set; }
        public Enums.TipoDispositivo TipoDispositivo { get; set; }

        public Enums.EstadoRequerimiento? EstadoKeyValue { get; set; }
        public string EstadoMotivo { get; set; }

        public string OrigenAlias { get; set; }
        public string OrigenSecret { get; set; }

        public Comando_ReferenteProvisorio ReferenteProvisorio { get; set; }

        public Comando_RequerimientoIntranet()
        {
            RelevamientoInterno = false;
            IdArchivos = new List<int>();
            Notas = new List<Comando_Nota>();
            TipoDispositivo = Enums.TipoDispositivo.DESCKTOP;
        }

        public class Comando_CampoDinamico
        {
            public int Id{get;set;}
            public string Valor{get;set;}
        }
        public class Comando_ReferenteProvisorio
        {
            public int IdRequerimiento { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public int? DNI { get; set; }
            public bool GeneroMasculino { get; set; }
            public string Telefono { get; set; }
            public string Observaciones { get; set; }
        }
        public string Imagen { get; set; }
    }


}

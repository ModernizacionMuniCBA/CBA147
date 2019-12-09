using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_ArchivoPorRequerimiento_Documento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Identificador { get; set; }
        public string Url { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string UsuarioReferenteNombre { get; set; }
        public string UsuarioReferenteApellido { get; set; }
        public int? UsuarioReferenteId { get; set; }

      public Resultado_ArchivoPorRequerimiento_Documento(ArchivoPorRequerimiento documento, string server, JObject data): base()
        {
              Id = documento.Id;
                        Nombre = data.GetValue("fileName").ToObject<string>();
                        Identificador = documento.Identificador;
                        Url = server + "/Archivo/" + documento.Identificador;
                        ContentLength = data.GetValue("contentLength").ToObject<int>();
                        ContentType = data.GetValue("contentType").ToObject<string>();
          if(documento.UsuarioReferente!=null){
                        UsuarioReferenteNombre=documento.UsuarioReferente.Nombre;
                    UsuarioReferenteApellido=documento.UsuarioReferente.Apellido;
                        UsuarioReferenteId=documento.UsuarioReferente.Id;}
                    }
        
    }
}

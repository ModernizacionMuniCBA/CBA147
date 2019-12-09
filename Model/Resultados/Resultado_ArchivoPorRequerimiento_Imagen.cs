using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Resultados
{
    [Serializable]
    public class Resultado_ArchivoPorRequerimiento_Imagen
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Identificador { get; set; }
        public string Url { get; set; }
        public string UrlPreview { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string UsuarioReferenteNombre { get; set; }
        public string UsuarioReferenteApellido { get; set; }
        public int? UsuarioReferenteId { get; set; }

        public Resultado_ArchivoPorRequerimiento_Imagen(ArchivoPorRequerimiento imagen, string server)
            : base()
        {
            Id = imagen.Id;
            Nombre = imagen.Nombre;
            Identificador = imagen.Identificador;
            Url = server + "/Archivo/" + imagen.Identificador;
            UrlPreview = server + "/Archivo/" + imagen.Identificador + "/3";
            ContentLength = imagen.ContentLength;
            ContentType = imagen.ContentType;
            Width = imagen.Width;
            Height = imagen.Height;
            if (imagen.UsuarioReferente != null)
            {
                UsuarioReferenteNombre = imagen.UsuarioReferente.Nombre;
                UsuarioReferenteApellido = imagen.UsuarioReferente.Apellido;
                UsuarioReferenteId = imagen.UsuarioReferente.Id;
            }
        }
    }
}

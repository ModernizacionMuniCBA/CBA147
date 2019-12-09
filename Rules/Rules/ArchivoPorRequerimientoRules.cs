using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Comandos;
using Model.Resultados;
using Model.Consultas;
using System.Configuration;
using System.Web;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using RestSharp;
using Newtonsoft.Json.Linq;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;

namespace Rules.Rules
{
    public class ArchivoPorRequerimientoRules : BaseRules<ArchivoPorRequerimiento>
    {

        private readonly ArchivoPorRequerimientoDAO dao;

        public ArchivoPorRequerimientoRules(UsuarioLogueado data)
            : base(data)
        {
            dao = ArchivoPorRequerimientoDAO.Instance;
        }


        public Result<int> Insertar(Comando_Archivo comando)
        {
            var resultado = new Result<int>();

            try
            {
                var urlServidor = ConfigurationManager.AppSettings["URL_SERVIDOR_CORDOBA_FILES"];
                var apiKey = ConfigurationManager.AppSettings["API_KEY_CORDOBA_FILES"];
                var url = urlServidor + "/Archivo/v1/InsertarBase64?apiKey=" + apiKey;

                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);

                var comandoInsertar = new
                {
                    FileName = comando.Nombre,
                    Content = comando.Data
                };
                request.AddBody(comandoInsertar);

                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request).Result;

                var respuesta = JObject.Parse(response.Content);
                if (respuesta.GetValue("status").ToObject<string>() == "ok")
                {
                    var data = respuesta.GetValue("data").ToObject<JObject>();

                    ArchivoPorRequerimiento archivo = new ArchivoPorRequerimiento();

                    archivo.Nombre = data.GetValue("fileName").ToObject<string>();
                    archivo.Identificador = data.GetValue("identificador").ToObject<string>();
                    archivo.ContentLength = data.GetValue("contentLength").ToObject<int>();
                    archivo.ContentType = data.GetValue("contentType").ToObject<string>();

                    //Calculo el tipo
                    if (data.GetValue("tipoKeyValue").ToObject<int>() == 1)
                    {
                        archivo.Width = data.GetValue("width").ToObject<int>();
                        archivo.Height = data.GetValue("height").ToObject<int>();
                        archivo.Tipo = Enums.TipoArchivo.IMAGEN;
                    }
                    else
                    {
                        archivo.Tipo = Enums.TipoArchivo.DOCUMENTO;
                    }

                    var idUsuario = comando.IdUsuarioCerrojoReferente;

                    if (idUsuario.HasValue)
                    {
                        var resultadoConsultaUsuario = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetById(idUsuario.Value);
                        if (!resultadoConsultaUsuario.Ok)
                        {
                            resultado.Copy(resultadoConsultaUsuario.Errores);
                            return resultado;
                        }
                        archivo.UsuarioReferente = resultadoConsultaUsuario.Return;
                    }

                    var resultadoInsert = base.Insert(archivo);
                    if (!resultadoInsert.Ok)
                    {
                        resultado.Copy(resultadoInsert.Errores);
                        return resultado;
                    }

                    resultado.Return = resultadoInsert.Return.Id;
                }
                else
                {
                    resultado.AddErrorPublico("Error insertando el archivo");
                }


            }
            catch (Exception e)
            {
                resultado.AddErrorInterno(e);
            }
            return resultado;
        }

        public Result<List<ArchivoPorRequerimiento>> GetByFilters(Consulta_ArchivoPorRequerimiento consulta)
        {
            return dao.GetByFilters(consulta);
        }

        public Result<List<Resultado_ArchivoPorRequerimiento_Imagen>> GetResultadoImagenesByFilters(string server, Consulta_ArchivoPorRequerimiento consulta)
        {
            var resultado = new Result<List<Resultado_ArchivoPorRequerimiento_Imagen>>();
            consulta.Tipo = Enums.TipoArchivo.IMAGEN;

            var resultadoConsulta = GetByFilters(consulta);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var lista = new List<Resultado_ArchivoPorRequerimiento_Imagen>();

            foreach (var imagen in resultadoConsulta.Return)
            {
                lista.Add(new Resultado_ArchivoPorRequerimiento_Imagen(imagen, server));

                var serverInfo = ConfigurationManager.AppSettings["URL_SERVIDOR_CORDOBA_FILES"];
                var client = new RestClient(serverInfo + "/Archivo/v1/Info?key=" + imagen.Identificador);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request).Result;
                var respuesta = JObject.Parse(response.Content);
                if (respuesta.GetValue("ok").ToObject<bool>())
                {
                    var data = respuesta.GetValue("data").ToObject<JObject>();

                }
            }

            resultado.Return = lista;
            return resultado;
        }

        public Result<List<Resultado_ArchivoPorRequerimiento_Documento>> GetResultadoDocumentosByFilters(string server, Consulta_ArchivoPorRequerimiento consulta)
        {
            var resultado = new Result<List<Resultado_ArchivoPorRequerimiento_Documento>>();
            consulta.Tipo = Enums.TipoArchivo.DOCUMENTO;

            var resultadoConsulta = GetByFilters(consulta);
            if (!resultadoConsulta.Ok)
            {
                resultado.Copy(resultadoConsulta.Errores);
                return resultado;
            }

            var lista = new List<Resultado_ArchivoPorRequerimiento_Documento>();

            foreach (var doc in resultadoConsulta.Return)
            {
                var serverInfo = ConfigurationManager.AppSettings["URL_SERVIDOR_CORDOBA_FILES"];
                var client = new RestClient(serverInfo + "/Archivo/v1/Info?key=" + doc.Identificador);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                IRestResponse response = client.Execute(request).Result;
                var respuesta = JObject.Parse(response.Content);
                if (respuesta.GetValue("ok").ToObject<bool>())
                {
                    var data = respuesta.GetValue("data").ToObject<JObject>();
                    lista.Add(new Resultado_ArchivoPorRequerimiento_Documento(doc, server, data));
                     
                }
            }

            resultado.Return = lista;
            return resultado;
        }

        //public Result<List<int>> GetIdsByFilters(Consulta_ArchivoPorRequerimiento consulta)
        //{
        //    return dao.GetIdsByFilters(consulta);
        //}

        //public Result<List<Resultado_ArchivoPorRequerimiento>> GetResultadoByFilters(Consulta_ArchivoPorRequerimiento consulta)
        //{
        //    var resultado = new Result<List<Resultado_ArchivoPorRequerimiento>>();
        //    var resultadoConsulta = dao.GetByFilters(consulta);
        //    if (!resultadoConsulta.Ok)
        //    {
        //        resultado.Copy(resultadoConsulta.Errores);
        //        return resultado;
        //    }

        //    resultado.Return = Resultado_ArchivoPorRequerimiento.ToList(resultadoConsulta.Return);
        //    return resultado;
        //}

        //public Result<Resultado_ArchivoPorRequerimiento> GetResultadoById(int id)
        //{
        //    var resultado = new Result<Resultado_ArchivoPorRequerimiento>();
        //    var resultadoConsulta = dao.GetById(id);
        //    if (!resultadoConsulta.Ok)
        //    {
        //        resultado.Copy(resultadoConsulta.Errores);
        //        return resultado;
        //    }

        //    if (resultadoConsulta.Return != null)
        //    {
        //        resultado.Return = new Resultado_ArchivoPorRequerimiento(resultadoConsulta.Return);
        //    }
        //    return resultado;
        //}

        //public Result<Resultado_ArchivoPorRequerimiento_Imagen> GetImagenById(string server, int id)
        //{
        //    var resultado = new Result<Resultado_ArchivoPorRequerimiento_Imagen>();

        //    var consulta = GetResultadoById(id);
        //    if (!consulta.Ok)
        //    {
        //        resultado.AddErrorPublico("Error");
        //        return resultado;
        //    }

        //    if (consulta.Return == null)
        //    {
        //        resultado.Return = null;
        //        return resultado;
        //    }

        //    try
        //    {
        //        if (!System.IO.Directory.Exists(server + "/" + carpeta))
        //        {
        //            System.IO.Directory.CreateDirectory(server + "/" + carpeta);
        //        }

        //        imagen = new Bitmap(imagen, ResizeKeepAspect(imagen.Size, 1000, 1000));
        //        Bitmap imagen1 = new Bitmap(imagen, ResizeKeepAspect(imagen.Size, 500, 500));
        //        Bitmap imagen2 = new Bitmap(imagen, ResizeKeepAspect(imagen.Size, 250, 250));
        //        Bitmap imagen3 = new Bitmap(imagen, ResizeKeepAspect(imagen.Size, 100, 100));
        //        Bitmap imagen4 = new Bitmap(imagen, ResizeKeepAspect(imagen.Size, 50, 50));

        //        imagen.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        //        imagen1.Save(path1, System.Drawing.Imaging.ImageFormat.Png);
        //        imagen2.Save(path2, System.Drawing.Imaging.ImageFormat.Png);
        //        imagen3.Save(path3, System.Drawing.Imaging.ImageFormat.Png);
        //        imagen4.Save(path4, System.Drawing.Imaging.ImageFormat.Png);

        //        System.Drawing.Image img = System.Drawing.Image.FromFile(path);
        //        imagenRequerimiento.W = img.Width;
        //        imagenRequerimiento.H = img.Height;

        //        resultado.Return = imagenRequerimiento;
        //        return resultado;
        //    }
        //    catch (Exception e)
        //    {
        //        resultado.AddErrorPublico("Error");
        //        return resultado;
        //    }
        //}

        //public Result<Resultado_ArchivoPorRequerimiento_Documento> GetDocumentoById(string server, int id)
        //{
        //    var resultado = new Result<Resultado_ArchivoPorRequerimiento_Documento>();

        //    var carpeta = ConfigurationManager.AppSettings["CARPETA_DOCUMENTOS"];
        //    var nombreArchivo = EncriptionHelper.Encrypt("" + id);
        //    Regex rgx = new Regex("[^a-zA-Z]");
        //    var resultadoExtension = dao.GetExtension(id);
        //    if (!resultadoExtension.Ok)
        //    {
        //        resultado.AddErrorPublico("El archivo esta currupto");
        //        return resultado;
        //    }
        //    nombreArchivo = rgx.Replace(nombreArchivo, "") + "." + resultadoExtension.Return;

        //    var path = server + "/" + carpeta + "/" + nombreArchivo;


        //    var archivoRequerimiento = new Resultado_ArchivoPorRequerimiento_Documento();
        //    archivoRequerimiento.Id = id;
        //    archivoRequerimiento.Nombre = dao.GetNombre(id).Return;
        //    archivoRequerimiento.Extension = resultadoExtension.Return;
        //    archivoRequerimiento.Url = path;
        //    if (File.Exists(path))
        //    {
        //        archivoRequerimiento.Tamaño = new System.IO.FileInfo(path).Length;
        //        resultado.Return = archivoRequerimiento;
        //        return resultado;
        //    }


        //    var consulta = GetResultadoById(id);
        //    if (!consulta.Ok)
        //    {
        //        resultado.AddErrorPublico("Error");
        //        return resultado;
        //    }

        //    if (consulta.Return == null)
        //    {
        //        resultado.Return = null;
        //        return resultado;
        //    }

        //    var documento = Base64ToByte(consulta.Return.Data);
        //    if (documento == null)
        //    {
        //        resultado.Return = null;
        //        return resultado;
        //    }

        //    try
        //    {
        //        if (!System.IO.Directory.Exists(server + "/" + carpeta))
        //        {
        //            System.IO.Directory.CreateDirectory(server + "/" + carpeta);
        //        }

        //        File.WriteAllBytes(path, documento);
        //        resultado.Return = archivoRequerimiento;
        //        return resultado;
        //    }
        //    catch (Exception e)
        //    {
        //        resultado.AddErrorPublico("Error");
        //        return resultado;
        //    }
        //}

        //public Bitmap Base64ToImage(string base64String)
        //{
        //    try
        //    {
        //        byte[] imageBytes = Convert.FromBase64String(base64String.Split(',')[1]);
        //        // Convert byte[] to Image
        //        using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //        {
        //            Image image = Image.FromStream(ms, true);
        //            return new Bitmap(image);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //public byte[] Base64ToByte(string base64String)
        //{
        //    try
        //    {
        //        return Convert.FromBase64String(base64String.Split(',')[1]);
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

        //public Size ResizeKeepAspect(Size CurrentDimensions, int maxWidth, int maxHeight)
        //{
        //    int newHeight = CurrentDimensions.Height;
        //    int newWidth = CurrentDimensions.Width;
        //    if (maxWidth > 0 && newWidth > maxWidth) //WidthResize
        //    {
        //        Decimal divider = Math.Abs((Decimal)newWidth / (Decimal)maxWidth);
        //        newWidth = maxWidth;
        //        newHeight = (int)Math.Round((Decimal)(newHeight / divider));
        //    }
        //    if (maxHeight > 0 && newHeight > maxHeight) //HeightResize
        //    {
        //        Decimal divider = Math.Abs((Decimal)newHeight / (Decimal)maxHeight);
        //        newHeight = maxHeight;
        //        newWidth = (int)Math.Round((Decimal)(newWidth / divider));
        //    }
        //    return new Size(newWidth, newHeight);
        //}

        public Result<bool> Migrar()
        {
            return dao.Migrar();
        }
    }
}

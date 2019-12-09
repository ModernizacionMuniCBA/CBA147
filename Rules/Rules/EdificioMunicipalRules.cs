using System;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Configuration;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using Model.Comandos;

namespace Rules.Rules
{
    public class EdificioMunicipalRules : BaseRules<EdificioMunicipal>
    {

        private readonly EdificioMunicipalDAO dao;

        public EdificioMunicipalRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EdificioMunicipalDAO.Instance;
        }

        public Result<ResultadoTabla_EdificioMunicipal> Insert(Comando_EdificioMunicipal comando)
        {
            var resultado = new Result<ResultadoTabla_EdificioMunicipal>();
            var trans = dao.Transaction(() =>
            {
                var resultCategoria = new CategoriaEdificioMunicipalRules(getUsuarioLogueado()).GetById(comando.IdCategoria);
                if (!resultCategoria.Ok)
                {
                    resultado.Copy(resultCategoria.Errores);
                    return false;
                }

                var edificio = new EdificioMunicipal();
                edificio.Categoria = resultCategoria.Return;
                edificio.Nombre = comando.Nombre;

                //Domicilio
                if (comando.Domicilio != null)
                {
                    var domicilioRules = new DomicilioRules(getUsuarioLogueado());
                    var resultadoDomicilio = domicilioRules.Buscar(comando.Domicilio.Latitud, comando.Domicilio.Longitud);
                    if (!resultadoDomicilio.Ok)
                    {
                        resultado.Copy(resultadoDomicilio.Errores);
                        return false;
                    }

                    if (resultadoDomicilio.Return == null || resultadoDomicilio.Return.Cpc == null || resultadoDomicilio.Return.Barrio == null)
                    {
                        resultado.AddErrorPublico("Domicilio inválido");
                        return false;
                    }


                    Domicilio domicilio = new Domicilio();
                    domicilio.Cpc = new CpcRules(getUsuarioLogueado()).GetByIdCatastro(resultadoDomicilio.Return.Cpc.IdCatastro).Return;
                    if (domicilio.Cpc == null)
                    {
                        resultado.AddErrorPublico("El cpc no existe");
                        return false;
                    }

                    domicilio.Barrio = new BarrioRules(getUsuarioLogueado()).GetByIdCatastro(resultadoDomicilio.Return.Barrio.IdCatastro).Return;
                    if (domicilio.Barrio == null)
                    {
                        resultado.AddErrorPublico("El barrio no existe");
                        return false;
                    }

                    domicilio.Observaciones = comando.Domicilio.Observaciones;

                    if (!string.IsNullOrEmpty(comando.Domicilio.Direccion))
                    {
                        domicilio.Sugerido = false;
                        domicilio.Direccion = comando.Domicilio.Direccion;
                    }
                    else
                    {
                        domicilio.Distancia = resultadoDomicilio.Return.Distancia;
                        domicilio.Sugerido = true;
                        domicilio.Direccion = resultadoDomicilio.Return.Direccion;
                    }

                    if (string.IsNullOrEmpty(domicilio.Direccion))
                    {
                        resultado.AddErrorPublico("Debe indicar la direccion del domicilio");
                        return false;
                    }

                    domicilio.Latitud = ("" + comando.Domicilio.Latitud).Replace(".", ",");
                    domicilio.Longitud = ("" + comando.Domicilio.Longitud).Replace(".", ",");

                    var resultDomicilio = domicilioRules.Insert(domicilio);
                    if (!resultDomicilio.Ok)
                    {
                        resultado.Copy(resultDomicilio.Errores);
                        return false;
                    }
                    edificio.Domicilio = resultDomicilio.Return;
                }

                //Inserto
                var resultadoInsert = base.Insert(edificio);
                if (!resultadoInsert.Ok)
                {
                    resultado.Copy(resultadoInsert.Errores);
                    return false;
                }

                resultado.Return = new ResultadoTabla_EdificioMunicipal(resultadoInsert.Return);
                return true;
            });

            return resultado;
        }


        public Result<ResultadoTabla_EdificioMunicipal> Editar(Comando_EdificioMunicipal comando)
        {
            var resultado = new Result<ResultadoTabla_EdificioMunicipal>();
            var trans = dao.Transaction(() =>
            {
                var resultConsulta= new EdificioMunicipalRules(getUsuarioLogueado()).GetById((int)comando.Id);
                if (!resultConsulta.Ok)
                {
                    resultado.Copy(resultConsulta.Errores);
                    return false;
                }

                var resultCategoria = new CategoriaEdificioMunicipalRules(getUsuarioLogueado()).GetById(comando.IdCategoria);
                if (!resultCategoria.Ok)
                {
                    resultado.Copy(resultCategoria.Errores);
                    return false;
                }

                var edificio = resultConsulta.Return;
                edificio.Categoria = resultCategoria.Return;
                edificio.Nombre = comando.Nombre;

                //Domicilio
                if (comando.Domicilio != null)
                {
                    var domicilioRules = new DomicilioRules(getUsuarioLogueado());
                    var resultadoDomicilio = domicilioRules.Buscar(comando.Domicilio.Latitud, comando.Domicilio.Longitud);
                    if (!resultadoDomicilio.Ok)
                    {
                        resultado.Copy(resultadoDomicilio.Errores);
                        return false;
                    }

                    if (resultadoDomicilio.Return == null || resultadoDomicilio.Return.Cpc == null || resultadoDomicilio.Return.Barrio == null)
                    {
                        resultado.AddErrorPublico("Domicilio inválido");
                        return false;
                    }


                    Domicilio domicilio = edificio.Domicilio;
                    domicilio.Cpc = new CpcRules(getUsuarioLogueado()).GetByIdCatastro(resultadoDomicilio.Return.Cpc.IdCatastro).Return;
                    if (domicilio.Cpc == null)
                    {
                        resultado.AddErrorPublico("El cpc no existe");
                        return false;
                    }

                    domicilio.Barrio = new BarrioRules(getUsuarioLogueado()).GetByIdCatastro(resultadoDomicilio.Return.Barrio.IdCatastro).Return;
                    if (domicilio.Barrio == null)
                    {
                        resultado.AddErrorPublico("El barrio no existe");
                        return false;
                    }

                    domicilio.Observaciones = comando.Domicilio.Observaciones;

                    if (!string.IsNullOrEmpty(comando.Domicilio.Direccion))
                    {
                        domicilio.Sugerido = false;
                        domicilio.Direccion = comando.Domicilio.Direccion;
                    }
                    else
                    {
                        domicilio.Distancia = resultadoDomicilio.Return.Distancia;
                        domicilio.Sugerido = true;
                        domicilio.Direccion = resultadoDomicilio.Return.Direccion;
                    }

                    if (string.IsNullOrEmpty(domicilio.Direccion))
                    {
                        resultado.AddErrorPublico("Debe indicar la direccion del domicilio");
                        return false;
                    }

                    domicilio.Latitud = ("" + comando.Domicilio.Latitud).Replace(".", ",");
                    domicilio.Longitud = ("" + comando.Domicilio.Longitud).Replace(".", ",");

                    var resultDomicilio = domicilioRules.Update(domicilio);
                    if (!resultDomicilio.Ok)
                    {
                        resultado.Copy(resultDomicilio.Errores);
                        return false;
                    }
                    edificio.Domicilio = resultDomicilio.Return;
                }

                //Inserto
                var resultadoInsert = base.Update(edificio);
                if (!resultadoInsert.Ok)
                {
                    resultado.Copy(resultadoInsert.Errores);
                    return false;
                }

                resultado.Return = new ResultadoTabla_EdificioMunicipal(resultadoInsert.Return);
                return true;
            });

            return resultado;
        }

        public Result<ResultadoTabla_EdificioMunicipal> DarDeBaja(int id)
        {
            var resultado = new Result<ResultadoTabla_EdificioMunicipal>();
            var result = DeleteById(id);
            if (!result.Ok)
            {
                resultado.Copy(result.Errores);
                return resultado;
            }

            resultado.Return = new ResultadoTabla_EdificioMunicipal(result.Return);
            return resultado;
        }

        public Result<ResultadoTabla<ResultadoTabla_EdificioMunicipal>> GetResultadoTabla(int idCategoria)
        {
            var resultado = new Result<ResultadoTabla<ResultadoTabla_EdificioMunicipal>>();
            var result = dao.GetByIdCategoria(idCategoria);
            if (!result.Ok)
            {
                resultado.Copy(result.Errores);
                return resultado;
            }

            resultado.Return = new ResultadoTabla<ResultadoTabla_EdificioMunicipal>();
            resultado.Return.Data = ResultadoTabla_EdificioMunicipal.ToList(result.Return);
            return resultado;
        }
        
        public Result<ResultadoTabla_EdificioMunicipal> GetResultadoTablaById(int id)
        {
            var resultado = new Result<ResultadoTabla_EdificioMunicipal>();
            var result = dao.GetById(id);
            if (!result.Ok)
            {
                resultado.Copy(result.Errores);
                return resultado;
            }

            resultado.Return= new ResultadoTabla_EdificioMunicipal(result.Return);
            return resultado;
        }

        public Result<Resultado_Domicilio> GetDomicilioById(int id)
        {
            var resultado = new Result<Resultado_Domicilio>();
            var result = dao.GetById(id);
            if (!result.Ok)
            {
                resultado.Copy(result.Errores);
                return resultado;
            }

            resultado.Return= new Resultado_Domicilio(result.Return.Domicilio);
            resultado.Return.Nombre = result.Return.Nombre;
            resultado.Return.Observaciones =  result.Return.Observaciones;
            return resultado;
        }
    }
}

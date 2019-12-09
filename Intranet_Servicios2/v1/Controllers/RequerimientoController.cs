using Intranet_Servicios2.Utils.Controllers;
using Intranet_Servicios2.Utils.Controllers.ActionFilters;
using Intranet_Servicios2.v1.Entities.Consultas;
using Model.Consultas;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GeoJSON.Net.Geometry;
using System.Web.Http.Description;
using Intranet_Servicios2.v1.Entities.Resultados;
using GeoJSON.Net.Feature;
using Intranet_Servicios2.v1.Entities.Comandos;

namespace Intranet_Servicios2.v1.Controllers
{
    public class Requerimiento_v1Controller : Control
    {
        const string routeBase = "v1/Requerimiento";

        [HttpPost]
        [Route(routeBase)]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_RequerimientoInsertado> Insertar(string token, v1.Entities.Comandos.ComandoApp_Requerimiento comando)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_RequerimientoInsertado>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).Insertar(comando);
        }

        [HttpGet]
        [Route(routeBase)]
        public ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_RequerimientoListado>> GetMisRequerimientos(string token)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<List<v1.Entities.Resultados.ResultadoApp_RequerimientoListado>>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).GetMisRequerimientos();
        }

        [HttpGet]
        [Route(routeBase + "/Detalle")]
        public ResultadoServicio<v1.Entities.Resultados.ResultadoApp_RequerimientoDetalle> GetDetalle(string token, int id)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<v1.Entities.Resultados.ResultadoApp_RequerimientoDetalle>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).GetDetalle(id);
        }

        [HttpGet]
        [Route(routeBase + "/EnviarEmailComprobante")]
        public ResultadoServicio<bool?> EnviarEmailComprobante(string token, int idRequerimiento)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool?>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).EnviarEmailComprobante(idRequerimiento);
        }

        [HttpGet]
        [Route(routeBase + "/Cancelar")]
        public ResultadoServicio<bool?> Cancelar(string token, int idRequerimiento)
        {
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool?>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).Cancelar(idRequerimiento);
        }


        [HttpPut]
        [ConToken]
        [ConClavePanel]
        [Route(routeBase + "/GetRequerimientos")]
        public ResultadoServicio<MultiPoint> GetPuntosRequerimientos(ConsultaApp_Requerimiento consulta)
        {
            string token = Request.Headers.GetValues("Token").First();
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<MultiPoint>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).GetPuntosRequerimientos(new Consulta_Requerimiento()
            {
                IdsArea = consulta.IdsArea,
                IdsMotivo = consulta.IdsMotivo,
                IdsServicio = consulta.IdsServicio,
                EstadosKeyValue = consulta.KeyValuesEstado,
                FechaDesde = consulta.FechaDesde,
                FechaHasta = consulta.FechaHasta

            });
        }

        [HttpPut]
        [ConToken]
        [ConClavePanel]
        [Route(routeBase + "/PuntosRequerimientos")]
        public ResultadoServicio<List<Feature>> GetPuntosRequerimientosPanel(ConsultaApp_Requerimiento consulta)
        {
            string token = Request.Headers.GetValues("Token").First();
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<List<Feature>>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).GetPuntosRequerimientosPanel(new Consulta_Requerimiento()
            {
                IdsArea = consulta.IdsArea,
                IdsMotivo = consulta.IdsMotivo,
                IdsServicio = consulta.IdsServicio,
                EstadosKeyValue = consulta.KeyValuesEstado,
                FechaDesde = consulta.FechaDesde,
                FechaHasta = consulta.FechaHasta

            });
        }

        [HttpPut]
        [ConToken]
        [ConClavePanel]
        [Route(routeBase + "/GetDetalleExternoById")]
        public ResultadoServicio<ResultadoExterno_Requerimiento> GetDetalleExternoById(int id)
        {
            string token = Request.Headers.GetValues("Token").First();
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<ResultadoExterno_Requerimiento>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).GetDetalleExternoById(id);
        }

        [HttpPut]
        [ConToken]
        [Route(routeBase + "/GetRequerimientosByFilters")]
        public ResultadoServicio<List<ResultadoExterno_Requerimiento>> GetRequerimientosByFilters(ConsultaApp_Requerimiento consulta)
        {
            string token = Request.Headers.GetValues("Token").First();
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<List<ResultadoExterno_Requerimiento>>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).GetRequerimientosByFilters(consulta);
        }

        [HttpPut]
        [ConToken]
        [Route(routeBase + "/CambiarEstadoRequerimiento")]
        public ResultadoServicio<bool> CambiarEstadoRequerimiento(ComandoExterno_RequerimientoCambiarEstado comando)
        {
            string token = Request.Headers.GetValues("Token").First();
            var resultadoUser = GetUsuarioLogeado(token);
            if (!resultadoUser.Ok)
            {
                var resultado = new ResultadoServicio<bool>();
                resultado.Error = resultadoUser.Error;
                return resultado;
            }

            return new v1.MisRules.WSRules_Requerimiento(resultadoUser.Return).CambiarEstadoRequerimiento(comando);
        }
    }
}
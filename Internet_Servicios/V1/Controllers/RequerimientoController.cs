using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Internet_Servicios.Utils;
using Internet_Servicios.V1.Entities.Resultados;
using Internet_Servicios.V1.Entities.Comandos;
using GeoJSON.Net.Geometry;
using Internet_Servicios2.v1.Entities.Consultas;
using GeoJSON.Net.Feature;

namespace Internet_Servicios.V1.Controllers
{
    [RoutePrefix("v1/Requerimiento")]
    public class Requerimiento_v1Controller : ApiController
    {
        [HttpPost]
        [Route("")]
        public ResultServicio<ResultadoApp_RequerimientoInsertado> Insertar(string token, ComandoApp_Requerimiento comando)
        {
            return RestCall.Call<ResultadoApp_RequerimientoInsertado>(Request, comando);
        }

        [HttpGet]
        [Route("")]
        public ResultServicio<List<ResultadoApp_RequerimientoListado>> GetMisRequerimientos(string token)
        {
            return RestCall.Call<List<ResultadoApp_RequerimientoListado>>(Request);
        }

        [HttpGet]
        [Route("Detalle")]
        public ResultServicio<ResultadoApp_RequerimientoDetalle> GetDetalle(string token, int id)
        {
            return RestCall.Call<ResultadoApp_RequerimientoDetalle>(Request);
        }

        [HttpGet]
        [Route("EnviarEmailComprobante")]
        public ResultServicio<bool?> EnviarEmailComprobante(string token, int idRequerimiento)
        {
            return RestCall.Call<bool?>(Request);
        }

        [HttpGet]
        [Route("Cancelar")]
        public ResultServicio<bool?> Cancelar(string token, int idRequerimiento)
        {
            return RestCall.Call<bool?>(Request);
        }

        [HttpPut]
        [Route("GetRequerimientos")]
        public ResultServicio<MultiPoint> GetRequerimientos(ConsultaApp_Requerimiento consulta)
        {
            return RestCall.Call<MultiPoint>(Request, consulta);
        }

        [HttpPut]
        [Route("PuntosRequerimientos")]
        public ResultServicio<List<Feature>> GetRequerimientosPuntosPanel(ConsultaApp_Requerimiento consulta)
        {
            return RestCall.Call<List<Feature>>(Request, consulta);
        }

        [HttpPut]
        [Route("GetDetalleExternoById")]
        public ResultServicio<ResultadoExterno_Requerimiento> DetalleExternoExternoById(int id)
        {
            return RestCall.Call<ResultadoExterno_Requerimiento>(Request);
        }

        [HttpPut]
        [Route("GetRequerimientosByFilters")]
        public ResultServicio<List<ResultadoExterno_Requerimiento>> GetRequerimientosByFilters(ConsultaApp_Requerimiento consulta)
        {
            return RestCall.Call<List<ResultadoExterno_Requerimiento>>(Request, consulta);
        }

        [HttpPut]
        [Route("CambiarEstadoRequerimiento")]
        public ResultServicio<bool> CambiarEstadoRequerimiento(ComandoExterno_RequerimientoCambiarEstado comando)
        {
            return RestCall.Call<bool>(Request, comando);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using Model;
using Model.Entities;
using Rules.Rules.Reportes;
using Model.Resultados;
using Intranet_Servicios2;
using Rules.Rules;
using Rules;
using Intranet_Servicios2.MisRules;
using Intranet_Servicios2.Utils.MisRules;
using Intranet_Servicios2.v1.Entities.Resultados;
using Intranet_Servicios2.v1.Entities.Comandos;
using Intranet_Servicios2.Utils.Entities.Comando;
using Model.Consultas;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Feature;
using Intranet_Servicios2.v1.Entities.Consultas;


namespace Intranet_Servicios2.v1.MisRules
{
    public class WSRules_Requerimiento : _WSRules_Base<Requerimiento>
    {
        private readonly RequerimientoRules rules;
        private readonly _WSRules_BaseRequerimiento rulesBase;

        public WSRules_Requerimiento(UsuarioLogueado data)
            : base(data)
        {
            rules = new RequerimientoRules(data);
            rulesBase = new _WSRules_BaseRequerimiento(data);
        }

        public ResultadoServicio<List<ResultadoApp_RequerimientoListado>> GetMisRequerimientos()
        {
            var resultado = new ResultadoServicio<List<ResultadoApp_RequerimientoListado>>();

            try
            {
                var resultadoConsulta = rules.ProcedimientoAlmacenado<ResultadoApp_RequerimientoListado>("WS_App_RequerimientoListado_v1 @idUsuarioReferente=" + getUsuarioLogueado().Usuario.Id);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Error = resultadoConsulta.Errores.ToStringPublico();
                    return resultado;
                }



                resultado.Return = resultadoConsulta.Return.OrderByDescending(x => x.FechaAlta).ToList();

            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
            }

            return resultado;
        }

        public ResultadoServicio<ResultadoApp_RequerimientoDetalle> GetDetalle(int id)
        {
            var resultado = new ResultadoServicio<ResultadoApp_RequerimientoDetalle>();

            try
            {

                //Valido que pueda manipular
                var resultadoPuedeManipular = rulesBase.PuedeManipularRequerimiento(getUsuarioLogueado().Usuario.Id, id);
                if (!resultadoPuedeManipular.Ok)
                {
                    resultado.Error = resultadoPuedeManipular.Error;
                    return resultado;
                }

                if (!resultadoPuedeManipular.Return)
                {
                    resultado.Error = "No tiene permisos para consultar el requerimiento indicado";
                    return resultado;
                }

                //Detalle
                var resultadoConsulta = rules.ProcedimientoAlmacenado<ResultadoApp_RequerimientoDetalle>("WS_App_RequerimientoDetalle_v1 @id=" + id + ", @idUsuarioReferente=" + getUsuarioLogueado().Usuario.Id);
                if (!resultadoConsulta.Ok)
                {
                    resultado.Error = resultadoConsulta.Errores.ToStringPublico();
                    return resultado;
                }

                if (resultadoConsulta.Return == null || resultadoConsulta.Return.Count == 0)
                {
                    resultado.Error = "El requerimiento no existe";
                    return resultado;
                }

                resultado.Return = resultadoConsulta.Return[0];

                //Fotos
                var resultadoConsultaFotos = rules.ProcedimientoAlmacenado<string>("WS_App_RequerimientoDetalle_v1_Fotos @id=" + id + ", @idUsuarioReferente=" + getUsuarioLogueado().Usuario.Id);
                if (!resultadoConsultaFotos.Ok)
                {
                    resultado.Error = resultadoConsultaFotos.Errores.ToStringPublico();
                    return resultado;
                }

                resultado.Return.Fotos = resultadoConsultaFotos.Return;

            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
            }

            return resultado;
        }

        public ResultadoServicio<ResultadoApp_RequerimientoInsertado> Insertar(ComandoApp_Requerimiento comando)
        {
            var resultado = new ResultadoServicio<ResultadoApp_RequerimientoInsertado>();

            try
            {
                var resultadoConsulta = rulesBase.Insertar(new ComandoAppBase_Requerimiento()
                {
                    Autenticacion = new ComandoAppBase_RequerimientoAutenticacion()
                    {
                        ReCaptcha = comando.Autenticacion.ReCaptcha,
                        KeyValidacionReCaptcha = comando.Autenticacion.KeyValidacionReCaptcha,
                        OrigenAlias = comando.Autenticacion.OrigenAlias,
                        OrigenKey = comando.Autenticacion.OrigenKey
                    },
                    IdMotivo = comando.IdMotivo,
                    Descripcion = comando.Descripcion,
                    Domicilio = new ComandoAppBase_Domicilio()
                    {
                        Direccion = comando.Domicilio.Direccion,
                        Observaciones = comando.Domicilio.Observaciones,
                        Latitud = comando.Domicilio.Latitud,
                        Longitud = comando.Domicilio.Longitud
                    },
                    Imagen = comando.Imagen
                });

                if (!resultadoConsulta.Ok)
                {
                    resultado.Error = resultadoConsulta.Error;
                    return resultado;
                }

                resultado.Return = new ResultadoApp_RequerimientoInsertado(resultadoConsulta.Return);
            }
            catch (Exception e)
            {
                resultado.Error = "Error procesando la solicitud";
            }

            return resultado;
        }

        public ResultadoServicio<bool?> EnviarEmailComprobante(int idRequerimiento)
        {
            return rulesBase.EnviarEmailComprobante(idRequerimiento);
        }

        public ResultadoServicio<bool?> Cancelar(int idRequerimiento)
        {
            return rulesBase.Cancelar(idRequerimiento);
        }

        public ResultadoServicio<MultiPoint> GetPuntosRequerimientos(Consulta_Requerimiento consulta)
        {
            var resultado = rules.GetIdsByFilters(consulta, null);
            var resultadoPuntos = rules.GetMarcadoresGoogleMaps(resultado.Return.ToList());
            var points = new List<Point>();

            foreach (Resultado_MarcadorGoogleMaps marcador in resultadoPuntos.Return)
            {
                Position position = new Position(Double.Parse(marcador.Latitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture), Double.Parse(marcador.Longitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture));
                Point point = new Point(position);
                points.Add(point);
            }


            var multiPoint = new MultiPoint(points);

            var result = new ResultadoServicio<MultiPoint>();
            //result.Return = JsonConvert.SerializeObject(multiPoint);
            result.Return = multiPoint;
            return result;
        }

        public ResultadoServicio<List<Feature>> GetPuntosRequerimientosPanel(Consulta_Requerimiento consulta)
        {
            var resultado = rules.GetIdsByFilters(consulta, null);
            var resultadoPuntos = rules.GetMarcadoresGoogleMaps(resultado.Return.ToList());
            var features = new List<Feature>();

            foreach (Resultado_MarcadorGoogleMaps marcador in resultadoPuntos.Return)
            {
                Position position = new Position(Double.Parse(marcador.Latitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture), Double.Parse(marcador.Longitud.Trim().Replace(',', '.'), CultureInfo.InvariantCulture));
                Point point = new Point(position);
                var featureProperties = new Dictionary<string, object> { { "Id", marcador.Id } };
                var model = new Feature(point, featureProperties);
                features.Add(model);
            }


            var result = new ResultadoServicio<List<Feature>>();
            //result.Return = JsonConvert.SerializeObject(multiPoint);
            result.Return = features;
            return result;
        }

        public ResultadoServicio<ResultadoExterno_Requerimiento> GetDetalleExternoById(int id)
        {
            var result = new ResultadoServicio<ResultadoExterno_Requerimiento>();

            var ids = new List<int>();
            ids.Add(id);

            var resultado = rules.GetResultadoExternoByIds(ids);
            if (!resultado.Ok || resultado.Return.Data.Count == 0)
            {
                resultado.AddErrorPublico("Error consultando el requerimiento");
                return result;
            }

            result.Return = new ResultadoExterno_Requerimiento(resultado.Return.Data[0]);
            return result;
        }

        public ResultadoServicio<List<ResultadoExterno_Requerimiento>> GetRequerimientosByFilters(ConsultaApp_Requerimiento consulta)
        {
            var result = new ResultadoServicio<List<ResultadoExterno_Requerimiento>>();

            try
            {
                var resultAreas = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetIdsAreasByIdUsuario(getUsuarioLogueado().Usuario.Id);
                if (!resultAreas.Ok)
                {
                    result.Error = resultAreas.Error;
                    return result;
                }

                //creo la consulta
                var consultaRules = new Consulta_Requerimiento();
                consultaRules.IdsArea = resultAreas.Return;

                var estadosKeyValue = new List<Enums.EstadoRequerimiento>();
                foreach (int entero in consulta.KeyValuesEstado)
                {
                    if (!Enum.IsDefined(typeof(Enums.EstadoRequerimiento), entero))
                    {
                        result.Error = "El número " + entero + " no es un key value correcto de un estado";
                        return result;
                    }

                    estadosKeyValue.Add((Enums.EstadoRequerimiento)entero);
                }

                consultaRules.EstadosKeyValue = estadosKeyValue;
                if (consulta.FechaDesde.HasValue)
                {
                    consultaRules.FechaDesde = consulta.FechaDesde;
                    consultaRules.FechaHasta = DateTime.Now;
                }

                var resultado = rules.GetResultadoExternoByFilters(consultaRules, null);

                if (!resultado.Ok)
                {
                    resultado.AddErrorPublico("Error consultando los requerimientos");
                }

                result.Return = ResultadoExterno_Requerimiento.ToList(resultado.Return.Data);
            }
            catch (Exception e)
            {
                MiLog_WS.Info(e);
                result.Error = "Error procesando la solicitud";
            }
            return result;
        }


        internal ResultadoServicio<bool> CambiarEstadoRequerimiento(ComandoExterno_RequerimientoCambiarEstado comando)
        {
            var result = new ResultadoServicio<bool>();

            try
            {
                var resultRequerimiento = GetDetalleExternoById(comando.IdRequerimiento);
                if (!resultRequerimiento.Ok)
                {
                    result.Error = resultRequerimiento.Error;
                    return result;
                }

                var resultAreas = new _VecinoVirtualUsuarioRules(getUsuarioLogueado()).GetIdsAreasByIdUsuario(getUsuarioLogueado().Usuario.Id);
                if (!resultAreas.Ok)
                {
                    result.Error = resultAreas.Error;
                    return result;
                }


                if (!resultAreas.Return.Contains(resultRequerimiento.Return.AreaId))
                {
                    result.Error = "Usted no tiene permiso para editar el estado de éste requerimiento";
                    return result;
                }

                var resultado = rules.CambiarEstado(comando.IdRequerimiento, (Enums.EstadoRequerimiento)comando.KeyValueEstado, comando.Descripcion);
                if (!resultado.Ok)
                {
                    result.Error = resultado.Error;
                    return result;
                }

                result.Return = true;
            }
            catch (Exception e)
            {
                MiLog_WS.Info(e);
                result.Error = "Error procesando la solicitud";
            }
            return result;
        }
    }
}

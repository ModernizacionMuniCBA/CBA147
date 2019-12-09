using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using Model.Entities;
using Model;
using Model.Resultados;
using System.Web;
using Model.Consultas;
using Model.Resultados.Estadisticas;

namespace Rules.Rules
{
    public class EstadisticaRules : BaseRules<BaseEntity>
    {

        private readonly EstadisticaDAO dao;

        public EstadisticaRules(UsuarioLogueado data)
            : base(data)
        {
            dao = EstadisticaDAO.Instance;
        }

        public Result<List<object[]>> Estadistica_Requerimiento_CPC(bool? relevamientoInterno, List<Enums.EstadoRequerimiento> estadosRequerimiento, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return dao.Estadistica_Requerimiento_CPC(relevamientoInterno, estadosRequerimiento, fechaDesde, fechaHasta);
        }

        //public Result<Resultado_DatosEstadisticaPanel> GetDatosEstadisticaYMapa(DateTime fechaDesde, DateTime fechaHasta)
        //{
        //    var result = new Result<Resultado_DatosEstadisticaPanel>();
        //    var resultado = new Resultado_DatosEstadisticaPanel();

        //    //Result para las estadisticas
        //    var resultData = GetDatosEstadistica(fechaDesde, fechaHasta);
        //    if (!resultData.Ok)
        //    {
        //        result.AddErrorPublico(resultData.Errores.ToStringPublico());
        //        return result;
        //    }
        //    resultado = resultData.Return;

        //    //Result para la criticidad por servicio
        //    var resultData6 = GetMapaCritico(Enums.TipoRequerimiento.RECLAMO, fechaDesde, fechaHasta);
        //    if (!resultData6.Ok)
        //    {
        //        result.AddErrorPublico(resultData6.Errores.ToStringPublico());
        //        return result;
        //    }
        //    resultado.ArrayDatosMapa = resultData6.Return;

        //    result.Return = resultado;
        //    return result;
        //}

        public Result<Resultado_DatosEstadisticaPanel> GetDatosEstadistica(DateTime fechaDesde, DateTime fechaHasta)
        {
            var result = new Result<Resultado_DatosEstadisticaPanel>();
            var resultado = new Resultado_DatosEstadisticaPanel();

            //Result par el ranking de motivos
            var resultData = GetDatosEstadisticaPanel_Motivos(fechaDesde, fechaHasta);
            if (!resultData.Ok)
            {
                result.AddErrorPublico(resultData.Errores.ToStringPublico());
                return result;
            }
            resultado.Ranking_Motivos = resultData.Return;

            //Result para los Totales (Indice de atencion)
            var resultData2 = GetDatosEstadisticaPanel_Totales(fechaDesde, fechaHasta);
            if (!resultData2.Ok)
            {
                result.AddErrorPublico(resultData2.Errores.ToStringPublico());
                return result;
            }
            resultado.Totales = resultData2.Return;

            //Result para el grafico radial
            var resultData3 = GetDatosEstadisticaPanel_Radial(fechaDesde, fechaHasta);
            if (!resultData3.Ok)
            {
                result.AddErrorPublico(resultData3.Errores.ToStringPublico());
                return result;
            }
            resultado.Radial = resultData3.Return;

            //Result para el promedio de atencion
            var resultData4 = GetDatosEstadisticaPanel_Radial_Promedio(fechaDesde, fechaHasta);
            if (!resultData4.Ok)
            {
                result.AddErrorPublico(resultData4.Errores.ToStringPublico());
                return result;
            }
            resultado.PromedioAtencion = resultData4.Return;

            //Result para la criticidad por servicio
            var resultData5 = GetDatosEstadisticaPanel_CriticiidadServicio(fechaDesde, fechaHasta);
            if (!resultData5.Ok)
            {
                result.AddErrorPublico(resultData5.Errores.ToStringPublico());
                return result;
            }
            resultado.CriticidadServicios = resultData5.Return;

            result.Return = resultado;

            return result;
        }

        #region EstadisticasV2
        public Result<List<Resultado_DatosEstadisticaPanel_Cpc>> GetDatosEstadisticaCpc(Consulta_EstadisticaCPC consulta)
        {

            var consultaRequerimiento = new Consulta_Requerimiento();

            consultaRequerimiento.EstadosKeyValue = consulta.EstadosKeyValue;
            consultaRequerimiento.RelevamientoDeOficio = consulta.RelevamientoDeOficio;
            consultaRequerimiento.FechaDesde = consulta.FechaDesde;
            consultaRequerimiento.FechaHasta = consulta.FechaHasta;
            
            if (consulta.Año != null && consulta.Año != -1)
            {
                consultaRequerimiento.AñoDeMes = consulta.Año;
                
            }
            if (consulta.Mes != null && consulta.Año != -1)
            {
                consultaRequerimiento.Mes = consulta.Mes;
            }

            List<int> idsAreas = new List<int>();
            var areas = consulta.Areas;
            foreach (var area in areas)
            {
                if (area.IdsHijos == null)
                {
                    idsAreas.Add(area.Id.Value);
                }
                else
                {
                    idsAreas.Add(area.Id.Value);
                    var subareas = area.IdsHijos;
                    foreach (int idsHijas in subareas)
                    {
                        idsAreas.Add(idsHijas);
                    }
                }

            }
            consultaRequerimiento.IdsArea = idsAreas;
      

           
            
           


           // var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());
            var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());
            var result = new Result<List<Resultado_DatosEstadisticaPanel_Cpc>>();

            //Total
           // var ids = requerimientoRules.GetIdsConDomicilio(Enums.TipoRequerimiento.RECLAMO, consulta.EstadosKeyValue, true, null, consulta.FechaDesde, consulta.FechaHasta, null, false).Return;
            //var total = ids.Count();
            //Mandar con null los marcadas para que me busque todos 
            var ids = requerimientoRules.GetIdsByFilters(consultaRequerimiento, null);
            var total = ids.Return.Count();
            if (total == 0)
            {
                result.AddErrorPublico("No hay requerimientos que cumplan con los filtros de busqueda");
                return result;
            }

            //CPCs
            var returnCpcs = new CpcRules(getUsuarioLogueado()).GetAll(false);
            if (!returnCpcs.Ok)
            {
                result.AddErrorPublico("Error obteniendo los CPCs.");
                return result;
            }
            var cpcs = returnCpcs.Return.FindAll(x => x.IdCatastro != 0).ToList();
            


            var listaCpcs = new List<Resultado_DatosEstadisticaPanel_Cpc>();

            foreach (Cpc cpc in cpcs)
            {
                //Ids de los reclamos con este IdCpc                
                //var returnIdsCpc = requerimientoRules.GetIds(Enums.TipoRequerimiento.RECLAMO, consulta.EstadosKeyValue, null, null, null, null, null, null, null, null, null, cpc.Id, null, consulta.FechaDesde, consulta.FechaHasta, false, null, false, null, null);

                //Creo una lista de cpc para poder enviarlo
                var listIntCpc = new List<int>();
                listIntCpc.Add(cpc.Numero);
                consultaRequerimiento.KeyValuesCPC = listIntCpc;

                var returnIdsCpc = requerimientoRules.GetIdsByFilters(consultaRequerimiento, null);
                if (!returnIdsCpc.Ok)
                {
                    result.AddErrorPublico("Error generando el mapa");
                    return result;
                }

                var cantidadCpc = returnIdsCpc.Return.Count();
                if (cantidadCpc != 0)
                {
                    var resultadoCpc = new Resultado_DatosEstadisticaPanel_Cpc();
                    resultadoCpc.Cpc = new Resultado_Cpc(cpc);
                    resultadoCpc.IdsRequerimientos = returnIdsCpc.Return;
                    resultadoCpc.CantidadRequerimientos = cantidadCpc;
                    resultadoCpc.Porcentaje = Math.Round((((float)cantidadCpc) / total) * 100, 2);
                    listaCpcs.Add(resultadoCpc);
                }
            }
          
            result.Return = listaCpcs;
            return result;      
        }
        public Result<List<Resultado_DatosEstadisticaUsuario>> GetDatosEstadisticaUsuario(Consulta_EstadisticaUsuario consulta)
        {
            return dao.GetDatosEstadisticaUsuario(consulta);
        }

        public Result<List<Resultado_DatosEstadisticaOrigen>> GetDatosEstadisticaOrigen(Consulta_EstadisticaOrigen consulta)
        {
            return dao.GetDatosEstadisticaOrigen(consulta);
            
            //if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            //{
            //    return dao.GetDatosEstadisticaOrigen(consulta.FechaDesde, consulta.FechaHasta);
            //}
            //else{
            //    return dao.GetDatosEstadisticaOrigen(consulta.Mes, consulta.Año);
            //}
        }

        public Result<List<Resultado_DatosEstadisticaEficacia>> GetDatosEstadisticaEficacia(Consulta_EstadisticaEficacia consulta)
        {
            return dao.GetDatosEstadisticaEficacia(consulta);
            //if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            //{
            //    return dao.GetDatosEstadisticaEficacia(consulta.FechaDesde, consulta.FechaHasta, consulta.IdsArea);
            //}
            //else
            //{
            //    return dao.GetDatosEstadisticaEficacia(consulta.Mes, consulta.Año, consulta.IdsArea);
            //}


        }
        public Result<List<Resultado_DatosEstadisticaResueltos>> GetDatosEstadisticaResueltos(Consulta_EstadisticaResueltos consulta)

        {
            return dao.GetDatosEstadisticaResueltos(consulta);
        }
        public Result<List<Resultado_DatosEstadisticaArea>> GetDatosEstadisticaArea(Consulta_EstadisticaArea consulta)
        {
            return dao.GetDatosEstadisticaArea(consulta);
        }

        public Result<List<Resultado_DatosEstadisticaArea>> GetDatosEstadisticaSubArea(Consulta_EstadisticaSubArea consulta)
        {
            return dao.GetDatosEstadisticaSubArea(consulta);
        }
        public Result<List<Resultado_DatosEstadisticaServicios>> GetDatosEstadisticaServicios(Consulta_EstadisticaServicios consulta)
        {
            return dao.GetDatosEstadisticaServicios(consulta);
        }
        public Result<List<Resultado_DatosEstadisticaZona>> GetDatosEstadisticaZona(Consulta_EstadisticaZona consulta)
        {
            var result = new Result<List<Resultado_DatosEstadisticaZona>>();

            /*Primero consulta si el area tiene zonas creadas*/
            var zonaRules = new ZonaRules(getUsuarioLogueado());
            var listaIdsAreas = new List<int>();
            listaIdsAreas.Add(consulta.IdArea);

            var zonaConsulta = new Consulta_Zona();
            zonaConsulta.IdsArea = listaIdsAreas;

            var resultZona = zonaRules.GetByFilters(zonaConsulta);
            if (!resultZona.Ok)
            {
                result.AddErrorPublico("Error buscando la zona");
                return result;
            }


            if (resultZona.Return.Count == 0)
            {
                result.AddErrorInterno("El area seleccionada no posee zonas");
                return result;
            }
            
            return dao.GetDatosEstadisticaZona(consulta);
        }

        public Result<List<Resultado_DatosEstadisticaMotivos>> GetDatosEstadisticaMotivos(Consulta_EstadisticaMotivos consulta)
        {
            return dao.GetDatosEstadisticaMotivos(consulta);
        }
        
        public Result<List<Resultado_DatosEstadisticaRubros>> GetDatosEstadisticaRubros(Consulta_EstadisticaRubros consulta)
        {
            var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());
            var result = new Result<List<Resultado_DatosEstadisticaRubros>>();

            var consultaRequerimiento = new Consulta_Requerimiento();

            consultaRequerimiento.FechaDesde = consulta.FechaDesde;
            consultaRequerimiento.FechaHasta = consulta.FechaHasta;


            if (consulta.Año != null && consulta.Año != -1)
            {
                consultaRequerimiento.AñoDeMes = consulta.Año;

            }
            if (consulta.Mes != null && consulta.Año != -1)
            {
                consultaRequerimiento.Mes = consulta.Mes;
            }

            //Motivos en grupo
            var resultadoMotivosGrupo = new MotivoPorRubroMotivoRules(getUsuarioLogueado()).GetByIdGrupo(consulta.IdGrupo);
            if (!resultadoMotivosGrupo.Ok)
            {
                result.AddErrorPublico("Error");
                return result;
            }

            List<int> idsMotivos = new List<int>();
            idsMotivos.AddRange(resultadoMotivosGrupo.Return.Select(z => z.Motivo.Id).ToList());

            consultaRequerimiento.IdsMotivo = idsMotivos;

            //Total

            //Mandar con null los marcadas para que me busque todos 
            var ids = requerimientoRules.GetIdsByFilters(consultaRequerimiento, null);
            var total = ids.Return.Count();
            if (total == 0)
            {
                result.AddErrorPublico("No hay requerimientos que cumplan con los filtros de busqueda");
                return result;
            }

          



            var listaCpcs = new List<Resultado_DatosEstadisticaRubros>();

            var rubrosPorGrupo = new RubroMotivoRules(getUsuarioLogueado()).GetRubrosByIdGrupo(consulta.IdGrupo);
            if (!rubrosPorGrupo.Ok)
            {
                result.AddErrorPublico("Error");
                return result;
            }
            var listaRubros = rubrosPorGrupo.Return;

            foreach (var rubro in listaRubros)
            {
                //Ids de los reclamos con este IdCpc                
                //var returnIdsCpc = requerimientoRules.GetIds(Enums.TipoRequerimiento.RECLAMO, consulta.EstadosKeyValue, null, null, null, null, null, null, null, null, null, cpc.Id, null, consulta.FechaDesde, consulta.FechaHasta, false, null, false, null, null);

                //Creo una lista de cpc para poder enviarlo
               

                /*Piso los motivosp articulares de este rubro*/
                var listMotivos = new List<int>();
                listMotivos.AddRange(rubro.Motivos.Select(z => z.Motivo.Id).ToList());
                consultaRequerimiento.IdsMotivo = listMotivos;

                var returnIdsRubro = requerimientoRules.GetIdsByFilters(consultaRequerimiento, null);
                if (!returnIdsRubro.Ok)
                {
                    result.AddErrorPublico("Error generando el mapa");
                    return result;
                }

                var cantidadRubro = returnIdsRubro.Return.Count();
                if (cantidadRubro != 0)
                {
                    var resultadoRubro = new Resultado_DatosEstadisticaRubros();

                    resultadoRubro.IdsRequerimientos = returnIdsRubro.Return;
                    resultadoRubro.Cantidad = cantidadRubro;
                    resultadoRubro.Porcentaje = Math.Round((((float)cantidadRubro) / total) * 100, 2);
                    resultadoRubro.Rubro = rubro.Nombre;
                    resultadoRubro.IdRubro = rubro.Id;                    
                    listaCpcs.Add(resultadoRubro);
                }
            }

            var listaOrdenada = listaCpcs.OrderByDescending(x => x.Porcentaje).ToList();

            result.Return = listaOrdenada;
            return result;
        }       
        #endregion

        #region Mapa
        /*Se comento para ver a donde se rompe , tiene la forma vieja de obtener los datos. Si se necesita usar pasara todo a resultado y consulta*/
        //public Result<List<Resultado_DatosEstadisticaPanel_Cpc>> GetMapaCritico(Enums.TipoRequerimiento? tipo, DateTime? fechaDesde, DateTime? fechaHasta)
        //{
        //    var estados = new List<Enums.EstadoRequerimiento>();
        //    estados.Add(Enums.EstadoRequerimiento.NUEVO);
        //    estados.Add(Enums.EstadoRequerimiento.PENDIENTE);
        //    estados.Add(Enums.EstadoRequerimiento.INSPECCION);
        //    estados.Add(Enums.EstadoRequerimiento.ENPROCESO);

        //    var requerimientoRules = new RequerimientoRules(getUsuarioLogueado());
        //    var result = new Result<List<Resultado_DatosEstadisticaPanel_Cpc>>();
        //    //Total
        //    var ids = requerimientoRules.GetIdsConDomicilio(Enums.TipoRequerimiento.RECLAMO, estados, true, null, fechaDesde, fechaHasta, null, false).Return;
        //    var total = ids.Count();
        //    if (total == 0)
        //    {
        //        result.AddErrorPublico("No hay requerimientos que cumplan con los filtros de busqueda");
        //        return result;
        //    }

        //    //CPCs
        //    var returnCpcs = new CpcRules(getUsuarioLogueado()).GetAll(false);
        //    if (!returnCpcs.Ok)
        //    {
        //        result.AddErrorPublico("Error obteniendo los CPCs.");
        //        return result;
        //    }
        //    var cpcs = returnCpcs.Return.FindAll(x => x.IdCatastro != 0).ToList();

        //    var listaCpcs = new List<Resultado_DatosEstadisticaPanel_Cpc>();

        //    foreach (Cpc cpc in cpcs)
        //    {
        //        //Ids de los reclamos con este IdCpc                
        //        var returnIdsCpc = requerimientoRules.GetIds(Enums.TipoRequerimiento.RECLAMO, estados, null, null, null, null, null, null, null, null, null, cpc.Id, null, fechaDesde, fechaHasta, false, null, false, null, null);
        //        if (!returnIdsCpc.Ok)
        //        {
        //            result.AddErrorPublico("Error generando el mapa");
        //            return result;
        //        }

        //        var cantidadCpc = returnIdsCpc.Return.Count();
        //        if (cantidadCpc != 0)
        //        {
        //            var resultadoCpc = new Resultado_DatosEstadisticaPanel_Cpc();
        //            resultadoCpc.Cpc = new Resultado_Cpc(cpc);
        //            resultadoCpc.IdsRequerimientos = returnIdsCpc.Return;
        //            resultadoCpc.CantidadRequerimientos = cantidadCpc;
        //            resultadoCpc.Porcentaje = Math.Round((((float)cantidadCpc) / total) * 100, 2);
        //            listaCpcs.Add(resultadoCpc);
        //        }
        //    }

        //    listaCpcs = SetColorYCriticidad(listaCpcs);
        //    result.Return = listaCpcs;
        //    return result;
        //    return CrearMapaCpc(listaCpcs);
        //}

        private List<Resultado_DatosEstadisticaPanel_Cpc> SetColorYCriticidad(List<Resultado_DatosEstadisticaPanel_Cpc> CPCs)
        {
            int maxValue = FindMaxValue(CPCs);

            foreach (var cpc in CPCs)
            {
                int cant = cpc.CantidadRequerimientos;
                string color = GetColor(maxValue, cant);
                string criticidad = GetCriticidad(maxValue, cant);

                cpc.Color = color;
                cpc.Criticidad = criticidad;
            }
            return CPCs;
        }

        private Result<string> CrearMapaCpc(List<Resultado_DatosEstadisticaPanel_Cpc> CPCs)
        {
            var result = new Result<string>();
            try
            {
                int maxValue = FindMaxValue(CPCs);
                var catastro = new CatastroMapas.CatastroWSDL();

                var datosCpc = new List<CatastroMapas.datosCPC>();

                foreach (var cpc in CPCs)
                {
                    var datoCpc = new CatastroMapas.datosCPC();
                    int cant = cpc.CantidadRequerimientos;
                    datoCpc.CantReclamos = cant;
                    datoCpc.NumeroCPC = cpc.Cpc.Numero;

                    string color = GetColor(maxValue, cant);
                    string criticidad = GetCriticidad(maxValue, cant);

                    datoCpc.Color = color;
                    datoCpc.Criticidad = criticidad;
                    datosCpc.Add(datoCpc);
                }

                CatastroMapas.datosCPC[] datos = datosCpc.ToArray();

                var url = catastro.reclamosCPC(datos);

                var parsedQuery = HttpUtility.ParseQueryString(url);
                var id = parsedQuery["idmapa"];
               
                result.Return = url;
            }
            catch (Exception e)
            {
                result.AddErrorPublico("Error comunicaciondose con el servicio de mapas");
                result.AddErrorInterno(e.Message);
                if (e.InnerException != null)
                {
                    result.AddErrorInterno(e.InnerException.Message);
                }
            }
            return result;
        }
        //private Result<string> CrearMapaCpcInternet(IList<Resultado_DatosEstadisticaMapa_Cpc> CPCs)
        //{
        //    var result = new Result<string>();
        //    try
        //    {
        //        int maxValue = FindMaxValue(CPCs);
        //        var catastro = new CatastroMapasInternet.CatastroWSDL();

        //        var datosCpc = new List<CatastroMapasInternet.datosCPC>();

        //        foreach (var cpc in CPCs)
        //        {
        //            var numeroCpc = 0;
        //            if (int.TryParse(cpc.Cpc.Numero, out numeroCpc))
        //            {
        //                var datoCpc = new CatastroMapasInternet.datosCPC();
        //                int cant = cpc.CantidadRequerimientos;
        //                datoCpc.CantReclamos = cant;
        //                datoCpc.NumeroCPC = numeroCpc;

        //                string color = GetColor(maxValue, cant);
        //                string criticidad = GetCriticidad(maxValue, cant);

        //                datoCpc.Color = color;
        //                datoCpc.Criticidad = criticidad;
        //                datosCpc.Add(datoCpc);
        //            }
        //        }

        //        CatastroMapasInternet.datosCPC[] datos = datosCpc.ToArray();

        //        var url = catastro.reclamosCPC(datos);



        //        var parsedQuery = HttpUtility.ParseQueryString(url);
        //        var id = parsedQuery["idmapa"];
      

        //        result.Return = url;
        //    }
        //    catch (Exception e)
        //    {
        //        result.AddErrorPublico("Error comunicaciondose con el servicio de mapas");
        //        result.AddErrorInterno(e.Message);
        //        if (e.InnerException != null)
        //        {
        //            result.AddErrorInterno(e.InnerException.Message);
        //        }
        //    }
        //    return result;
        //}
        /*
         * Busca el maximo dentro de un list
         */
        private int FindMaxValue(List<Resultado_DatosEstadisticaPanel_Cpc> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }
            int maxAge = int.MinValue;
            foreach (var cpc in list)
            {
                int cantReclamos = cpc.CantidadRequerimientos;
                if (cantReclamos > maxAge)
                {
                    maxAge = cantReclamos;
                }
            }
            return maxAge;
        }

        /*Me devuelve el color del cual se pintara el mapa , recibe como parametro la maxima cantidad de reclamos
         * de todos los CPCs y la cantidad de reclamos del CPC del cual se quiere obtener el color
         */
        public string GetColor(int maximo, int cantidad)
        {
            string resultado = null;

            var porcentaje = (cantidad * 100) / maximo;

            if (porcentaje <= 12.5)
            {
                resultado = "#2ACB3B";
                return resultado;
            }
            if (porcentaje <= 37.5)
            {
                resultado = "#DDFF53";
                return resultado;
            }

            if (porcentaje <= 62.5)
            {
                resultado = "#FFF000";
                return resultado;
            }

            if (porcentaje <= 87.5)
            {
                resultado = "#FFA041";
                return resultado;
            }

            if (porcentaje <= 100)
            {
                resultado = "#FF3636";
                return resultado;
            }
            return resultado;
        }

        public string GetCriticidad(int maximo, int cantidad)
        {
            string resultado = null;

            var porcentaje = (cantidad * 100) / maximo;

            if (porcentaje <= 12.5)
            {
                resultado = "Muy baja";
                return resultado;
            }
            if (porcentaje <= 37.5)
            {
                resultado = "Baja";
                return resultado;
            }

            if (porcentaje <= 62.5)
            {
                resultado = "Normal";
                return resultado;
            }

            if (porcentaje <= 87.5)
            {
                resultado = "Alta";
                return resultado;
            }

            if (porcentaje <= 100)
            {
                resultado = "Muy alta";
                return resultado;
            }
            return resultado;
        }
        #endregion

        #region Panel
        public Result<List<Resultado_DatosEstadisticaPanel_Motivos>> GetDatosEstadisticaPanel_Motivos(DateTime fechaDesde, DateTime fechaHasta)
        {
            return dao.GetDatosEstadisticaPanel(fechaDesde, fechaHasta);
        }

        public Result<Resultado_DatosEstadisticaPanel_Totales> GetDatosEstadisticaPanel_Totales(DateTime fechaDesde, DateTime fechaHasta)
        {
            return dao.GetDatosEstadisticaPanel_Totales(fechaDesde, fechaHasta);
        }

        public Result<List<Resultado_DatosEstadisticaPanel_Radial>> GetDatosEstadisticaPanel_Radial(DateTime fechaDesde, DateTime fechaHasta)
        {
            return dao.GetDatosEstadisticaPanel_Radial(fechaDesde, fechaHasta);
        }

        public Result<int> GetDatosEstadisticaPanel_Radial_Promedio(DateTime fechaDesde, DateTime fechaHasta)
        {
            return dao.GetDatosEstadisticaPanel_Radial_Promedio(fechaDesde, fechaHasta);
        }

        public Result<List<Resultado_DatosEstadisticaPanel_CriticidadServicio>> GetDatosEstadisticaPanel_CriticiidadServicio(DateTime fechaDesde, DateTime fechaHasta)
        {
            return dao.GetDatosEstadisticaPanel_CriticidadServicio(fechaDesde, fechaHasta);
        }
        #endregion

    }
}

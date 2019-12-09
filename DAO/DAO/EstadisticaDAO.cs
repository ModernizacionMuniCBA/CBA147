using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Entities;
using System.Text;
using Model.Resultados;
using NHibernate;
using Model.Consultas;
using Model.Resultados.Estadisticas;

namespace DAO.DAO
{
    public class EstadisticaDAO : BaseDAO<BaseEntity>
    {
        private static EstadisticaDAO instance;

        public static EstadisticaDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EstadisticaDAO();
                }
                return instance;
            }
        }

        public Result<List<object[]>> Estadistica_Requerimiento_CPC(bool? relevamientoInterno, List<Enums.EstadoRequerimiento> estadosRequerimiento, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var result = new Result<List<object[]>>();

            var sql = "EXEC Estaditica_RequerimientoCPC";
            var parametros = new List<string>();

            //Relevamiento interno
            if (relevamientoInterno.HasValue)
            {
                parametros.Add("@RelevamientoInterno = " + (relevamientoInterno.Value ? "1" : "0"));
            }

            //Esatdos
            if (estadosRequerimiento != null && estadosRequerimiento.Count != 0)
            {
                StringBuilder keysEstados = new StringBuilder();
                foreach (var estado in estadosRequerimiento)
                {
                    if (keysEstados.Length != 0)
                    {
                        keysEstados.Append(", ");
                    }
                    keysEstados.Append("" + (int)estado);
                }
                parametros.Add("@Estados = (" + keysEstados + ")");
            }


            //fecha desde
            if (fechaDesde.HasValue)
            {
                parametros.Add("@FechaDesde = " + fechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT));
            }

            //fecha hasta
            if (fechaHasta.HasValue)
            {
                parametros.Add("@FechaHasta= " + fechaHasta.Value.ToString(Utils.SQLDATETIME_FORMAT));
            }

            sql += " " + string.Join(", ", parametros);

            result.Return = GetSession().CreateSQLQuery(sql).List<object[]>().ToList();
            return result;
        }

        #region Panel

        public Result<List<Resultado_DatosEstadisticaPanel_Motivos>> GetDatosEstadisticaPanel(DateTime fechaDesde, DateTime fechaHasta)
        {
            /*Nos da el ranking de motivos*/
            var result = new Result<List<Resultado_DatosEstadisticaPanel_Motivos>>();

            try
            {
                var fechaDesdeString = fechaDesde.ToString(Utils.SQLDATETIME_FORMAT);
                var fechaHastaString = fechaHasta.ToString(Utils.SQLDATETIME_FORMAT);
                var sql = @"
                            SELECT m.Nombre AS Motivo, Count(*) AS toprank
                            FROM Tema t
                            INNER JOIN dbo.Servicio s ON t.IdServicio = s.Id
                            INNER JOIN dbo.Motivo m ON m.IdTema = t.Id
                            INNER JOIN dbo.Requerimiento r ON r.IdMotivo = m.Id
                            WHERE r.FechaBaja is null   AND
                            r.FechaAlta BETWEEN" + "'" + fechaDesdeString + "'" + " AND " + "'" + fechaHastaString + "'" + @"
                            GROUP BY
                            m.Nombre,
                            s.Nombre
                            order by toprank desc";
                var query = GetSession().CreateSQLQuery(sql);
                var lista = new List<Resultado_DatosEstadisticaPanel_Motivos>();
                foreach (object[] obj in query.List<object[]>().ToList())
                {
                    var resultado = new Resultado_DatosEstadisticaPanel_Motivos();
                    resultado.Motivo = (string)obj[0];
                    resultado.Cantidad = (int)obj[1];
                    lista.Add(resultado);
                }

                result.Return = lista;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<Resultado_DatosEstadisticaPanel_Totales> GetDatosEstadisticaPanel_Totales(DateTime fechaDesde, DateTime fechaHasta)
        {
            var result = new Result<Resultado_DatosEstadisticaPanel_Totales>();
            try
            {

                var fechaDesdeString = fechaDesde.ToString(Utils.SQLDATETIME_FORMAT);
                var fechaHastaString = fechaHasta.ToString(Utils.SQLDATETIME_FORMAT);
                /*Nos da el total de reclamos de esas fechas*/
                var sql1 = @"
                            SELECT Count(*) As Total
                            FROM
                            Requerimiento r
                            INNER JOIN EstadoRequerimientoHistorial erh ON erh.IdRequerimiento = r.Id
                            INNER JOIN EstadoRequerimiento er ON erh.IdEstado = er.Id
                            WHERE
                            erh.Ultimo = 1 AND
                            r.FechaBaja IS NULL AND 
                            r.FechaAlta BETWEEN" + "'" + fechaDesdeString + "'" + "AND" + "'" + fechaHastaString + "'";

                var query1 = GetSession().CreateSQLQuery(sql1);
                /*De ese total , sacamos la cantidad que se consideran atendidos(todos los estados menos el nuevo)*/
                var sql2 = @"
                            SELECT Count(*) As Total
                            FROM
                            Requerimiento r
                            INNER JOIN EstadoRequerimientoHistorial erh ON erh.IdRequerimiento = r.Id
                            INNER JOIN EstadoRequerimiento er ON erh.IdEstado = er.Id
                            WHERE
                            erh.Ultimo = 1 AND
                            r.FechaBaja IS NULL AND 
                            r.FechaAlta BETWEEN" + "'" + fechaDesdeString + "'" + "AND" + "'" + fechaHastaString + "'" + @"
                            AND er.KeyValue IN (2,4,6,5,7,9,8)";

                var query2 = GetSession().CreateSQLQuery(sql2);
                //Trae la fila cero y el regsitro cero
                var total = query1.List<int>()[0];

                var indice = query2.List<int>()[0];

                var porcentaje = (indice * 100) / total;

                var resultado = new Resultado_DatosEstadisticaPanel_Totales();
                resultado.Total = total;
                resultado.Porcentaje = porcentaje;
                result.Return = resultado;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<List<Resultado_DatosEstadisticaPanel_Radial>> GetDatosEstadisticaPanel_Radial(DateTime fechaDesde, DateTime fechaHasta)
        {
            var result = new Result<List<Resultado_DatosEstadisticaPanel_Radial>>();

            var fechaDesdeString = fechaDesde.ToString(Utils.SQLDATETIME_FORMAT);
            var fechaHastaString = fechaHasta.ToString(Utils.SQLDATETIME_FORMAT);

            try
            {
                var sql = @"
                            SELECT s.Nombre as Servicio, ROUND(avg(CAST((DATEDIFF(DAY, r.FechaAlta, erh.FechaAlta) +1) as FLOAT)),2) as Dias
                            FROM
                            dbo.Requerimiento r
                            INNER JOIN dbo.Motivo m ON r.IdMotivo = m.Id
                            INNER JOIN dbo.Tema t ON m.IdTema = t.Id
                            INNER JOIN dbo.Servicio s ON t.IdServicio = s.Id
                            INNER JOIN dbo.EstadoRequerimientoHistorial erh on erh.IdRequerimiento = r.Id
                            Inner join dbo.EstadoRequerimiento er on erh.IdEstado = er.Id
                            where er.KeyValue = 7 and erh.Ultimo = 1 and s.FechaBaja is null and r.FechaBaja is null and erh.FechaBaja is null
                            and r.FechaAlta BETWEEN" + "'" + fechaDesdeString + "'" + "AND" + "'" + fechaHastaString + "'" + @"
                            group by s.Nombre";
                var query = GetSession().CreateSQLQuery(sql);
                var lista = new List<Resultado_DatosEstadisticaPanel_Radial>();
                foreach (object[] obj in query.List<object[]>().ToList())
                {
                    var resultado = new Resultado_DatosEstadisticaPanel_Radial();
                    resultado.Servicio = (string)obj[0];
                    resultado.Dias = (double)obj[1];
                    lista.Add(resultado);
                }

                result.Return = lista;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<int> GetDatosEstadisticaPanel_Radial_Promedio(DateTime fechaDesde, DateTime fechaHasta)
        {
            var result = new Result<int>();
            var fechaDesdeString = fechaDesde.ToString(Utils.SQLDATETIME_FORMAT);
            var fechaHastaString = fechaHasta.ToString(Utils.SQLDATETIME_FORMAT);

            try
            {
                var sql = @"
                            SELECT CAST(ROUND(avg(CAST((DATEDIFF(DAY, r.FechaAlta, erh.FechaAlta) +1) as FLOAT)),0) as int) as Dias
                            FROM
                            dbo.Requerimiento r
                            INNER JOIN dbo.Motivo m ON r.IdMotivo = m.Id
                            INNER JOIN dbo.Tema t ON m.IdTema = t.Id
                            INNER JOIN dbo.Servicio s ON t.IdServicio = s.Id
                            INNER JOIN dbo.EstadoRequerimientoHistorial erh on erh.IdRequerimiento = r.Id
                            Inner join dbo.EstadoRequerimiento er on erh.IdEstado = er.Id
                            where er.KeyValue = 7 and erh.Ultimo = 1 and s.FechaBaja is null and r.FechaBaja is null and erh.FechaBaja is null
                            and r.FechaAlta BETWEEN" + "'" + fechaDesdeString + "'" + "AND" + "'" + fechaHastaString + "'" + @"     
";
                var query = GetSession().CreateSQLQuery(sql);
                var resultado = query.List<int>()[0];
                result.Return = resultado;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<List<Resultado_DatosEstadisticaPanel_CriticidadServicio>> GetDatosEstadisticaPanel_CriticidadServicio(DateTime fechaDesde, DateTime fechaHasta)
        {
            var result = new Result<List<Resultado_DatosEstadisticaPanel_CriticidadServicio>>();

            try
            {
                var fechaDesdeString = fechaDesde.ToString(Utils.SQLDATETIME_FORMAT);
                var fechaHastaString = fechaHasta.ToString(Utils.SQLDATETIME_FORMAT);

                var sql = @"
Select 
a.Id as IdArea,
a.Nombre as Area,
(
	Select count(*) from Requerimiento r1
	inner join Motivo m1 on m1.Id = r1.IdMotivo
	inner join CerrojoArea a1 on a1.Id = m1.IdAreaCerrojo
	INNER JOIN EstadoRequerimientoHistorial erh1 ON erh1.IdRequerimiento = r1.Id
	INNER JOIN EstadoRequerimiento er1 ON erh1.IdEstado = er1.Id
	where 
	r1.FechaBaja is null and 
	erh1.Ultimo = 1 and 
    er1.KeyValue in (9,2,4,5,6,7,8) and

	a1.Id = a.Id and 
	erh1.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'
) as Atendidos,
count(*) as Total
from Requerimiento r
inner join Motivo m on m.Id = r.IdMotivo
inner join CerrojoArea a on a.Id = r.IdAreaCerrojoResponsable
INNER JOIN EstadoRequerimientoHistorial erh ON erh.IdRequerimiento = r.Id
INNER JOIN EstadoRequerimiento er ON erh.IdEstado = er.Id
where r.FechaBaja is null and erh.Ultimo = 1 and
erh.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'
group by a.Id, a.Nombre
order by Total desc
                ";

                //                                var sql = @"
                //                                CREATE TABLE #TEMPTABLE
                //                                (
                //                                IdServicio int,
                //                                Nombre varchar(max),
                //                                Total int,
                //                                Atendidos int
                //                                )
                //                
                //                                INSERT INTO #TEMPTABLE
                //                                SELECT s.Id as IdServicio, s.Nombre as Nombre, count(*) as Total, (SELECT count(*)
                //                                FROM
                //                                dbo.Servicio s1
                //                                INNER JOIN Tema t1 ON t1.IdServicio = s1.Id
                //                                INNER JOIN Motivo m1 ON m1.IdTema = t1.Id
                //                                INNER JOIN Requerimiento r1 ON r1.IdMotivo = m1.Id
                //                                INNER JOIN EstadoRequerimientoHistorial erh1 ON erh1.IdRequerimiento = r1.Id
                //                                INNER JOIN EstadoRequerimiento er1 ON erh1.IdEstado = er1.Id
                //                                WHERE r1.FechaBaja IS NULL and erh1.Ultimo = 1 and
                //                                er1.KeyValue in (2,4,6,5,7,9,8) and s1.Id = s.Id AND
                //                                r1.FechaAlta BETWEEN" + "'" + fechaDesdeString + "'" + "AND" + "'" + fechaHastaString + "'" + @"
                //                
                //                                ) as Atendidos
                //                                FROM
                //                                dbo.Servicio s
                //                                INNER JOIN Tema t ON t.IdServicio = s.Id
                //                                INNER JOIN Motivo m ON m.IdTema = t.Id
                //                                INNER JOIN Requerimiento r ON r.IdMotivo = m.Id
                //                                INNER JOIN EstadoRequerimientoHistorial erh ON erh.IdRequerimiento = r.Id
                //                                INNER JOIN EstadoRequerimiento er ON erh.IdEstado = er.Id
                //                                WHERE r.FechaBaja IS NULL and erh.Ultimo = 1 AND
                //                                r.FechaAlta BETWEEN + "'" + fechaDesdeString + "'" + "AND" + "'" + fechaHastaString + "'" + @"
                //                                group by s.Id, s.Nombre


                //                                SELECT *, ROUND((Atendidos*100/Total),2) as Porcentaje
                //                                FROM #TEMPTABLE
                //                                DROP TABLE #TEMPTABLE";
                var query = GetSession().CreateSQLQuery(sql);
                var data = query.List();
                var lista = new List<Resultado_DatosEstadisticaPanel_CriticidadServicio>();
                foreach (object[] obj in data)
                {
                    var resultado = new Resultado_DatosEstadisticaPanel_CriticidadServicio();
                    resultado.IdArea = (int)obj[0];
                    resultado.Area = (string)obj[1];
                    resultado.Atendidos = (int)obj[2];
                    resultado.Total = (int)obj[3];

                    resultado.Porcentaje = ((float)resultado.Atendidos / resultado.Total) * 100;
                    resultado.Color = GetColorCriticidadServicio(resultado.Porcentaje);
                    lista.Add(resultado);
                }

                lista.Sort((x, y) => y.Porcentaje.CompareTo(x.Porcentaje));
                result.Return = lista;
            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }


        public string GetColorCriticidadServicio(float porcentaje)
        {
            if (porcentaje <= 20)
            {
                return "#FF3636";
            }

            if (porcentaje <= 40)
            {
                return "#FFA041";
            }
            if (porcentaje <= 60)
            {
                return "#FFF000";
            }
            if (porcentaje <= 80)
            {
                return "#DDFF53";
            }

            if (porcentaje <= 100)
            {
                return "#2ACB3B";
            }

            return "#FF00FF";
            //string resultado = null;

            //List<RangoCriticidadServicio> criticidad = null;

            ////calculo la criticadad buscandola en la base de daqtos o seteandola por default
            //var consultaCriticidadesServicio = RangoCriticidadServicioDAO.Instance.GetByIdServicio(idServicio, false);

            ////si no hay valores para ese servicio pone la tabla por defecto
            //if (!consultaCriticidadesServicio.Ok)
            //{
            //    // criticidad = GetDefault();
            //    //Tirar excepcion
            //}
            //else
            //{
            //    criticidad = consultaCriticidadesServicio.Return;
            //    if (criticidad == null)
            //    {
            //        // criticidad = GetDefault();
            //    }
            //}

            //var ultimo = criticidad.Count;
            //int pivot = 0;
            //bool esUltimo = false;

            //foreach (var c in criticidad)
            //{
            //    pivot++;
            //    //En el ultimo valor del list se tiene que comprobar que sea mayor o igual , asi se incluyen todos los que sean mas grandes.
            //    /*
            //    if (ultimo == pivot)
            //    {
            //        var desde = c.Desde;
            //        if (cantidad >= desde)
            //        {
            //            resultado = c.Color;
            //            return resultado;
            //        }
            //    }*/

            //    var desde = c.Desde;
            //    if (cantidad >= desde)
            //    {
            //        resultado = c.Color;
            //    }
            //    else
            //    {
            //        if (resultado == null)
            //        {
            //            return "FFFFFF";
            //        }
            //        return resultado;
            //    }
            //}
            //return resultado;
        }
        #endregion

        #region Estadisticas v2

        public Result<List<Resultado_DatosEstadisticaOrigen>> GetDatosEstadisticaOrigen(Consulta_EstadisticaOrigen consulta)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaOrigen>>();

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }

            var año = consulta.Año;
            var mes = consulta.Mes;
            //var lineaWhere1 = "";
            var lineaWhere2 = "";

            if (año != null && mes != null)
            {
                // lineaWhere1 = "WHERE month(r2.FechaAlta)=" + mes + "AND year(r2.FechaAlta) =" + año + @"";
                lineaWhere2 = "WHERE month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                // lineaWhere1 = "WHERE r2.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
                lineaWhere2 = "WHERE r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";

            }


            try
            {
                var sql = @"
                            SELECT o.Nombre AS Origen, o.Id as IdOrigen, Count(*) AS Cantidad,
                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje                         
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN dbo.Origen AS o ON r.IdOrigen = o.Id " + lineaWhere2 + @"
                            
                            and r.FechaBaja is null
                            GROUP BY o.Nombre, o.Id";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaOrigen)));
                var resultado = query.List<Resultado_DatosEstadisticaOrigen>().ToList();
                var data = new List<Resultado_DatosEstadisticaOrigen>();



                List<Resultado_DatosEstadisticaOrigen> dataIntermedio = new List<Resultado_DatosEstadisticaOrigen>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);


                    foreach (var resultadoEstadisticaOrigen in data)
                    {
                        Resultado_DatosEstadisticaOrigen nuevoResult = new Resultado_DatosEstadisticaOrigen();
                        nuevoResult.Cantidad = resultadoEstadisticaOrigen.Cantidad;
                        nuevoResult.IdOrigen = resultadoEstadisticaOrigen.IdOrigen;
                        nuevoResult.Origen = resultadoEstadisticaOrigen.Origen;
                        nuevoResult.Porcentaje = resultadoEstadisticaOrigen.Porcentaje;

                        int idOrigen = resultadoEstadisticaOrigen.IdOrigen;

                        var sql2 = @"

                            SELECT r.Id
                            FROM    
                            dbo.Requerimiento AS r
                            INNER JOIN dbo.Origen AS o ON r.IdOrigen = o.Id " + lineaWhere2 + @"                           
                            and r.FechaBaja is null
                            and o.Id =" + idOrigen;

                        IQuery query2 = GetSession().CreateSQLQuery(sql2);

                        List<int> idsRequerimientos = query2.List<int>().ToList();

                        nuevoResult.IdsRequerimientos = idsRequerimientos;

                        dataIntermedio.Add(nuevoResult);
                    }
                }


                List<Resultado_DatosEstadisticaOrigen> dataFinal = new List<Resultado_DatosEstadisticaOrigen>();
                if (resultado != null && resultado.Count != 0)
                {
                    // data.AddRange(resultado);

                    /*Armo el dataFinal porque se necesitan todos los CPC en una sola fila*/
                    //List<Resultado_DatosEstadisticaOrigen> dataFinal = new List<Resultado_DatosEstadisticaOrigen>();
                    /*Primero agrego todas las filas menos las del cpc*/
                    dataFinal.AddRange(dataIntermedio.Where(x => !x.Origen.ToLower().Contains("cpc")));
                    /*Armo la ultima fila con la suma de los porcentajes y la suma de las cantidades y se las seteo a la fila nueva*/
                    var listaCpcs = dataIntermedio.Where(x => x.Origen.ToLower().Contains("cpc")).ToList<Resultado_DatosEstadisticaOrigen>();
                    var porcentaje = Math.Round(listaCpcs.Select(x => x.Porcentaje).Sum(), 2);
                    var cantidad = listaCpcs.Select(x => x.Cantidad).Sum();


                    var idsrequerimientos = new List<int>();
                    listaCpcs.Select(x => x.IdsRequerimientos).ToList().ForEach(x => { idsrequerimientos.AddRange(x); });


                    dataFinal.Add(new Resultado_DatosEstadisticaOrigen()
                    {
                        Origen = "Cpcs",
                        Cantidad = cantidad,
                        Porcentaje = porcentaje,
                        IdsRequerimientos = idsrequerimientos
                    });

                }

                result.Return = dataFinal;
                return result;


            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
        public Result<List<Resultado_DatosEstadisticaEficacia>> GetDatosEstadisticaEficacia(Consulta_EstadisticaEficacia consulta)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaEficacia>>();

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }


            // var IdsArea = consulta.IdsArea;


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

            var año = consulta.Año;
            var mes = consulta.Mes;
            //var lineaWhere1 = "";
            //var lineaWhere2 = "";
            var lineaWhere3 = "";
            var lineaWhere4 = "";

            if (año != null && mes != null)
            {
                //lineaWhere1 = "and month(erh3.Fecha)=" + mes + "AND year(erh3.Fecha) =" + año + @"";
                //lineaWhere2 = "and month(r2.FechaAlta)=" + mes + "AND year(r2.FechaAlta) =" + año + @"";
                lineaWhere3 = "and month(erh1.Fecha)=" + mes + "AND year(erh1.Fecha) =" + año + @"";
                lineaWhere4 = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                //lineaWhere1 = "and erh3.Fecha BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
                //lineaWhere2 = "and  r2.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
                lineaWhere3 = "and erh1.Fecha BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
                lineaWhere4 = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
            }
            
            try
            {

                var data = new List<Resultado_DatosEstadisticaEficacia>();

                var sql = @"
                            SELECT
                            case
                                when er.KeyValue = 1 then 'Sin Atención'
		                        when er.KeyValue in (5,7) then 'Resueltos'
                                when er.KeyValue not in (1,5,7) then 'En Atención'
                            end Condicion,
                            Count(*) AS Cantidad,
                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje
                            
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN dbo.EstadoRequerimientoHistorial AS erh ON erh.Id =
                                (
			                        SELECT TOP 1 id
			                        FROM EstadoRequerimientoHistorial erh1
        			                WHERE erh1.IdRequerimiento = r.Id " + lineaWhere3 + @"                                    
                                    ORDER BY erh1.Fecha desc
                                )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id
                            inner join Motivo m on m.Id=r.IdMotivo
                            inner JOIN CerrojoArea a on a.Id=r.IdAreaCerrojoResponsable
                            WHERE a.Id in (" + string.Join(", ", idsAreas) + @") " + lineaWhere4 + @"
                            GROUP BY
	                        case
		                        when er.KeyValue = 1 then 'Sin Atención'
		                        when er.KeyValue in (5,7) then 'Resueltos'
                                when er.KeyValue not in (1,5,7) then 'En Atención'
                            end
                            order by Cantidad desc";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaEficacia)));
                var resultado = query.List<Resultado_DatosEstadisticaEficacia>().ToList();



                List<Resultado_DatosEstadisticaEficacia> dataFinal = new List<Resultado_DatosEstadisticaEficacia>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);


                    foreach (var resultadoEstadisticaEficacia in data)
                    {
                        Resultado_DatosEstadisticaEficacia nuevoResult = new Resultado_DatosEstadisticaEficacia();
                        nuevoResult.Cantidad = resultadoEstadisticaEficacia.Cantidad;
                        nuevoResult.Condicion = resultadoEstadisticaEficacia.Condicion;
                        nuevoResult.Porcentaje = resultadoEstadisticaEficacia.Porcentaje;

                        var lineaWherePendientesOAtendidos = "";

                        if (resultadoEstadisticaEficacia.Condicion == "Sin Atención")
                        {
                            lineaWherePendientesOAtendidos = "and er.KeyValue = 1";

                        }
                        if (resultadoEstadisticaEficacia.Condicion == "Atendidos")
                        {
                            lineaWherePendientesOAtendidos = "and er.KeyValue not in (1,5,7)";
                        }
                        if (resultadoEstadisticaEficacia.Condicion == "Resueltos")
                        {
                            lineaWherePendientesOAtendidos = "and er.KeyValue in (5,7)";
                        }

                        var sql2Pendientes = @"
                            SELECT r.Id
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN dbo.EstadoRequerimientoHistorial AS erh ON erh.Id =
                                (
			                        SELECT TOP 1 id
			                        FROM EstadoRequerimientoHistorial erh1
        			                WHERE erh1.IdRequerimiento = r.Id " + lineaWhere3 + @"                                    
                                    ORDER BY erh1.Fecha desc
                                )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id
                            inner join Motivo m on m.Id=r.IdMotivo
                            inner JOIN CerrojoArea a on a.Id=r.IdAreaCerrojoResponsable
                            WHERE a.Id in (" + string.Join(", ", idsAreas) + @") " + lineaWherePendientesOAtendidos + lineaWhere4 + @"
                            ";

                        IQuery query2 = GetSession().CreateSQLQuery(sql2Pendientes);

                        List<int> idsRequerimientos = query2.List<int>().ToList();

                        nuevoResult.IdsRequerimientos = idsRequerimientos;

                        dataFinal.Add(nuevoResult);
                    }
                }

                result.Return = dataFinal;
                return result;


            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
        public Result<List<Resultado_DatosEstadisticaResueltos>> GetDatosEstadisticaResueltos(Consulta_EstadisticaResueltos consulta)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaResueltos>>();

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }


            //var IdsArea = consulta.IdsArea;

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

            var año = consulta.Año;
            var mes = consulta.Mes;
            var lineaWhere1 = "";

            if (año != null && mes != null)
            {

                lineaWhere1 = "and month(erh1.Fecha)=" + mes + "AND year(erh1.Fecha) =" + año + @"";

            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                lineaWhere1 = "and erh1.Fecha BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
            }


            try
            {

                var data = new List<Resultado_DatosEstadisticaResueltos>();

                var sql = @"
                            SELECT
                            Year (r.FechaAlta) AS [Año],
                            Month(r.FechaAlta) AS Mes,
                            Count(*) AS Cantidad,
                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje                              

                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN dbo.EstadoRequerimientoHistorial AS erh ON erh.Id = (
			                SELECT TOP 1 id
			                FROM EstadoRequerimientoHistorial erh1
			                WHERE erh1.IdRequerimiento = r.Id " + lineaWhere1 + @"                                                     			               
			                ORDER BY erh1.Fecha desc
		                    )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in(5,7)
                            INNER JOIN Motivo m on m.Id=r.IdMotivo
                            INNER JOIN CerrojoArea a on a.Id=r.IdAreaCerrojoResponsable
                            WHERE r.FechaBaja is null AND a.Id in (" + string.Join(", ", idsAreas) + @")  
                            GROUP BY
                            year(r.FechaAlta),
                            month(r.FechaAlta)
                            ORDER BY año desc , mes desc
                            ";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaResueltos)));
                var resultado = query.List<Resultado_DatosEstadisticaResueltos>().ToList();

                /*Esta es la consulta que me trae todos los ids , que luego con linq voy a agrupar por año y mes , para asi btener la lsita de ids y setearsela al resultado_datosestadisticaresueltos*/
                var sql2 = @"
                SELECT
                r.Id as IdRequerimiento,
                month(r.FechaAlta) as Mes,
                year(r.FechaAlta) as Año
                FROM
                dbo.Requerimiento AS r
                INNER JOIN dbo.EstadoRequerimientoHistorial AS erh ON erh.Id = (
							SELECT TOP 1 id
			                FROM EstadoRequerimientoHistorial erh1
                            WHERE erh1.IdRequerimiento = r.Id " + lineaWhere1 + @"			                
			                ORDER BY erh1.Fecha desc
		                    )
                INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in(5,7)
                INNER JOIN Motivo m on m.Id=r.IdMotivo
                INNER JOIN CerrojoArea a on a.Id=r.IdAreaCerrojoResponsable
                WHERE r.FechaBaja is null AND a.Id in (" + string.Join(", ", idsAreas) + @")
                ";



                IQuery query2 = GetSession().CreateSQLQuery(sql2);
                query2.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaResueltos_Intermedio)));
                var resultado2 = query2.List<Resultado_DatosEstadisticaResueltos_Intermedio>().ToList();

                /*Es un resultado intermedio*/
                List<Resultado_DatosEstadisticaResueltos> data2 = new List<Resultado_DatosEstadisticaResueltos>();
                var testeando = resultado2
                    .GroupBy(u => new { u.Mes, u.Año })
                    .Select(grp => new Resultado_DatosEstadisticaResueltos
                    {
                        Mes = grp.Key.Mes,
                        Año = grp.Key.Año,
                        IdsRequerimientos = grp.ToList().Select(x => x.IdRequerimiento).ToList()
                    })
                    .ToList().OrderByDescending(x => x.Año).ThenByDescending(x => x.Mes);
                data2.AddRange(testeando);



                List<Resultado_DatosEstadisticaResueltos> dataFinal = new List<Resultado_DatosEstadisticaResueltos>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);

                    int contador = 1;


                    foreach (var resultadoEstadistica in data)
                    {
                        Resultado_DatosEstadisticaResueltos nuevoResult = new Resultado_DatosEstadisticaResueltos();
                        nuevoResult.Año = resultadoEstadistica.Año;
                        nuevoResult.Mes = resultadoEstadistica.Mes;
                        nuevoResult.Cantidad = resultadoEstadistica.Cantidad;
                        nuevoResult.Porcentaje = resultadoEstadistica.Porcentaje;
                        if (contador == 1)
                        {
                            nuevoResult.Etiqueta = "Mes seleccionado";

                        }
                        else
                        {
                            nuevoResult.Etiqueta = "" + resultadoEstadistica.Mes + "/" + resultadoEstadistica.Año;
                            //int proximoContador = contador - 1;
                            //if (proximoContador == 1)
                            //{
                            //    nuevoResult.Etiqueta = proximoContador + " Mes antes";
                            //}
                            //else
                            //{
                            //    nuevoResult.Etiqueta = proximoContador + " Meses antes";
                            //}

                        }

                        foreach (var r in data2)
                        {
                            if (r.Año == resultadoEstadistica.Año && r.Mes == resultadoEstadistica.Mes)
                            {
                                nuevoResult.IdsRequerimientos = r.IdsRequerimientos;
                            }
                        }

                        dataFinal.Add(nuevoResult);
                        contador++;
                    }
                }

                List<Resultado_DatosEstadisticaResueltos> dataFinal2 = new List<Resultado_DatosEstadisticaResueltos>();
                if (resultado != null && resultado.Count != 0)
                {

                    /*Armo el dataFinal porque se necesitan todos los CPC en una sola fila*/
                    /*Primero agrego todas las filas menos las del cpc*/
                    dataFinal2.AddRange(dataFinal.Take(10));
                    /*Armo la ultima fila con la suma de los porcentajes y la suma de las cantidades y se las seteo a la fila nueva*/
                    var listaOtros = dataFinal.Except(dataFinal2).ToList<Resultado_DatosEstadisticaResueltos>();//no se se si va asi o al revez

                    var porcentaje = Math.Round(listaOtros.Select(x => x.Porcentaje).Sum(), 2);
                    var cantidad = listaOtros.Select(x => x.Cantidad).Sum();


                    var idsrequerimientos = new List<int>();
                    listaOtros.Select(x => x.IdsRequerimientos).ToList().ForEach(x => { idsrequerimientos.AddRange(x); });


                    dataFinal2.Add(new Resultado_DatosEstadisticaResueltos()
                    {
                        Etiqueta = "10 o mas",
                        Cantidad = cantidad,
                        Porcentaje = porcentaje,
                        IdsRequerimientos = idsrequerimientos
                    });

                }

                result.Return = dataFinal2;
                return result;


            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
        public Result<List<Resultado_DatosEstadisticaServicios>> GetDatosEstadisticaServicios(Consulta_EstadisticaServicios consulta)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaServicios>>();

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }



            var año = consulta.Año;
            var mes = consulta.Mes;
            var lineaWhere = "";


            if (año != null && mes != null)
            {
                lineaWhere = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                lineaWhere = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
            }


            try
            {
                var sql = @"
                            SELECT s.Nombre AS Servicio, s.Id as IdServicio, Count(*) AS Cantidad,
                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje                         
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id
                            INNER JOIN dbo.Tema AS t ON m.IdTema = t.Id
                            INNER JOIN dbo.Servicio AS s ON t.IdServicio = s.Id
                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND t.FechaBaja is null AND s.FechaBaja is null " + lineaWhere + @"
                            GROUP BY s.Id, s.Nombre
                            ORDER BY count(*) DESC";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaServicios)));
                var resultado = query.List<Resultado_DatosEstadisticaServicios>().ToList();
                var data = new List<Resultado_DatosEstadisticaServicios>();



                List<Resultado_DatosEstadisticaServicios> dataIntermedio = new List<Resultado_DatosEstadisticaServicios>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);


                    foreach (var resultadoEstadisticaServicio in data)
                    {
                        Resultado_DatosEstadisticaServicios nuevoResult = new Resultado_DatosEstadisticaServicios();
                        nuevoResult.Cantidad = resultadoEstadisticaServicio.Cantidad;
                        nuevoResult.IdServicio = resultadoEstadisticaServicio.IdServicio;
                        nuevoResult.Servicio = resultadoEstadisticaServicio.Servicio;
                        nuevoResult.Porcentaje = resultadoEstadisticaServicio.Porcentaje;

                        int idServicio = resultadoEstadisticaServicio.IdServicio;

                        var sql2 = @"


                            SELECT r.Id                                           
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id
                            INNER JOIN dbo.Tema AS t ON m.IdTema = t.Id
                            INNER JOIN dbo.Servicio AS s ON t.IdServicio = s.Id
                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND t.FechaBaja is null AND s.FechaBaja is null " + lineaWhere + @"
                            and s.Id =" + idServicio;

                        IQuery query2 = GetSession().CreateSQLQuery(sql2);

                        List<int> idsRequerimientos = query2.List<int>().ToList();

                        nuevoResult.IdsRequerimientos = idsRequerimientos;

                        dataIntermedio.Add(nuevoResult);
                    }
                }


                List<Resultado_DatosEstadisticaServicios> dataFinal = new List<Resultado_DatosEstadisticaServicios>();
                if (resultado != null && resultado.Count != 0)
                {

                    /*Armo el dataFinal porque se necesitan todos los CPC en una sola fila*/
                    /*Primero agrego todas las filas menos las del cpc*/
                    dataFinal.AddRange(dataIntermedio.Take(9));
                    /*Armo la ultima fila con la suma de los porcentajes y la suma de las cantidades y se las seteo a la fila nueva*/
                    var listaOtros = dataIntermedio.Except(dataFinal).ToList<Resultado_DatosEstadisticaServicios>();//no se se si va asi o al revez

                    var porcentaje = Math.Round(listaOtros.Select(x => x.Porcentaje).Sum(), 2);
                    var cantidad = listaOtros.Select(x => x.Cantidad).Sum();


                    var idsrequerimientos = new List<int>();
                    listaOtros.Select(x => x.IdsRequerimientos).ToList().ForEach(x => { idsrequerimientos.AddRange(x); });


                    dataFinal.Add(new Resultado_DatosEstadisticaServicios()
                    {
                        Servicio = "Otros",
                        Cantidad = cantidad,
                        Porcentaje = porcentaje,
                        IdsRequerimientos = idsrequerimientos
                    });

                }

                result.Return = dataFinal;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
//        public Result<List<Resultado_DatosEstadisticaArea>> GetDatosEstadisticaArea(Consulta_EstadisticaArea consulta)
//        {
//            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
//            var result = new Result<List<Resultado_DatosEstadisticaArea>>();

//            var fechaDesdeString = "";
//            var fechaHastaString = "";
//            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
//            {
//                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
//                /*Agrego hasta el final del dia*/
//                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
//            }

//            var año = consulta.Año;
//            var mes = consulta.Mes;
//            var lineaWhere = "";


//            if (año != null && mes != null)
//            {
//                lineaWhere = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
//            }
//            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
//            {
//                lineaWhere = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
//            }

//            List<int> est = new List<int>();
//            consulta.EstadosKeyValue.ForEach(x => est.Add((int)x));


//            try
//            {
//                var sql = @"
//
//                            SELECT a.Nombre AS Area, a.Id as IdArea, COUNT (*) AS Cantidad,
//                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje
//                            FROM
//                            dbo.Requerimiento AS r
//                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
//				            (
//				            SELECT TOP 1 id
//				            FROM EstadoRequerimientoHistorial erh1 
//				            WHERE erh1.IdRequerimiento = r.Id 
//				            ORDER BY erh1.Fecha desc
//				            )
//                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")
//
//                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id
//                            INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable                            
//                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND a.FechaBaja is null " + lineaWhere + @"
//  
//                          
//                            GROUP BY a.Id, a.Nombre
//                            ORDER BY count(*) DESC";

//                IQuery query = GetSession().CreateSQLQuery(sql);
//                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaArea)));
//                var resultado = query.List<Resultado_DatosEstadisticaArea>().ToList();
//                var data = new List<Resultado_DatosEstadisticaArea>();

//                var listaAreasLimpia = new List<int?>();
//                if (resultado != null && resultado.Count != 0)
//                {
//                    data.AddRange(resultado);
//                    /*al data le agrefo los ids de los rqs*/
//                    foreach (var resultadoEstadisticaArea in data)
//                    {
//                        int idArea = resultadoEstadisticaArea.IdArea;
//                        var sql2 = @"
//                            SELECT r.Id                                           
//                            FROM
//                            dbo.Requerimiento AS r
//                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
//				            (
//				            SELECT TOP 1 id
//				            FROM EstadoRequerimientoHistorial erh1 
//				            WHERE erh1.IdRequerimiento = r.Id 
//				            ORDER BY erh1.Fecha desc
//				            )
//                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")
//                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id                            
//                            INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable
//                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND a.FechaBaja is null " + lineaWhere + @"
//                            and a.Id =" + idArea;
//                        IQuery query2 = GetSession().CreateSQLQuery(sql2);
//                        List<int> idsRequerimientos = query2.List<int>().ToList();
//                        resultadoEstadisticaArea.IdsRequerimientos = idsRequerimientos;
//                    }

//                    /*aca se hace el tratamiento de las areas que tienen hijas*/
//                    /*recorro las areas que me vienen de filtro*/
                    
//                    foreach (Consulta_AreaConSubarea area in consulta.Areas)
//                    {
//                        /*Aca voy a obtener los ids de mis areas, que son los que me interesan (sin el padre)*/
                        

                      
//                        /*si no tiene hijas no hago anda, continuo al siguiente*/
//                        if (area.IdsHijos == null || area.IdsHijos.Count == 0)
//                        {
//                            listaAreasLimpia.Add(area.Id);
//                            continue;
//                        }
//                        listaAreasLimpia.AddRange(area.IdsHijos);
                        

//                        /*Aca saco del data el area que tiene hijas (en el data pueden estar las hijas pero no el padre)*/
//                        Resultado_DatosEstadisticaArea areaResumida;
//                        var areaPrincipal = data.SingleOrDefault(x => x.IdArea == area.Id);
                        
//                        //para saber si es principal o hija
//                        bool esPrincipal;
//                        if (areaPrincipal != null)
//                        {
//                            areaResumida = areaPrincipal;
//                            esPrincipal = true;
//                        }
//                        //else
//                        //{
//                        //    areaResumida = data.FirstOrDefault(x => area.IdsHijos.Contains(x.IdArea));
//                        //    esPrincipal = false;
//                        //}
//                        /*veo si hay algun hijo*/
//                    }
//                }


//                /*Armo el data final con todas menos mis areas*/

//                List<Resultado_DatosEstadisticaArea> dataFinal = new List<Resultado_DatosEstadisticaArea>();
//                if (resultado != null && resultado.Count != 0)
//                {
//                    /*Armo el dataFinal porque se necesitan todas las otras areas en una sola fila*/
//                    /*Primero agrego todas las filas menos las de las otras areas*/

//                    foreach (int? idAreaLimpia in listaAreasLimpia )
//                    {
//                        dataFinal.AddRange(data.Where(x => x.IdArea == idAreaLimpia));
//                    }


//                    /*Armo la ultima fila con la suma de los porcentajes y la suma de las cantidades y se las seteo a la fila nueva*/
//                    var listaOtros = data.Except(dataFinal).ToList<Resultado_DatosEstadisticaArea>();//no se se si va asi o al revez

//                    var porcentaje = Math.Round(listaOtros.Select(x => x.Porcentaje).Sum(), 2);

//                    var cantidad = listaOtros.Select(x => x.Cantidad).Sum();


//                    var idsrequerimientos = new List<int>();
//                    listaOtros.Select(x => x.IdsRequerimientos).ToList().ForEach(x => { idsrequerimientos.AddRange(x); });


//                    dataFinal.Add(new Resultado_DatosEstadisticaArea()
//                    {
//                        Area = "Otras",
//                        Cantidad = cantidad,
//                        Porcentaje = porcentaje,
//                        IdsRequerimientos = idsrequerimientos
//                    });

//                }

//                result.Return = dataFinal;
//                return result;

//            }
//            catch (Exception e)
//            {
//                result.AddErrorInterno(e);
//            }

//            return result;
//        }

        public Result<List<Resultado_DatosEstadisticaArea>> GetDatosEstadisticaArea(Consulta_EstadisticaArea consulta)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaArea>>();

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }

            var año = consulta.Año;
            var mes = consulta.Mes;
            var lineaWhere = "";


            if (año != null && mes != null)
            {
                lineaWhere = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                lineaWhere = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
            }

            List<int> est = new List<int>();
            consulta.EstadosKeyValue.ForEach(x => est.Add((int)x));

            var idServicio = consulta.IdServicio;


            try
            {
                var sql = @"

                            SELECT a.Nombre AS Area, a.Id as IdArea, COUNT (*) AS Cantidad,
                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
				            (
				            SELECT TOP 1 id
				            FROM EstadoRequerimientoHistorial erh1 
				            WHERE erh1.IdRequerimiento = r.Id 
				            ORDER BY erh1.Fecha desc
				            )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")

                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id
                            INNER JOIN dbo.Tema as t on m.IdTema = t.Id
                            INNER JOIN dbo.Servicio as s on t.IdServicio = s.Id
                            INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable                            
                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND s.Id = " + idServicio + " AND a.FechaBaja is null " + lineaWhere + @"
  
                          
                            GROUP BY a.Id, a.Nombre
                            ORDER BY count(*) DESC";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaArea)));
                var resultado = query.List<Resultado_DatosEstadisticaArea>().ToList();
                
                result.Return = resultado;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        public Result<List<Resultado_DatosEstadisticaArea>> GetDatosEstadisticaSubArea(Consulta_EstadisticaSubArea consulta)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaArea>>();

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }

            var año = consulta.Año;
            var mes = consulta.Mes;
            var lineaWhere = "";


            if (año != null && mes != null)
            {
                lineaWhere = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                lineaWhere = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
            }

            List<int> est = new List<int>();
            consulta.EstadosKeyValue.ForEach(x => est.Add((int)x));


            try
            {
                var sql = @"

                            SELECT a.Nombre AS Area, a.Id as IdArea, COUNT (*) AS Cantidad,
                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
				            (
				            SELECT TOP 1 id
				            FROM EstadoRequerimientoHistorial erh1 
				            WHERE erh1.IdRequerimiento = r.Id 
				            ORDER BY erh1.Fecha desc
				            )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")

                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id
                            INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable                            
                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND a.FechaBaja is null " + lineaWhere + @"
                            and a.Id in (" + string.Join(", ", consulta.idsAreas) + @")
  
                          
                            GROUP BY a.Id, a.Nombre
                            ORDER BY count(*) DESC";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaArea)));
                var resultado = query.List<Resultado_DatosEstadisticaArea>().ToList();
                var data = new List<Resultado_DatosEstadisticaArea>();

                
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);
                    /*al data le agrefo los ids de los rqs*/
                    foreach (var resultadoEstadisticaArea in data)
                    {
                        int idArea = resultadoEstadisticaArea.IdArea;
                        var sql2 = @"
                            SELECT r.Id                                           
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
				            (
				            SELECT TOP 1 id
				            FROM EstadoRequerimientoHistorial erh1 
				            WHERE erh1.IdRequerimiento = r.Id 
				            ORDER BY erh1.Fecha desc
				            )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")
                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id                            
                            INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable
                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND a.FechaBaja is null " + lineaWhere + @"
                            and a.Id =" + idArea;
                        IQuery query2 = GetSession().CreateSQLQuery(sql2);
                        List<int> idsRequerimientos = query2.List<int>().ToList();
                        resultadoEstadisticaArea.IdsRequerimientos = idsRequerimientos;
                    }
                }
              

                result.Return = data;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        /*backup metodo*/
//        public Result<List<Resultado_DatosEstadisticaArea>> GetDatosEstadisticaArea(Consulta_EstadisticaArea consulta)
//        {
//            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
//            var result = new Result<List<Resultado_DatosEstadisticaArea>>();

//            var fechaDesdeString = "";
//            var fechaHastaString = "";
//            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
//            {
//                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
//                /*Agrego hasta el final del dia*/
//                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
//            }

//            var año = consulta.Año;
//            var mes = consulta.Mes;
//            var lineaWhere = "";


//            if (año != null && mes != null)
//            {
//                lineaWhere = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
//            }
//            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
//            {
//                lineaWhere = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
//            }

//            List<int> est = new List<int>();
//            consulta.EstadosKeyValue.ForEach(x => est.Add((int)x));


//            try
//            {
//                var sql = @"
//
//                            SELECT a.Nombre AS Area, a.Id as IdArea, COUNT (*) AS Cantidad,
//                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje
//                            FROM
//                            dbo.Requerimiento AS r
//                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
//				            (
//				            SELECT TOP 1 id
//				            FROM EstadoRequerimientoHistorial erh1 
//				            WHERE erh1.IdRequerimiento = r.Id 
//				            ORDER BY erh1.Fecha desc
//				            )
//                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")
//
//                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id
//                            INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable                            
//                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND a.FechaBaja is null " + lineaWhere + @"
//  
//                          
//                            GROUP BY a.Id, a.Nombre
//                            ORDER BY count(*) DESC";

//                IQuery query = GetSession().CreateSQLQuery(sql);
//                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaArea)));
//                var resultado = query.List<Resultado_DatosEstadisticaArea>().ToList();
//                var data = new List<Resultado_DatosEstadisticaArea>();

//                if (resultado != null && resultado.Count != 0)
//                {
//                    data.AddRange(resultado);
//                    /*al data le agrefo los ids de los rqs*/
//                    foreach (var resultadoEstadisticaArea in data)
//                    {
//                        int idArea = resultadoEstadisticaArea.IdArea;
//                        var sql2 = @"
//                            SELECT r.Id                                           
//                            FROM
//                            dbo.Requerimiento AS r
//                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
//				            (
//				            SELECT TOP 1 id
//				            FROM EstadoRequerimientoHistorial erh1 
//				            WHERE erh1.IdRequerimiento = r.Id 
//				            ORDER BY erh1.Fecha desc
//				            )
//                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")
//                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id                            
//                            INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable
//                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND a.FechaBaja is null " + lineaWhere + @"
//                            and a.Id =" + idArea;
//                        IQuery query2 = GetSession().CreateSQLQuery(sql2);
//                        List<int> idsRequerimientos = query2.List<int>().ToList();
//                        resultadoEstadisticaArea.IdsRequerimientos = idsRequerimientos;
//                    }

//                    /*aca se hace el tratamiento de las areas que tienen hijas*/
//                    /*recorro las areas que me vienen de filtro*/
//                    foreach (Consulta_AreaConSubarea area in consulta.Areas)
//                    {
//                        /*si no tiene hijas no hago anda, continuo al siguiente*/
//                        if (area.IdsHijos == null || area.IdsHijos.Count == 0)
//                        {
//                            continue;
//                        }

//                        /*Aca saco del data el area que tiene hijas (en el data pueden estar las hijas pero no el padre)*/
//                        Resultado_DatosEstadisticaArea areaResumida;
//                        var areaPrincipal = data.SingleOrDefault(x => x.IdArea == area.Id);
//                        //para saber si es principal o hija
//                        bool esPrincipal;
//                        if (areaPrincipal != null)
//                        {
//                            areaResumida = areaPrincipal;
//                            esPrincipal = true;
//                        }
//                        else
//                        {
//                            areaResumida = data.FirstOrDefault(x => area.IdsHijos.Contains(x.IdArea));
//                            esPrincipal = false;
//                        }
//                        /*veo si hay algun hijo*/



//                        /*inicializo las variables del objeto*/
//                        var cantidad = 0;
//                        var porcentaje = 0.0;
//                        var idsrequerimientos = new List<int>();
//                        var areaNombre = "";
//                        var idArea = 0;
//                        if (!esPrincipal)
//                        {
//                            var resultArea = CerrojoAreaDAO.Instance.GetById(area.Id.Value);
//                            if (resultArea.Return == null || !resultArea.Ok)
//                            {
//                                result.AddErrorInterno("Error consultando");
//                                return result;
//                            }
//                            areaNombre = resultArea.Return.Nombre;
//                            idArea = area.Id.Value;
//                        }


//                        var listaDeAreasRemove = new List<int>();
//                        if (areaResumida != null && esPrincipal)
//                        {
//                            /*armo el nuevo objeto inicializando en los valores que tiene el padre por si tiene reclamos , cosa q no deberia pasar */
//                            cantidad = areaResumida.Cantidad;
//                            porcentaje = areaResumida.Porcentaje;
//                            idsrequerimientos.AddRange(areaResumida.IdsRequerimientos);
//                            areaNombre = areaResumida.Area;
//                            idArea = areaResumida.IdArea;
//                            /*armo una lista de las areas a eliminar*/
//                            listaDeAreasRemove = new List<int>();
//                            listaDeAreasRemove.Add(areaResumida.IdArea);
//                        }


//                        /*si tiene hijas voy a eliminar del data esa subarea y voty a resumir enuna sola area*/
//                        foreach (int idAreaHija in area.IdsHijos)
//                        {
//                            //le sumas al objeto del padre las cantidades, etc. El single devuelve un solo valor
//                            var areaHija = data.SingleOrDefault(x => x.IdArea == idAreaHija);
//                            if (areaHija == null)
//                            {
//                                continue;
//                            }

//                            cantidad += areaHija.Cantidad;
//                            porcentaje += areaHija.Porcentaje;
//                            idsrequerimientos.AddRange(areaHija.IdsRequerimientos);
//                            listaDeAreasRemove.Add(idAreaHija);
//                            /*elimino del data el area hija*/
//                            //data = data.Where(x=>x.IdArea!=idAreaHija).ToList();
//                        }
//                        /*elimino del data el area con sus hijas para agtregar el nuevo onjeto resumido*/
//                        data = data.Where(x => !listaDeAreasRemove.Contains(x.IdArea)).ToList();
//                        /*agrego el objeto resumido*/
//                        data.Add(new Resultado_DatosEstadisticaArea()
//                        {
//                            Area = areaNombre,
//                            Cantidad = cantidad,
//                            Porcentaje = Math.Round(porcentaje, 2),
//                            IdArea = idArea,
//                            IdsRequerimientos = idsrequerimientos
//                        });
//                    }
//                }


//                /*Armo el data final con todas menos mis areas*/

//                List<Resultado_DatosEstadisticaArea> dataFinal = new List<Resultado_DatosEstadisticaArea>();
//                if (resultado != null && resultado.Count != 0)
//                {
//                    /*Armo el dataFinal porque se necesitan todas las otras areas en una sola fila*/
//                    /*Primero agrego todas las filas menos las de las otras areas*/

//                    foreach (Consulta_AreaConSubarea area in consulta.Areas)
//                    {
//                        dataFinal.AddRange(data.Where(x => x.IdArea == area.Id));

//                        /*if (area.IdsHijos == null || area.IdsHijos.Count == 0)
//                        {
//                            continue;
//                        }

//                        foreach (int idAreaHija in area.IdsHijos)
//                        {
//                            dataFinal.AddRange(data.Where(x => x.IdArea == idAreaHija));
//                        }*/
//                    }



//                    /*Armo la ultima fila con la suma de los porcentajes y la suma de las cantidades y se las seteo a la fila nueva*/
//                    var listaOtros = data.Except(dataFinal).ToList<Resultado_DatosEstadisticaArea>();//no se se si va asi o al revez

//                    var porcentaje = Math.Round(listaOtros.Select(x => x.Porcentaje).Sum(), 2);

//                    var cantidad = listaOtros.Select(x => x.Cantidad).Sum();


//                    var idsrequerimientos = new List<int>();
//                    listaOtros.Select(x => x.IdsRequerimientos).ToList().ForEach(x => { idsrequerimientos.AddRange(x); });


//                    dataFinal.Add(new Resultado_DatosEstadisticaArea()
//                    {
//                        Area = "Otras",
//                        Cantidad = cantidad,
//                        Porcentaje = porcentaje,
//                        IdsRequerimientos = idsrequerimientos
//                    });

//                }

//                result.Return = dataFinal;
//                return result;

//            }
//            catch (Exception e)
//            {
//                result.AddErrorInterno(e);
//            }

//            return result;
//        }
        /*Estos tienen filtro por estado*/
        public Result<List<Resultado_DatosEstadisticaMotivos>> GetDatosEstadisticaMotivos(Consulta_EstadisticaMotivos consulta)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaMotivos>>();
           

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }


            //var IdsArea = consulta.IdsArea;
            var año = consulta.Año;
            var mes = consulta.Mes;


            /*Esto no va porque los Motivos q1ue  fueron inspeccionados en tal feha, no los que se crearon en esa tal fecha*/
            var lineaWhere = "";


            if (año != null && mes != null)
            {
                lineaWhere = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                lineaWhere = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
            }

            List<int> est = new List<int>();
            consulta.EstadosKeyValue.ForEach(x => est.Add((int)x));

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




            try
            {
                var sql = @"
                Select m.Id as IdMotivo, m.Nombre as Motivo, count(*) as Cantidad,
                ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje
                from Requerimiento r 
                INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
				(
				SELECT TOP 1 id
				FROM EstadoRequerimientoHistorial erh1 
				WHERE erh1.IdRequerimiento = r.Id 
				ORDER BY erh1.Fecha desc
				)
                INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")
                INNER JOIN Motivo m on m.Id = r.IdMotivo
                INNER JOIN CerrojoArea a on a.Id = r.IdAreaCerrojoResponsable
                WHERE m.FechaBaja is null
                AND a.FechaBaja is null        
                AND r.FechaBaja is null  " + lineaWhere + @"
                AND a.Id in (" + string.Join(", ", idsAreas) + @")
                group by m.Id, m.Nombre
                order by count(*) desc
                ";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaMotivos)));
                var resultado = query.List<Resultado_DatosEstadisticaMotivos>().ToList();
                var data = new List<Resultado_DatosEstadisticaMotivos>();



                List<Resultado_DatosEstadisticaMotivos> dataIntermedio = new List<Resultado_DatosEstadisticaMotivos>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);


                    foreach (var resultData in data)
                    {
                        Resultado_DatosEstadisticaMotivos nuevoResult = new Resultado_DatosEstadisticaMotivos();
                        nuevoResult.Cantidad = resultData.Cantidad;
                        nuevoResult.IdMotivo = resultData.IdMotivo;
                        nuevoResult.Motivo = resultData.Motivo;
                        nuevoResult.Porcentaje = resultData.Porcentaje;

                        int idMotivo = resultData.IdMotivo;

                        var sql2 = @"


                            SELECT r.Id                                           
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
				            (
				                SELECT TOP 1 id
				                FROM EstadoRequerimientoHistorial erh1 
				                WHERE erh1.IdRequerimiento = r.Id 
				                ORDER BY erh1.Fecha desc
				             )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")

                            INNER JOIN dbo.Motivo AS m ON r.IdMotivo = m.Id
                            INNER JOIN CerrojoArea a ON a.Id = r.IdAreaCerrojoResponsable
                            WHERE m.FechaBaja IS null AND r.FechaBaja IS null AND a.FechaBaja is null " + lineaWhere + @"
                            and m.Id =" + idMotivo;



                        IQuery query2 = GetSession().CreateSQLQuery(sql2);

                        List<int> idsRequerimientos = query2.List<int>().ToList();

                        nuevoResult.IdsRequerimientos = idsRequerimientos;

                        dataIntermedio.Add(nuevoResult);
                    }
                }


                List<Resultado_DatosEstadisticaMotivos> dataFinal = new List<Resultado_DatosEstadisticaMotivos>();
                if (resultado != null && resultado.Count != 0)
                {

                    /*Armo el dataFinal porque se necesitan todos los CPC en una sola fila*/
                    /*Primero agrego todas las filas menos las del cpc*/
                    dataFinal.AddRange(dataIntermedio.Take(9));
                    /*Armo la ultima fila con la suma de los porcentajes y la suma de las cantidades y se las seteo a la fila nueva*/
                    var listaOtros = dataIntermedio.Except(dataFinal).ToList<Resultado_DatosEstadisticaMotivos>();//no se se si va asi o al revez

                    var porcentaje = Math.Round(listaOtros.Select(x => x.Porcentaje).Sum(), 2);
                    var cantidad = listaOtros.Select(x => x.Cantidad).Sum();

                    var idsrequerimientos = new List<int>();
                    listaOtros.Select(x => x.IdsRequerimientos).ToList().ForEach(x => { idsrequerimientos.AddRange(x); });


                    dataFinal.Add(new Resultado_DatosEstadisticaMotivos()
                    {
                        Motivo = "Otros",
                        Cantidad = cantidad,
                        Porcentaje = porcentaje,
                        IdsRequerimientos = idsrequerimientos
                    });

                }

                result.Return = dataFinal;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }
        public Result<List<Resultado_DatosEstadisticaUsuario>> GetDatosEstadisticaUsuario(Consulta_EstadisticaUsuario consulta)
        {
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaUsuario>>();

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }

            var año = consulta.Año;
            var mes = consulta.Mes;
            //var IdsArea = consulta.IdsArea;

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
            //var lineaWhere = "";


            //if (año != null && mes != null)
            //{
            //    lineaWhere = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
            //}
            //if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            //{
            //    lineaWhere = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
            //}

            var lineaWhere1 = "";

            if (año != null && mes != null)
            {

                lineaWhere1 = "and month(erh1.Fecha)= " + mes + " AND year(erh1.Fecha) = " + año + @"";

            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                lineaWhere1 = "and erh1.Fecha BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";

            }

            List<int> est = new List<int>();
            consulta.EstadosKeyValue.ForEach(x => est.Add((int)x));            

            try
            {
                var sql = @"
                            SELECT top 20 vvu.Username as Usuario, vvu.Id as IdUsuario, count(*) as Cantidad,
                            ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje 
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
				            (
				            SELECT TOP 1 id
				            FROM EstadoRequerimientoHistorial erh1 
				            WHERE erh1.IdRequerimiento = r.Id " + lineaWhere1 + @"  
				            ORDER BY erh1.Fecha desc
				            )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")
                            INNER JOIN VecinoVirtualUsuario vvu on r.IdUsuarioCerrojoCreador = vvu.Id                            
                            INNER JOIN CerrojoArea a on a.Id = r.IdAreaCerrojoResponsable

                            WHERE r.FechaBaja is null and vvu.FechaBaja is NULL
                            AND a.Id in (" + string.Join(", ", idsAreas) + @")


                            GROUP BY vvu.Username, vvu.Id
                            ORDER BY count(*) DESC";

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaUsuario)));
                var resultado = query.List<Resultado_DatosEstadisticaUsuario>().ToList();
                var data = new List<Resultado_DatosEstadisticaUsuario>();



                List<Resultado_DatosEstadisticaUsuario> dataFinal = new List<Resultado_DatosEstadisticaUsuario>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);


                    foreach (var resultadoEstadisticaUsuario in data)
                    {
                        Resultado_DatosEstadisticaUsuario nuevoResult = new Resultado_DatosEstadisticaUsuario();
                        nuevoResult.Cantidad = resultadoEstadisticaUsuario.Cantidad;
                        nuevoResult.IdUsuario = resultadoEstadisticaUsuario.IdUsuario;
                        nuevoResult.Usuario = resultadoEstadisticaUsuario.Usuario;
                        nuevoResult.Porcentaje = resultadoEstadisticaUsuario.Porcentaje;

                        int idUsuario = resultadoEstadisticaUsuario.IdUsuario;

                        var sql2 = @"

	                        SELECT r.Id                                           
                            FROM
                            dbo.Requerimiento AS r
                            INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
				            (
				                SELECT TOP 1 id
				                FROM EstadoRequerimientoHistorial erh1 
				                WHERE erh1.IdRequerimiento = r.Id " + lineaWhere1 + @"  
				                ORDER BY erh1.Fecha desc
				             )
                            INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")
                            INNER JOIN dbo.VecinoVirtualUsuario AS vvu ON r.IdUsuarioCerrojoCreador = vvu.Id                        
                            WHERE r.FechaBaja IS null AND vvu.FechaBaja IS null 
                            and vvu.Id =" + idUsuario;


                        IQuery query2 = GetSession().CreateSQLQuery(sql2);

                        List<int> idsRequerimientos = query2.List<int>().ToList();

                        nuevoResult.IdsRequerimientos = idsRequerimientos;

                        dataFinal.Add(nuevoResult);
                    }
                }


                result.Return = dataFinal;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;

        }
        public Result<List<Resultado_DatosEstadisticaZona>> GetDatosEstadisticaZona(Consulta_EstadisticaZona consulta)
        {
          
            /*En esta estadistica uso una nueva forma, seteando la lista que devuelve la query al objeto resultado*/
            var result = new Result<List<Resultado_DatosEstadisticaZona>>();

            var fechaDesdeString = "";
            var fechaHastaString = "";
            if (consulta.FechaDesde.HasValue && consulta.FechaHasta.HasValue)
            {
                fechaDesdeString = consulta.FechaDesde.Value.ToString(Utils.SQLDATETIME_FORMAT);
                /*Agrego hasta el final del dia*/
                fechaHastaString = consulta.FechaHasta.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString(Utils.SQLDATETIME_FORMAT);
            }


            var idArea = consulta.IdArea;
            var idCategoria = consulta.IdCategoria;
            var año = consulta.Año;
            var mes = consulta.Mes;
            //var lineaWhere = "";


            //if (año != null && mes != null)
            //{
            //    lineaWhere = "and month(r.FechaAlta)=" + mes + "AND year(r.FechaAlta) =" + año + @"";
            //}
            //if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            //{
            //    lineaWhere = "and r.FechaAlta BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";
            //}

            var lineaWhere1 = "";

            if (año != null && mes != null)
            {

                lineaWhere1 = "and month(erh1.Fecha)=" + mes + "AND year(erh1.Fecha) =" + año + @"";

            }
            if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
            {
                lineaWhere1 = "and erh1.Fecha BETWEEN '" + fechaDesdeString + @"' AND '" + fechaHastaString + @"'";

            }


            List<int> est = new List<int>();
            consulta.EstadosKeyValue.ForEach(x => est.Add((int)x));

            try
            {
                var sql = @"

                SELECT
                z.Id as IdZona, z.Nombre as Zona,
                Count(*) AS Cantidad,
                ROUND( CAST (count(*) AS FLOAT) * 100.0 / sum(count(*)) over(),2) AS Porcentaje

                FROM
                dbo.Requerimiento AS r
                INNER JOIN EstadoRequerimientoHistorial AS erh ON erh.Id = 
				(
				SELECT TOP 1 id
				FROM EstadoRequerimientoHistorial erh1 
				WHERE erh1.IdRequerimiento = r.Id " + lineaWhere1 + @"  
				ORDER BY erh1.Fecha desc
				)
                INNER JOIN dbo.EstadoRequerimiento AS er ON erh.IdEstado = er.Id AND er.KeyValue in (" + string.Join(", ", est) + @")


                INNER JOIN dbo.Motivo AS m ON m.Id = r.IdMotivo                                 
                INNER JOIN dbo.CerrojoArea AS a ON a.Id = r.IdAreaCerrojoResponsable
                INNER JOIN dbo.Domicilio AS d ON d.Id = r.IdDomicilio
                INNER JOIN dbo.Barrio AS b ON b.Id = d.IdBarrio ,
                Zona AS z
                INNER JOIN BarrioPorZona AS bxz ON bxz.IdZona = z.Id
                WHERE
                r.FechaBaja IS null
                and bxz.FechaBaja is null	
                AND z.IdAreaCerrojo = " + idArea + @"
                AND a.Id = " + idArea;
                if (idCategoria.HasValue)
                {
                    sql += @" and m.IdCategoriaMotivoArea = " + idCategoria;
                }
                sql += @" 
                AND z.FechaBaja is null
                /*Esto se lo puse aca porque en el inner join me daba error*/
                and bxz.IdBarrio = b.Id 
                GROUP BY
                z.Id,
                z.Nombre
                ORDER BY
                count(*) DESC";
                
     

                IQuery query = GetSession().CreateSQLQuery(sql);
                query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(Resultado_DatosEstadisticaZona)));
                var resultado = query.List<Resultado_DatosEstadisticaZona>().ToList();
                var data = new List<Resultado_DatosEstadisticaZona>();



                List<Resultado_DatosEstadisticaZona> dataIntermedio = new List<Resultado_DatosEstadisticaZona>();
                if (resultado != null && resultado.Count != 0)
                {
                    data.AddRange(resultado);


                    foreach (var resultv in data)
                    {
                        Resultado_DatosEstadisticaZona nuevoResult = new Resultado_DatosEstadisticaZona();
                        var idZona = resultv.IdZona;
                        nuevoResult.Cantidad = resultv.Cantidad;
                        nuevoResult.IdZona = idZona;
                        nuevoResult.Zona = resultv.Zona;
                        nuevoResult.Porcentaje = resultv.Porcentaje;

                        /*creo la consulta*/
                        var consultaRequerimiento = new Consulta_Requerimiento();
                        /*creo una lista para mandarle a al consulta*/
                        var listaZonas = new List<int>();
                        listaZonas.Add(idZona);
                        /*buisco todos los datos para simular el fitrlo y obtener los ids de los rqs. Necesito la zon el servicio y el area */


                        var resultZona = ZonaDAO.Instance.GetById(idZona);
                        if (resultZona.Return == null || !resultZona.Ok)
                        {
                            result.AddErrorInterno("Error consultando");
                            return result;
                        }
                        var idAreaDeZona = resultZona.Return.Area.Id;

                        var resultArea = CerrojoAreaDAO.Instance.GetById(idAreaDeZona);
                        if (resultArea.Return == null || !resultArea.Ok)
                        {
                            result.AddErrorInterno("Error consultando");
                            return result;
                        }
                        var idAreaReferente = resultArea.Return.Id;

                        var listaAreas = new List<int>();
                        listaAreas.Add(idArea);

                        var listaCategorias = new List<int>();
                        if (idCategoria.HasValue && idCategoria.Value != -1)
                        {
                            
                            listaCategorias.Add(idCategoria.Value);
                        }
                        

                        var resultServicio = MotivoDAO.Instance.GetServicioByIdArea(idAreaReferente);
                        if (resultServicio.Return == null || !resultServicio.Ok)
                        {
                            result.AddErrorInterno("Error consultando");
                            return result;
                        }
                        var idServicio = resultServicio.Return.Id;
                        var listaServicio = new List<int>();
                        listaServicio.Add(idServicio);



                        consultaRequerimiento.IdsZona = listaZonas;
                        consultaRequerimiento.IdsArea = listaAreas;
                        consultaRequerimiento.IdsServicio = listaServicio;
                        consultaRequerimiento.IdsCategoria = listaCategorias;

                        if (año != null && mes != null)
                        {
                            consultaRequerimiento.Mes = mes;
                            consultaRequerimiento.AñoDeMes = año;
                        }
                        if (!string.IsNullOrEmpty(fechaDesdeString) && !string.IsNullOrEmpty(fechaHastaString))
                        {
                            consultaRequerimiento.FechaDesde = consulta.FechaDesde;
                            consultaRequerimiento.FechaHasta = consulta.FechaHasta;
                        }

                        consultaRequerimiento.EstadosKeyValue = consulta.EstadosKeyValue;


                        var resultIds = RequerimientoDAO.Instance.GetIdsByFilters(consultaRequerimiento, null);
                        if (resultIds.Return == null || !resultIds.Ok)
                        {
                            result.AddErrorInterno("Error consultando");
                            return result;
                        }
                        var idsRequerimientos = resultIds.Return;
                        var listaIds = new List<int>();
                        listaIds = idsRequerimientos;
                        nuevoResult.IdsRequerimientos = listaIds;
                        dataIntermedio.Add(nuevoResult);
                    }
                }

                result.Return = dataIntermedio;
                return result;

            }
            catch (Exception e)
            {
                result.AddErrorInterno(e);
            }

            return result;
        }

        #endregion

        /*ESTO ES PARA UNA MIGRACION DE USUARIOS VALIDADOS*/
//        public bool migrarUsuariosTestPaso1()
//        {            
//            //lista de dnis que tienen un usuario validado y a su vez tienen uno o varios sin validar;
//            var lista = new List<int>();

//            var sql = @"
//            SELECT
//	        Dni
//            FROM
//	        cerrojo.dbo.Usuario
//            WHERE
//	        FechaBaja IS NULL
//            AND FechaValidacionRenaper IS NULL /*No esta validado */
//            AND Dni IS NOT NULL /*CON DNI*/
//            AND Dni IN (
//	        SELECT
//		    u.Dni
//	        FROM
//		    cerrojo.dbo.Usuario u
//	        WHERE
//		    u.FechaBaja IS NULL
//	        AND u.FechaValidacionRenaper IS NOT NULL
//            )
//            GROUP BY Dni
//            order by count(*) desc";

//            IQuery query = GetSession().CreateSQLQuery(sql);
//            var data = query.List<int>().ToList();
//            lista.AddRange(data);

//            /*por cada uno de esta lista busco el validado y se lo pego a la tabla de los rqs */
//            var B = Transaction(() =>
//            {
//                try
//                {

//                    IQuery query2 = null;
//                    IQuery query3 = null;
//                    IQuery query4 = null;
//                    IQuery query5 = null;

//                    foreach (int dniabuscar in lista)
//                    {
//                        var sql2 = @"
//                    select Id
//                    from cerrojo.dbo.Usuario u
//                    where u.FechaValidacionRenaper is not NULL
//                    and u.Dni in (" + dniabuscar + @")";

//                        query2 = GetSession().CreateSQLQuery(sql2);
//                        var data2 = query2.List<int>().ToList();
//                        var idValidado = data2;

//                        /*buscar los reclamos que tienen el dniabuscar y hacer el Update pegandole el idvalido */

//                        //este es para validar que no venga solo
//                        var sql3 = @"
//                    SELECT r.Id
//                    FROM
//	                Requerimiento r
//                    INNER JOIN UsuarioReferentePorRequerimiento ux ON r.Id = ux.IdRequerimiento
//                    INNER JOIN cerrojo.dbo.Usuario u ON u.Id = ux.IdUsuarioReferente
//                    WHERE
//	                r.FechaBaja IS NULL
//                    and u.Dni in (" + dniabuscar + @")
//                    and u.FechaValidacionRenaper is null";

//                        query3 = GetSession().CreateSQLQuery(sql3);
//                        var data3 = query3.List<int>().ToList();
//                        var cantidadrq = data3;

//                        /*si hay rq los actualizo*/
//                        if (cantidadrq.Count > 0)
//                        {
//                            var id = idValidado[0];
//                            /*el update de los rqs*/
//                            var sql4 = @"
//                        UPDATE UsuarioReferentePorRequerimiento
//                        SET IdUsuarioReferente = " + id + @"
//                        , FechaModificacion = CAST('2019-02-27' AS DATETIME)
//                        WHERE
//	                    IdRequerimiento IN (
//		                SELECT
//			            r.Id
//		                FROM
//			            Requerimiento r
//		                INNER JOIN UsuarioReferentePorRequerimiento ux ON r.Id = ux.IdRequerimiento
//		                INNER JOIN cerrojo.dbo.Usuario u ON u.Id = ux.IdUsuarioReferente
//		                WHERE
//			            r.FechaBaja IS NULL
//		                AND u.Dni IN (" + dniabuscar + @")
//		                AND u.FechaValidacionRenaper IS NULL
//	                    )";

//                            query4 = GetSession().CreateSQLQuery(sql4);
//                            var cantidad = query4.ExecuteUpdate();

//                            System.Diagnostics.Debug.WriteLine("Update 1 Filas afectadas: "+ cantidad + " DNI: "+ dniabuscar + "");
//                        }

//                        /**dar de baja los usuarios que no estan validados*/
//                        var sql5 = @"
//                    UPDATE cerrojo.dbo.Usuario
//                    SET FechaBaja = CAST('2019-02-27' AS DATETIME)
//                    WHERE Id in (
//                    select Id
//                    from cerrojo.dbo.Usuario u
//                    where u.FechaValidacionRenaper IS NULL
//                    AND u.Dni IN (" + dniabuscar + @"))";

//                        query5 = GetSession().CreateSQLQuery(sql5);
//                        var cantidad2 = query5.ExecuteUpdate();
//                        System.Diagnostics.Debug.WriteLine("Update 2 Filas afectadas: " + cantidad2 + " DNI: " + dniabuscar + "");

//                    }
//                    return true;

//                }
//                catch (Exception e)
//                {
//                    return false;
//                }
//            });
//            return B;
//        }
//        public bool migrarUsuariosTestPaso2()
//        {
//            //lista de ids que tienen menos de 10 rqs y que no estan validados
//            var lista = new List<int>();

//            var sql = @"
//                SELECT u.Id
//                FROM cerrojo.dbo.Usuario u 
//                LEFT JOIN UsuarioReferentePorRequerimiento ux ON u.Id = ux.IdUsuarioReferente 
//                LEFT JOIN Requerimiento r on ux.IdRequerimiento = r.Id
//                WHERE u.FechaValidacionRenaper IS NULL and
//                r.FechaBaja is null and
//                u.FechaBaja is null 
//                GROUP BY u.Dni, u.Id
//                HAVING count(ux.IdUsuarioReferente)<10
//                ORDER BY u.Dni desc";

//            IQuery query = GetSession().CreateSQLQuery(sql);
//            var data = query.List<int>().ToList();
//            lista.AddRange(data);

//            /*por cada uno de esta lista busco el validado y se lo pego a la tabla de los rqs */
//            var B = Transaction(() =>
//            {
//                try
//                {

//                    IQuery query2 = null;
//                    IQuery query3 = null;
//                    IQuery query4 = null;
//                    IQuery query5 = null;

//                    foreach (int idabuscar in lista)
//                    {
//                        /*Se le pasan los rqs al id de funes*/
//                        var idValido = 340;

//                        /*buscar los reclamos que tienen el dniabuscar y hacer el Update pegandole el idvalido */

//                        //este es para validar que no venga solo
//                        var sql3 = @"
//                        select count(*)
//                        from Requerimiento r inner join UsuarioReferentePorRequerimiento ux on r.Id = ux.IdRequerimiento
//                        where ux.IdUsuarioReferente in ("+ idabuscar +@")
//                        and r.FechaBaja is NULL";

//                        query3 = GetSession().CreateSQLQuery(sql3);
//                        var data3 = query3.List<int>().ToList();
//                        var cantidadrq = data3;

//                        /*si hay rq los actualizo*/
//                        if (cantidadrq.Count > 0)
//                        {
                        
//                            /*el update de los rqs*/
//                            var sql4 = @"
//                        UPDATE UsuarioReferentePorRequerimiento
//                        SET IdUsuarioReferente = " + idValido + @"
//                        WHERE
//	                        Id IN (
//		                 SELECT
//			                ux.Id
//		                FROM
//			            Requerimiento r
//		                INNER JOIN UsuarioReferentePorRequerimiento ux ON r.Id = ux.IdRequerimiento
//		                WHERE
//			            ux.IdUsuarioReferente IN (" + idabuscar + @")
//		                AND r.FechaBaja IS NULL)";

//                            query4 = GetSession().CreateSQLQuery(sql4);
//                            var cantidad = query4.ExecuteUpdate();

//                            System.Diagnostics.Debug.WriteLine("Update 1 Filas afectadas: " + cantidad + " ID: " + idabuscar + "");
//                        }

//                        /**dar de baja los usuarios que no estan validados*/
//                        var sql5 = @"
//                    UPDATE cerrojo.dbo.Usuario
//                    SET FechaBaja = CAST('2019-02-28' AS DATETIME)
//                    WHERE Id in (
//                    select Id
//                    from cerrojo.dbo.Usuario u
//                    where u.FechaValidacionRenaper IS NULL
//                    AND u.Id IN (" + idabuscar + @"))";

//                        query5 = GetSession().CreateSQLQuery(sql5);
//                        var cantidad2 = query5.ExecuteUpdate();
//                        System.Diagnostics.Debug.WriteLine("Update 2 Filas afectadas: " + cantidad2 + " Id: " + idabuscar + "");

//                    }
//                    return true;

//                }
//                catch (Exception e)
//                {
//                    return false;
//                }
//            });
//            return B;
//        }


       


       


    }
}

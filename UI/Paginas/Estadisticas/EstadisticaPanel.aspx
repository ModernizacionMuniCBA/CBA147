<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="EstadisticaPanel.aspx.cs" Inherits="UI.EstadisticaPanel" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/EstadisticaPanel.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.charts.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.theme.fint.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.widgets.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.powercharts.js") %>"></script>


    <div id="contenedorResumen">

        <div id="sectorPromedios">

            <div id="contenedor-fechaActual">
                <label id="lblFechaActual"></label>
            </div>
            <div id="contenedor-fecha">
                <label periodo="1" class="seleccionado">Ultimo 30 dias</label>
                <label periodo="2">Ultimo 3 meses</label>
                <label periodo="3">Ultimo 6 meses</label>
                <%--<label periodo="4">Todo el período</label>--%>
            </div>



            <div id="promedios">

                <div class="card flex-main" id="cardTotales">
                    <div class="contenedor no-margin">

                        <div class="contenedor-main no-margin no-padding">
                            <div class="row full-height">
                                <div class="col s6 l6">
                                    <a class="btn-flat btn-redondo tooltipAyuda waves-effect" style="position: absolute; right: 1rem;"><i class="material-icons colorAyuda">help</i></a>
                                    <div style="display: none">
                                        Estos requerimientos invleyen los siguientes estados: Incompleto, En Proceso, Suspendido, Cancelado, Completado, Inspeccion, Cerrado
                                    </div>
                                    <div class="card flex-main no-margin">

                                        <div class="contenedor">

                                            <label class="totales">Requerimientos</label>


                                            <label id="lblTotalReclamos" class="textoTotales"></label>
                                        </div>
                                    </div>

                                </div>
                                <div class="col s6 l6">
                                    <div class="card flex-main no-margin">
                                        <div class="contenedor">
                                            <label class="totales">Atención</label>
                                            <label id="lblIndice" class="textoTotales"></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row full-height">
                                <div class="col s12 l12">
                                    <div class="contenedor">

                                        <a class="btn-flat btn-redondo tooltipAyuda waves-effect" style="position: absolute; right: 1rem;"><i class="material-icons colorAyuda">help</i></a>
                                        <div style="display: none">
                                            Los colores representan:
                                            -Rojo:
                                            -Naranja:
                                            -Amarillo:
                                            -Verde claro
                                            -Verde Oscuro
                                        </div>


                                        <div class="no-scroll contenedor-main no-padding no-margin flex flex-main direction-vertical">

                                            <div id="chart-container" class="no-scroll flex-main"></div>
                                        </div>


                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



                <div class="card flex-main " id="cardRankingMotivos">
                    <div class="contenedor no-margin">
                        <div class="contenedor-header">
                            <label class="titulo">Ranking de Motivos</label>
                        </div>
                        <div class="contenedor-main no-margin no-padding">
                            <div class="tabla-contenedor flex-main flex direction-vertical">
                                <table id="tablaRankingMotivos"></table>
                            </div>
                        </div>
                        <div class="contenedor-footer">
                            <div class="tabla-footer"></div>
                        </div>
                    </div>
                </div>
                <div class="card flex-main" id="cardRadial">
                    <div class="contenedor no-margin">
                        <div class="contenedor-main no-margin no-padding">
                            <div class="row full-height">
                                <div class="col s12 l12">
                                    <div class="card flex-main no-margin">
                                        <div class="contenedor">
                                            <label class="totales">Tiempo promedio de resolución</label>
                                            <label id="lblPromedioAtencion" class="textoTotales"></label>
                                            <a class="btn-flat btn-redondo tooltipAyuda waves-effect" style="position: absolute; right: 1rem;"><i class="material-icons colorAyuda">help</i></a>
                                            <div style="display: none">
                                                Es el tiempo que passa entre que se crea el requerimiento y que es resuelto
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row full-height">
                                <div class="col s12 l12">
                                    <div class="contenedor">
                                        <div class=" contenedor-main no-padding no-margin flex flex-main direction-vertical" id="contenedorGrafico">
                                            <div id="chart-container-radial" class=" flex-main"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="card flex-main no-margin" id="cardCriticidadServicio">
                    <div class="contenedor no-margin">
                        <div class="contenedor-header">
                            <label class="titulo">Atención</label>
                        </div>
                        <div class="contenedor-main no-margin no-padding">
                            <div class="tabla-contenedor flex-main flex direction-vertical">
                                <table id="tablaCriticidadServicios_Colores"></table>
                            </div>
                        </div>
                        <div class="contenedor-footer">
                            <div class="tabla-footer"></div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div id="contenedorMapa">
            <div id="contenedorConsulta" class="card contenedor flex flex-main no-padding no-margin">

                <div class="contenedor-footer flex  direction-vertical no-padding no-margin">
                    <div class="flex direction-vertical">
                        <div id="chart-container2" class="no-scroll flex-main"></div>
                    </div>
                </div>
                <div class="contenedor-main flex  direction-vertical no-padding no-margin">
                    <iframe id="iframeMapa" class="flex-main" style="border: 0; width: 100%"></iframe>
                </div>


                <!-- Cargando -->
                <div class="cargando opaco">
                    <div class="preloader-wrapper big active">
                        <div class="spinner-layer">
                            <div class="circle-clipper left">
                                <div class="circle"></div>
                            </div>
                            <div class="gap-patch">
                                <div class="circle"></div>
                            </div>
                            <div class="circle-clipper right">
                                <div class="circle"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="contenedorError" class=" card contenedor">
        <label runat="server" id="msjError"></label>
    </div>
    <!-- Mi JS -->

    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/Estadisticas/EstadisticaPanel.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

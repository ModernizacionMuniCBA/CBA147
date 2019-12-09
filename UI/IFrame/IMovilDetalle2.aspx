<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilDetalle2.aspx.cs" Inherits="UI.IFrame.IMovilDetalle2" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilDetalle2.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenedor_Detalle">
        <div id="contenedor_Encabezado">
            <div id="contenedor_Numero">
                <label id="texto_Titulo"></label>
                <div id="contenedor_NumeroArea">
                    <label id="texto_NumeroInterno"></label>
                    <label id="texto_Dominio"></label>
                    <label id="texto_Area"></label>
                </div>
            </div>

            <div id="contenedor_Indicadores">
                <div class="fila fila1">
                    <div>
                        <i id="icono_IndicadorEstado" class="material-icons">swap_vertical_circle</i>
                        <div class="textos">
                            <label id="texto_IndicadorEstado"></label>
                        </div>
                    </div>
                    <div>
                        <i id="icono_IndicadorCondicion" class="material-icons">opacity</i>
                        <div class="textos">
                            <label id="texto_IndicadorCondicion">Condición</label>
                        </div>
                    </div>
                    <div>
                        <i id="icono_IndicadorTipo" class="material-icons">local_shipping</i>
                        <div class="textos">
                            <label id="texto_IndicadorTipo"><b>Tipo: </b></label>
                        </div>
                    </div>
                </div>
                <div class="fila fila2">
                    <div>
                        <i id="icono_IndicadorCarga" class="material-icons">category</i>
                        <div class="textos">
                            <label id="texto_IndicadorCarga">Carga</label>
                        </div>
                    </div>
                    <div>
                        <i id="icono_IndicadorAsientos" class="material-icons">airline_seat_recline_normal</i>
                        <div class="textos">
                            <label id="texto_IndicadorAsientos">Asientos</label>
                        </div>
                    </div>
                    <div>
                        <label id="icono_IndicadorTipoCombustible" class="material-icons">local_gas_station</label>
                        <div class="textos">
                            <label id="texto_IndicadorTipoCombustible"></label>
                        </div>
                    </div>
                </div>

            </div>

        </div>

        <div class="flex direccion-horizontal" id="contenedor_SeccionCaracteristicas" >
            <div id="contenedor_Caracteristicas" class="seccion flex-main">
                <label class="titulo">Características</label>
                <label id="texto_Caracteristicas">"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</label>
            </div>
        </div>

        <div id="contenedor_Alertas">
        </div>

        <div id="contenedor_Acciones">
            <label id="btn_Acciones" class="link no-select">Ocultar acciones</label>
            <div class="contenido visible">
            </div>
        </div>

        <div id="template_Accion" style="display: none">
            <div class="accion">
                <i class="material-icons icono"></i>
                <label class="texto"></label>
            </div>
        </div>


        <div id="contenedor_Contenido">
            <div id="contenedor_Notas" class="seccion">
                <label class="titulo">Notas sin visar</label>

                <div class="sinItems">
                    <label class="textoSinItems">No hay notas</label>
                    <label class="link" id="btn_AgregarNota">Agregar una</label>
                </div>
                <div class="contenido">
                    <div class="items"></div>
                    <div class="verMas">
                        <label>Ver todas</label>
                    </div>
                </div>
            </div>

            <div id="contenedor_Reparaciones" class="seccion">
                <label class="titulo">Reparaciones</label>
                <div class="contenido">
                    <div class="item"></div>
                    <div class="verMas">
                        <label>Ver todas</label>
                    </div>
                </div>
            </div>

            <div id="contenedor_ITVTUV" class="seccion" style="display:none">
                <div class="row">
                    <div class="col s12 m6 no-margin" id="contenedor_ITV">
                        <label class="titulo">Vencimiento ITV</label>

                        <div class="contenido">
                            <div class="linea1">
                                <label class="textoUltimoITV">El último ITV se realizó el </label>
                                <label class="textoFechaUltimoITV"></label>
                            </div>
                            <div class="linea2">
                                <label class="texto">El vencimiento</label>
                                <label class="textoConectorITV">es el </label>
                                <label class="textoFechaVencimientoITV"></label>
                            </div>
                            <div class="linea3">
                                <label class="textoObservacionesITV"></label>
                            </div>
                        </div>
                    </div>

                    <div class="col s12 m6 no-margin" id="contenedor_TUV">
                        <label class="titulo">Vencimiento TUV</label>

                        <div class="contenido">
                            <div class="linea1">
                                <label class="textoUltimoTUV">El último TUV se realizó el </label>
                                <label class="textoFechaUltimoTUV"></label>
                            </div>
                            <div class="linea2">
                                <label class="texto">El vencimiento</label>
                                <label class="textoConectorTUV">es el </label>
                                <label class="textoFechaVencimientoTUV"></label>
                            </div>
                            <div class="linea3">
                                <label class="textoObservacionesTUV"></label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="seccion" id="contenedor_ValuacionKilometraje">
                <div class="row">
                    <div class="col s12 m6 no-margin" id="contenedor_Valuacion">
                        <label class="titulo">Valuación</label>

                        <div class="contenido">
                            <label class="texto">Valuado en </label>
                            <label class="textoValuacion"></label>
                            <label class="texto">el </label>
                            <label class="textoFechaValuacion"></label>
                            <label class="textoObservacionesValuacion"></label>
                        </div>
                    </div>

                    <div class="col s12 m6" id="contenedor_Kilometraje">
                        <label class="titulo">Kilometraje</label>

                        <div class="contenido">
                            <label class="texto">El móvil tiene al día </label>
                            <label class="textoFechaKilometraje"></label>
                            <label class="texto">, </label>
                            <label class="textoKilometraje"></label>
                            <label class="texto">km.</label>

                            <label class="textoObservacionesKilometraje"></label>
                        </div>
                    </div>
                </div>
            </div>
            <div id="contenedor_InfoAdicional" class="seccion">
                <label class="titulo">Información adicional</label>

                <div class="linea1">
                    <label class="texto">El móvil fue incorporado al área el </label>
                    <label class="textoFechaIncorporacion"></label>

                </div>
                <div class="linea2">
                    <label class="texto">Creado el</label>
                    <label class="textoFechaCreacion"></label>
                    <label class="texto textoUsuarioCreadorConector">por</label>
                    <label class="textoUsuarioCreador link"></label>
                </div>
                <div class="linea3">
                    <label class="texto">Fue modificado el</label>
                    <label class="textoFechaModificacion"></label>
                    <label class="texto textoUsuarioModificacionConector">por</label>
                    <label class="textoUsuarioModificacion link"></label>
                </div>
            </div>
        </div>

    </div>

    <%--Mapa--%>
    <div id="contenedor_Mapa">
        <Controles:Mapa ID="mapa" runat="server" />
    </div>

    <%--Templates--%>
    <div id="template_Nota" style="display: none">
        <div class="nota">
            <div class="persona">
                <img src="" />
                <label class="link"></label>
            </div>

            <div class="card">
                <label class="contenido"></label>

                <div class="fechas">
                    <label class="fecha"></label>
                    <div class="vistoPor" style="display: none">
                        <label>Visado por </label>
                        <label class="link personaVisto"></label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="template_Reparacion" style="display: none">
        <div class="reparacion">
            <div class="textos">
                <div class="linea1">
                    <label class="motivo"></label>
                </div>
                <div class="linea2">
                    <label class="taller"></label>
                    <label class="monto"></label>
                </div>
                <div class="linea3">
                    <label class="observaciones"></label>
                </div>
            </div>
            <div class="botones">
                <a class="borrar">Eliminar</a>
            </div>
        </div>
    </div>

    <div id="template_Alerta" style="display: none">
        <div class="alerta">
            <div class="textos">
                <label class="contenido"></label>
                <label class="detalle"></label>
            </div>
            <label class="link"></label>
        </div>
    </div>

    <div id="template_HistorialEstados" style="display: none">
        <div id="contenedor_HistorialEstados">
        </div>
    </div>

    <div id="template_HistorialEstadoItem" style="display: none">
        <div class="estado">
            <div class="indicador">
                <div class="circulo">
                    <i class="material-icons">swap_vertical</i>
                </div>
                <div class="linea1">
                </div>
                <div class="linea2">
                </div>
            </div>

            <div class="textos">
                <label class="nombre">Nuevo</label>

                <label class="motivo">Test test test test test test 1</label>
                <div class="contenedor_Fecha">
                    <label>Por </label>
                    <label class="nombrePersona link"></label>
                    <label class="fecha"></label>
                </div>
            </div>
        </div>
    </div>

    <div id="template_NotasDetalle" style="display: none">
        <div>
            <div class="contenedor_Input">
                <input type="text" placeholder="Nueva nota..." />
                <a class="btn btnNuevoNota waves-effect colorExito">Agregar</a>
            </div>
            <div class="contenido"></div>
        </div>
    </div>

    <div id="template_ReparacionesDetalle" style="display: none">
        <div class="contenido"></div>
    </div>


    <%--Panel Deslizable--%>
    <div id="contenedor_PanelDeslizable">
        <div class="fondo"></div>
        <div class="contenido">
            <div class="encabezado">
                <label class="titulo">Titulo seccion deslizable</label>
                <a id="btn_CerrarPanelDeslizable" class="btn-flat btn-redondo waves-effect"><i class="material-icons">close</i></a>
            </div>
            <div class="contenedor_Contenido"></div>
        </div>

    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/photoswipe.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/photoswipe-ui-default.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/_PoligonosCPC.js?v=1")%>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilDetalle2.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

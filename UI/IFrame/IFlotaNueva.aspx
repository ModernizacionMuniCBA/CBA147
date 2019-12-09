<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IFlotaNueva.aspx.cs" Inherits="UI.IFrame.IFlotaNueva" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IFlotaNueva.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenedor">

        <div id="contenedor_Form">
            <div id="contenedor_Flota">
                <div id="contenedor_TablaMoviles">
                    <div class="encabezado">
                        <label class="titulo">Móviles</label>
                        <div class="input-field contenedor_Busqueda">
                            <i class="material-icons prefix">search</i>
                            <input id="input_BusquedaMoviles" name="barrio" type="text" />
                            <label for="input_BusquedaMoviles" class="no-select">Buscar móviles...</label>
                        </div>
                    </div>

                    <div class="tabla-contenedor">
                        <table id="tablaMoviles"></table>
                    </div>
                    <div class="tabla-footer"></div>
                    <div class="contenedor-movilSeleccionado"></div>
                </div>


                <div id="contenedor_TablaEmpleados">
                    <div class="encabezado">
                        <label class="titulo">Personal</label>
                        <div class="input-field contenedor_Busqueda">
                            <i class="material-icons prefix">search</i>
                            <input id="input_BusquedaEmpleados" name="barrio" type="text" />
                            <label for="input_BusquedaEmpleados" class="no-select">Buscar personal...</label>
                        </div>
                    </div>

                    <div class="tabla-contenedor">
                        <table id="tablaEmpleados"></table>
                    </div>
                    <div class="tabla-footer"></div>
                    <div class="contenedor-personalSeleccionado"></div>
                </div>

                <div id="contenedor_Seleccionados">
                    <div id="contenedor_MovilSeleccionado">
                        <label class="titulo">Móvil Seleccionado</label>
                        <label class="detalle">No hay ningún mate seleccionado aún</label>

                        <div class="movil" style="display: none">
                            <div class="datos">
                                <label class="numero"></label>
                                <label class="tipo"></label>
                                <label class="marca"></label>
                            </div>
                            <a class="btn-flat btn-redondo">
                                <i class="material-icons borrar">delete</i></a>
                        </div>
                    </div>
                    <div id="contenedor_PersonalSeleccionadoTitulo">
                        <label class="titulo">Personal Seleccionado</label>
                        <label class="detalle">No hay personal seleccionado aún</label>
                    </div>
                    <div id="contenedor_PersonalSeleccionado">
                        <div class="contenido"></div>
                    </div>
                    <div class="separador">
                        <div id="contenedor_Encabezado">
                            <div class="row">
                                <div class="col s12" style="margin-top: 8px;">
                                    <div class="input-field fix-margin">
                                        <textarea id="input_Observaciones" class="materialize-textarea contador" length="100"></textarea>
                                        <label for="input_Observaciones" class="no-select">Observaciones</label>
                                        <a id="errorFormulario_Observacion" class="control-observacion colorTextoError no-select"></a>
                                    </div>
                                </div>
                            </div>
                            <div id="contenedor_Acciones">
                                <label id="btn_DarDeBaja" class="link">Dar de baja</label>
                                <label id="btn_DarDeAlta" class="link">Dar de alta</label>
                            </div>
                        </div>
                    </div>
            </div>
        </div>
    </div>

    <div id="template_Empleado" style="display: none">
        <div class="empleado">
            <div class="persona">
                <img class="foto" src="">
                <label class=" nombre"></label>
                <a class="btn-flat btn-redondo">
                    <i class="material-icons borrar">delete</i></a>
            </div>
        </div>
    </div>

    <div id="template_Movil" style="display: none">
        <div class="movil">
            <div class="persona">
            </div>
        </div>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IFlotaNueva.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

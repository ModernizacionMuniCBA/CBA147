<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMovilDetalle.aspx.cs" Inherits="UI.IFrame.IMovilDetalle" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div style="display: block" class="scroll padding">

        <div class="row">
            <!-- Nombre -->
            <div class="col s12">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label id="textoNombre" class="tituloPrincipal no-select"></label>
                    </div>
                </div>
            </div>

        </div>
        <div class="row">
            <!--Área -->
            <div class="col s12 m3">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo no-select">Área</label>
                    </div>
                    <div class="col s12 mi-input-field  contenedor-detalle flex direction-vertical">
                        <label class="detalle" id="textoArea" runat="server"></label>
                    </div>
                </div>
            </div>

            <!-- Estado-->
            <div class="col s12 m3">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo  no-select">Estado</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoEstado" runat="server"></label>
                    </div>
                </div>
            </div>


            <!-- Condicion-->
            <div class="col s12 m3" id="contenedorCondicion">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo  no-select">Condición</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoCondicion" runat="server"></label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col s12 ">
                <label class="subtitulo  no-select">Datos adicionales</label>
            </div>

            <!-- Fecha incorporación -->
            <div class="col s12 m3">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="titulo no-select">Fecha de incorporación</label>
                    </div>
                    <div class="col s12 mi-input-field  contenedor-detalle flex direction-vertical">
                        <label class="detalle" id="textoFechaIncorporacion" runat="server"></label>
                    </div>
                </div>
            </div>

            <!-- ITV-->
            <div class="col s12 m3" id="contenedorITV">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="titulo  no-select">Vencimiento ITV</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoITV" runat="server"></label>
                    </div>
                </div>
            </div>

            <!-- TUV-->
            <div class="col s12 m3" id="contenedorTUV">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="titulo  no-select">Vencimiento TUV</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoTUV" runat="server"></label>
                    </div>
                </div>
            </div>

            <!-- Taller-->
            <div class="col s12 m3" id="contenedorTaller">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="titulo  no-select">Último service</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoTaller" runat="server"></label>
                    </div>
                </div>
            </div>

            <!-- Valuacion-->
            <div class="col s12 m3" id="contenedorValuacion">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="titulo  no-select">Valuación</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoValuacion" runat="server"></label>
                    </div>
                </div>
            </div>

            <!-- Km-->
            <div class="col s12 m3" id="contenedorKm">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="titulo  no-select">Kilometraje</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoKm" runat="server"></label>
                    </div>
                </div>
            </div>

            <!-- Carga-->
            <div class="col s12 m3" id="contenedorCarga">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="titulo  no-select">Capacidad de carga</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoCarga" runat="server"></label>
                    </div>
                </div>
            </div>

            <!-- Asientos-->
            <div class="col s12 m3" id="contenedorAsientos">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="titulo  no-select">Asientos</label>
                    </div>
                    <div class="col s12 mi-input-field flex  contenedor-detalle direction-vertical">
                        <label class="detalle" id="textoAsientos" runat="server"></label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <!-- Observaciones -->
            <div class="col s12" id="contenedorObservaciones" runat="server">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo no-select">Observaciones</label>
                    </div>
                    <div class="col s12 mi-input-field contenedor-detalle  flex direction-vertical">
                        <label class="detalle" id="textoObservaciones" runat="server"></label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMovilDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

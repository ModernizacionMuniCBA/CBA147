<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IUbicacionAutomaticoSelector.aspx.cs" Inherits="UI.IFrame.IUbicacionAutomaticoSelector" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IUbicacionAutomaticoSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <form id="formNuevo" style="height: 100%;">
        <div id="UbicacionSelector_ContenedorBusqueda" class="scroll padding">
            <div class="UbicacionSelector_ContenedorInstrucciones">
                <label>Ingrese calle y número o punto de interés y presione buscar para confirmar el domicilio. También puede presionar el botón del mapa para buscarlo ahí.</label>
            </div>

            <div id="UbicacionSelector_ContenedorBuscar">
                <div class="input-field ">
                    <input id="UbicacionSelector_InputBuscar" type="text" required="" aria-required="true" />
                    <label for="UbicacionSelector_InputBuscar" class="no-select">Ingrese datos del domicilio y pulse buscar</label>
                </div>
                <div class="contenedor-boton">
                    <a id="UbicacionSelector_BtnMapa" class="tooltipped btn btn-cuadrado " data-position="bottom" data-tooltip="Seleccionar en mapa"><i class="material-icons">map</i></a>
                    <a id="UbicacionSelector_BtnBuscar" class="tooltipped btn btn-cuadrado " data-position="bottom" data-tooltip="Buscar"><i class="material-icons">search</i></a>
                </div>
            </div>
        </div>

        <div id="UbicacionSelector_ContenedorSugerencias" class="scroll">
        </div>

        <div id="UbicacionSelector_ContenedorUbicacionSeleccionada">
        </div>

        <div id="UbicacionSelector_ContenedorObservaciones" style="display: none">
            <div class="input-field">
                <textarea id="UbicacionSelector_InputObservaciones" class="materialize-textarea"></textarea>
                <label for="UbicacionSelector_InputObservaciones" class="no-select textarea">Observación del domicilio</label>
            </div>
        </div>

        <div id="UbicacionSelector_TemplateSugerencia" style="display: none">
            <div class="sugerencia clickable">
                <i class="material-icons"></i>
                <div class="textos">
                    <label class="texto1"></label>
                    <label class="texto2"></label>
                </div>

                <div class="contenedor-boton">
                    <a class="tooltipped btn btn-cuadrado " data-position="bottom" data-tooltip="Buscar"><i class="material-icons">check</i></a>
                </div>
            </div>
        </div>

        <div id="UbicacionSelector_TemplateUbicacionSeleccionada" style="display: none">
            <div class="ubicacion">
                <div class="textos">
                    <label class="domicilio"></label>
                    <label class="barrio"></label>
                    <label class="cpc"></label>
                </div>
                <div class="cancelar">
                    <i class="btn-icono material-icons">clear</i>
                </div>
            </div>
        </div>
    </form>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IUbicacionAutomaticoSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

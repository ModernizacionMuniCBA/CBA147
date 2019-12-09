<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenTrabajoNuevo.aspx.cs" Inherits="UI.IFrame.IOrdenTrabajoNuevo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrdenTrabajoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical full-height">
        <div class="contenedorTabs">
            <ul id="MenuTab" class="tabs">
                <li class="tab col s3"><a class="active" href="#tabPrincipal">Principal</a></li>
                <li id="btnTabMoviles" class="tab col s3"><a id="textoTabMoviles" href="#tabMoviles">Móviles</a></li>
                <li id="btnTabPersonal" class="tab col s3"><a id="textoTabPersonal" href="#tabPersonal">Personal</a></li>
                <li id="btnTabFlotas" class="tab col s3"><a id="textoTabFlotas" href="#tabFlotas">Flotas</a></li>
            </ul>
        </div>
        <div class="flex-main flex direction-vertical" id="contenedorContenidoTabs" style="overflow: auto">
            <!-- Pestaña Principal -->
            <div id="tabPrincipal" class="flex-main scroll padding">
                <div class="row" >
                    <div class="col s6" id="contenedor-estadosOT" style="display:none">
                        <div class="contenedor-detalle">
                            <label class="titulo no-select" style="margin-bottom: 1rem;">Crear Orden de Trabajo en estado...</label>
                            <div id="checkboxEstadosOT"></div>
                        </div>
                    </div>

                    <div class="col s6" id="contenedor-estadosRQ">
                        <div class="contenedor-detalle">
                            <label class="titulo no-select" style="margin-bottom: 1rem;">Establecer estado de requerimientos a...</label>
                            <div id="checkboxEstadosRQ"></div>
                        </div>
                    </div>
                </div>

                <div class="form-separador" id="separador-estados"></div>

                <div class="row">
                    <div class="col s12">
                        <div class="contenedor-detalle">
                            <label class="titulo no-select">Requerimientos seleccionados</label>
                        </div>
                    </div>
                </div>

                <div class="row" id="contenedor-requerimientos">
                </div>

                <div>
                    <label id="textoDiferenteZona">Los requerimientos no pertenecen a la misma zona</label>
                </div>
                <div class="form-separador"></div>


                <div class="row">
                    <div class="col s12">
                        <div class="contenedor-detalle">
                            <label class="titulo no-select">Descripcion</label>
                        </div>
                    </div>
                </div>

                <div class="row ">
                    <div class="col s12">
                        <div class="input-field fix-margin">
                            <textarea id="inputFormulario_Descripcion" class="materialize-textarea"></textarea>
                            <label for="inputFormulario_Descripcion" class=" no-select textarea">Descripción</label>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col s12">
                        <div class="contenedor-detalle">
                            <label class="titulo no-select">Recursos</label>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col s12">
                        <div class="input-field fix-margin">
                            <textarea id="inputFormulario_Personal" class="materialize-textarea"></textarea>
                            <label for="inputFormulario_Personal" class=" no-select textarea">Personal</label>
                        </div>
                    </div>
                </div>
                <%--                <div class="row"  >
                    <div class="col s12">
                        <div class="input-field fix-margin">
                            <textarea id="inputFormulario_Flota" class="materialize-textarea"></textarea>
                            <label for="inputFormulario_Flota" class=" no-select textarea">Flota</label>
                        </div>
                    </div>
                </div>--%>
                <div class="row">
                    <div class="col s12">
                        <div class="input-field fix-margin">
                            <textarea id="inputFormulario_Material" class="materialize-textarea"></textarea>
                            <label for="inputFormulario_Material" class=" no-select textarea">Material</label>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col s12">
                        <div class="input-field fix-margin">
                            <textarea id="inputFormulario_RecursoObservacion" class="materialize-textarea"></textarea>
                            <label for="inputFormulario_RecursoObservacion" class=" no-select textarea">Observaciones del recurso</label>
                        </div>
                    </div>
                </div>


                <div class="row" id="contenedor-seccion">
                    <div class="form-separador"></div>
                    <%--                 <div class="col s12 m6 l4">
                        <div class="mi-input-field margin-bottom">
                            <label class="no-select">Zona</label>
                            <select id="inputFormulario_SelectZona" style="width: 100%"></select>
                        </div>
                    </div>--%>
                    <div class="col s12 m6 l4">
                        <div class="mi-input-field margin-bottom">
                            <label class="no-select">Sección</label>
                            <select id="inputFormulario_SelectSeccion" style="width: 100%"></select>
                        </div>
                    </div>
                </div>




            </div>
            <!-- Pestaña Moviles -->
            <div id="tabMoviles" class="flex-main scroll">
                <div class="contenedor-main no-padding flex direction-vertical full-height">
                    <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                        <table id="tablaMoviles"></table>
                    </div>
                    <div class="tabla-footer padding-left">
                    </div>
                </div>
            </div>

            <!-- Pestaña Personal -->
            <div id="tabPersonal" class="flex-main scroll">
                <div class="contenedor-main no-padding flex direction-vertical full-height">
                    <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                        <table id="tablaPersonal"></table>
                    </div>
                    <div class="tabla-footer padding-left">
                    </div>
                </div>
            </div>

            <!-- Pestaña Flota -->
            <div id="tabFlotas" class="flex-main scroll">
                <div class="contenedor-main no-padding flex direction-vertical full-height">
                    <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                        <table id="tablaFlotas"></table>
                    </div>
                    <div class="tabla-footer padding-left">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Mi Js -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenTrabajoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

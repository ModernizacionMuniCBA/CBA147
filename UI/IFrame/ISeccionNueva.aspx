 <%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="ISeccionNueva.aspx.cs" Inherits="UI.IFrame.ISeccionNueva" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/ISeccionNueva.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenedor">

        <div id="contenedor_Form">

            <form id="form">
                <div style="display: block">

                    <div id="contenedor_Encabezado">
                        <div class="row">
                            <div class="col s12  m3">
                                <div class="input-field ">
                                    <input id="input_Nombre" type="text" required="" aria-required="true" />
                                    <label for="input_Nombre" class="no-select">Nombre</label>
                                </div>
                            </div>
                            <div class="col s12 m9">
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
            </form>

            <div id="contenedor_Empleados">
                <div id="contenedor_TablaDisponibles">
                    <div class="encabezado">
                        <label class="titulo">Personal disponible</label>
                        <div class="input-field contenedor_Busqueda">
                            <i class="material-icons prefix">search</i>
                            <input id="input_BusquedaDisponibles" name="barrio" type="text" />
                            <label for="input_BusquedaDisponibles" class="no-select">Buscar personal...</label>
                        </div>
                    </div>

                    <div class="tabla-contenedor">
                        <table id="tablaDisponibles"></table>
                    </div>
                    <div class="tabla-footer"></div>
                </div>


                <div id="contenedor_TablaSeleccionados">
                    <div class="encabezado">
                        <label class="titulo">Personal seleccionado</label>
                        <div class="input-field contenedor_Busqueda">
                            <i class="material-icons prefix">search</i>
                            <input id="input_BusquedaSeleccionados" name="barrio" type="text" />
                            <label for="input_BusquedaSeleccionados" class="no-select">Buscar personal...</label>
                        </div>
                    </div>

                    <div class="tabla-contenedor">
                        <table id="tablaSeleccionados"></table>
                    </div>
                    <div class="tabla-footer"></div>
                </div>
            </div>
        </div>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/ISeccionNueva.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

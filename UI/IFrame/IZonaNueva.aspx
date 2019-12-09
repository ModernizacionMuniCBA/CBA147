<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IZonaNueva.aspx.cs" Inherits="UI.IFrame.IZonaNueva" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IZonaNueva.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="contenedor">

        <div id="contenedor_Form">

            <form id="form">
                <div style="display: block">

                    <div id="contenedor_Encabezado">
                        <div class="row">
                            <div class="col s12 m6">
                                <div class="input-field ">
                                    <input id="input_Nombre" name="nombre" type="text" required="" aria-required="true" />
                                </div>
                            </div>



                        </div>
                        <div id="contenedor_Acciones">
                            <label id="btn_DarDeBaja" class="link">Dar de baja</label>
                            <%--<label id="btn_DarDeAlta" class="link">Dar de alta</label>--%>
                        </div>

                    </div>

                </div>
            </form>

            <div id="contenedor_Barrios">

                
                <%--<div class="form-separador"></div>--%>
                <div id="contenedor_TablaDisponibles">
                    <div class="encabezado">
                        <label class="titulo">Barrios disponibles</label>
                        <div class="input-field contenedor_Busqueda">
                            <i class="material-icons prefix">search</i>
                            <input id="input_BusquedaDisponibles" name="barrio" type="text" />
                            <label for="input_BusquedaDisponibles" class="no-select">Buscar barrio...</label>
                        </div>
                    </div>

                    <div class="tabla-contenedor">
                        <table id="tablaDisponibles"></table>
                    </div>
                    <div class="tabla-footer"></div>
                </div>
                <div id="contenedor_TablaSeleccionados">
                    <div class="encabezado">
                        <label class="titulo">Barrios seleccionados</label>
                        <div class="input-field contenedor_Busqueda">
                            <i class="material-icons prefix">search</i>
                            <input id="input_BusquedaSeleccionados" name="barrio" type="text" />
                            <label for="input_BusquedaSeleccionados" class="no-select">Buscar barrio...</label>
                        </div>
                    </div>

                    <div class="tabla-contenedor">
                        <table id="tablaSeleccionados"></table>
                    </div>
                    <div class="tabla-footer"></div>
                </div>
            </div>

        </div>

        <div id="contenedor_Mapa">
            <div id="mapa">
            </div>

        </div>
    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IZonaNueva.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

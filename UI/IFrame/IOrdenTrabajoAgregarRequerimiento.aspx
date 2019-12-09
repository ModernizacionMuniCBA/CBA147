<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenTrabajoAgregarRequerimiento.aspx.cs" Inherits="UI.IFrame.IOrdenTrabajoAgregarRequerimiento" %>

<%@ Register Src="~/Controls/ControlSelectorRangoFecha.ascx" TagName="SelectorFecha" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrdenTrabajoAgregarRequerimiento.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="contenedor_Encabezado">

        <div id="contenedor_Busqueda">

            <div id="contenedor_Numero" class="row">
                <div class="col s12 m4">
                    <div class="input-field">
                        <input id="input_Numero" type="text" />
                        <label for="input_Año" class="no-select">Número</label>
                    </div>
                </div>

                <div class="col s12 m2">
                    <div class="input-field">
                        <input id="input_Año" type="text" />
                        <label for="input_Año" class="no-select">Año</label>
                    </div>
                </div>

            </div>

            <div class="indicador">
                <label>Hola hola hola hola hola</label>
            </div>

            <div id="contenedor_Fecha" class="row">
                <div class="col s12 m8 no-padding">
                    <Controles:SelectorFecha runat="server" />
                </div>
            </div>
        </div>

        <div id="contenedor_Botones">
            <a id="btn_OcultarEncabezado" class="btn-flat btn-redondo waves-effect"><i class="material-icons">chevron_right</i></a>
            <a id="btn_Buscar" class="btn waves-effect colorExito"><i class="material-icons btn-icono">search</i>Buscar</a>
        </div>

    </div>

    <div id="contenedor_Resultado">
        <div class="tabla-contenedor">
            <table id="tabla"></table>
        </div>
        <div class="tabla-footer">
        </div>
    </div>

    <div id="contenedor_Error" class="panel-mensaje">
        <i class="material-icons">error_outline</i>
        <label></label>
    </div>

    <!-- Mi Js -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenTrabajoAgregarRequerimiento.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

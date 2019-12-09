<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="UsuarioDetalle.aspx.cs" Inherits="UI.UsuarioDetalle" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/UsuarioDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div class="no-scroll flex direction-vertical full-height">

        <div style="display: block">

            <div class="row padding-top">

                <!--Nombre -->
                <div class="col s12 m6">
                    <div class="row">
                        <div class="col s12 no-padding no-margin">
                            <label class="subtitulo no-select">Nombre</label>
                        </div>
                        <div class="col s12 mi-input-field flex direction-vertical">
                            <label class="detalle" id="textoNombre"></label>
                        </div>
                    </div>
                </div>

                   <!--Nombre -->
                <div class="col s12 m6">
                    <div class="row">
                        <div class="col s12 no-padding no-margin">
                            <label class="subtitulo no-select">Rol</label>
                        </div>
                        <div class="col s12 mi-input-field flex direction-vertical">
                            <label class="detalle" id="textoRol"></label>
                        </div>
                    </div>
                </div>

                <!--Areas -->
                <div class="col s12 m4" id="contenedorAreas" runat="server">
                    <div class="row">
                        <div class="col s12 no-padding no-margin">
                            <label class="subtitulo no-select">Areas</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="flex-main flex direction-vertical">
            <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                <table id="tabla"></table>
            </div>
            <div class="tabla-footer">
            </div>
        </div>

    </div>


    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/UsuarioDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

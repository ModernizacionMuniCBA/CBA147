<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrigenDetalle.aspx.cs" Inherits="UI.IOrigenDetalle" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrigenDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div style="display: block" class="scroll padding">

        <div class="row">

            <!--Nombre -->
            <div class="col s12">
                <div class="row">
                    <div class="col s12 no-padding no-margin contenedor-detalle">
                        <label class="titulo no-select">Nombre</label>
                        <label class="detalle" id="textoNombre"></label>
                    </div>
                </div>
            </div>

            <!--Key Alias  -->
            <div class="col s12 m4">
                <div class="row">
                    <div class="col s12 no-padding no-margin contenedor-detalle">
                        <label class="titulo no-select">Key Alias</label>
                        <label class="detalle" id="textoKeyAlias"></label>
                    </div>
                </div>
            </div>

            <!--Key Secret -->
            <div class="col s12 m4">
                <div class="row">
                     <div class="col s12 no-padding no-margin contenedor-detalle">
                        <label class="titulo no-select">Key Secret</label>
                        <label class="detalle" id="textoKeySecret"></label>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrigenDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

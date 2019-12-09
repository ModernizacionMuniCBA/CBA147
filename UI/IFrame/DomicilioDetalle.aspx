<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="DomicilioDetalle.aspx.cs" Inherits="UI.DomicilioDetalle" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/DomicilioDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div style="display: block" class="scroll padding">

        <div class="row">

            <!--Dirección -->
            <div class="col s12" id="contenedorCalle" runat="server">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo no-select">Dirección</label>
                    </div>
                    <div class="col s12 mi-input-field flex direction-vertical">
                        <label class="detalle" id="textoDireccion"></label>
                    </div>
                </div>
            </div>

            <!--Barrio -->
            <div class="col s12 m4" id="contenedorBarrio" runat="server">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo no-select">Barrio</label>
                    </div>
                    <div class="col s12 mi-input-field flex direction-vertical">
                        <label class="detalle" id="textoBarrio"></label>
                    </div>
                </div>
            </div>

            <!--CPC -->
            <div class="col s12 m4" id="contenedorCPC" runat="server">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo no-select">CPC</label>
                    </div>
                    <div class="col s12 mi-input-field flex direction-vertical">
                        <label class="detalle" id="textoCPC"></label>
                    </div>
                </div>
            </div>

            <!--Observaciones -->
            <div class="col s12 margin-top" id="contenedorObservaciones" runat="server">
                <div class="row">
                    <div class="col s12 no-padding no-margin">
                        <label class="subtitulo no-select">Observaciones</label>
                    </div>
                    <div class="col s12 mi-input-field flex direction-vertical">
                        <label class="detalle" id="textoObservaciones"></label>
                    </div>
                </div>
            </div>

        </div>
    </div>

     <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/DomicilioDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>

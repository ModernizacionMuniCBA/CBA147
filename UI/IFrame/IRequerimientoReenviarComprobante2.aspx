<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoReenviarComprobante2.aspx.cs" Inherits="UI.IFrame.IRequerimientoReenviarComprobante2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IEmpleadoSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical no-scroll full-height">
        <div class="row" style="margin: 12px;">
            <label>Se reenviará el comprobante a los usuarios referentes que seleccione y, si lo desea, a un email extra que puede ingresar en el campo de texto.</label>
        </div>
        <div class="busqueda">
            <div class="row">
                <div class="col s4">
                    <div class="input-field">
                        <input type="text" id="input_Email" />
                        <label for="input_Email">E-mail</label>
                    </div>
                </div>
            </div>
        </div>

        <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">

            <table id="tablaSelector"></table>
        </div>
        <div class="tabla-footer padding-left">
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoReenviarComprobante2.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

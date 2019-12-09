<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IFlotaSelector.aspx.cs" Inherits="UI.IFrame.IFlotaSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IFlotaSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical no-scroll full-height">


        <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
            <div class="busqueda">
                <div class="row">
                    <div class="col">
                        <i class="material-icons">search</i>
                    </div>
                    <div class="col s6">
                        <input id="input_BusquedaEmpleados" type="text" placeholder="Buscar..." class="busqueda" />
                    </div>
                </div>
            </div>
            <table id="tablaSelector"></table>
        </div>
        <div class="tabla-footer padding-left">
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IFlotaSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

﻿<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrigenPorAreaNuevo.aspx.cs" Inherits="UI.IFrame.IOrigenPorAreaNuevo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrigenPorAreaNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="flex: 1" id="contenedor" class="scroll">
        <div class="row">
            <!-- Area -->
            <div class="col s12 m6 l4">
                <div class="mi-input-field margin-bottom">
                    <label class="no-select">Area</label>
                    <select id="inputFormulario_SelectArea" style="width: 100%"></select>
                    <a class="control-observacion colorTextoError no-select"></a>
                </div>
            </div>
        </div>

        <div class="row">
            <!-- Origen -->
            <div class="col s12 m6 l4">
                <div class="mi-input-field margin-bottom">
                    <label class="no-select">Origen</label>
                    <select id="inputFormulario_SelectOrigen" style="width: 100%"></select>
                    <a class="control-observacion colorTextoError no-select"></a>
                </div>
            </div>
        </div>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrigenPorAreaNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>

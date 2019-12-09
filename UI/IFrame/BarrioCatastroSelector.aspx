<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="BarrioCatastroSelector.aspx.cs" Inherits="UI.IFrame.BarrioCatastroSelector" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/BarrioCatastroSelector.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


        <div class="flex direction-vertical no-scroll full-height">
        <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
            <table id="tabla"></table>
        </div>
        <div class="tabla-footer padding-left">
        </div>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/BarrioCatastroSelector.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IMapa.aspx.cs" Inherits="UI.IFrame.IMapa" %>

<%@ Register Src="~/Controls/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMapa.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <Controles:Mapa runat="server" />


    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IMapa.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
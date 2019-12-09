<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IGoogleMaps.aspx.cs" Inherits="UI.IFrame.IGoogleMaps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IGoogleMaps.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="map"></div>

    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IGoogleMaps.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <script src="https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/markerclusterer.js"></script>
    <script src="https://maps.googleapis.com/maps/api/js?key=<% =ConfigurationManager.AppSettings["KEY_GOOGLE_MAPS"] %>&callback=initMap&libraries=visualization"></script>

</asp:Content>

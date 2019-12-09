<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reporte.ascx.cs" Inherits="UI.Controls.Reporte" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.0.15.324, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<telerik:ReportViewer 
    ID="ReportViewer" 
    runat="server"
    ShowDocumentMapButton="False" 
    ShowExportGroup="False" 
    ShowParametersButton="False"
    ShowPrintPreviewButton="False" 
    ShowRefreshButton="False" 
    ShowZoomSelect="True"
    Skin="Default" 
    Width="100%" 
    Height="400px">

</telerik:ReportViewer>


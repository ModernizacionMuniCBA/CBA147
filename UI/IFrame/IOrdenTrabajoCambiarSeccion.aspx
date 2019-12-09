<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenTrabajoCambiarSeccion.aspx.cs" Inherits="UI.IFrame.IOrdenTrabajoCambiarSeccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="flex direction-vertical scroll">

        <div class="contenedor-main padding">
                <!-- Sección -->
                <div class="col s12">
                    <div class="mi-input-field">
                        <label class="no-select">Sección</label>
                        <select id="select_Seccion" style="width: 100% "></select>
                        <a class="control-observacion colorTextoError no-select"></a>
                    </div>
                </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenTrabajoCambiarSeccion.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

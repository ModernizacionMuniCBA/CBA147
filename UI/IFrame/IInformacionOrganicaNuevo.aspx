<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IInformacionOrganicaNuevo.aspx.cs" Inherits="UI.IFrame.IInformacionOrganicaNuevo" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IInformacionOrganicaNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="padding">

        <form id="form">

            <div class="row">

                <div class="col s12 m6 l4">
                    <div>

                        <div class="mi-input-field">
                            <label class="no-select">Secretaria</label>
                            <select id="input_Secretaria" class="select-requerido" style="width: 100%"></select>
                        </div>
                        <a id="btn_AgregarSecretaria" class="btn btn-cuadrado chico waves-effect"><i class="material-icons">add</i></a>
                    </div>

                </div>

                <div class="col s12 m6 l4">
                    <div>
                        <div class="mi-input-field">
                            <label class="no-select">Dirección</label>
                            <select id="input_Direccion" class="select-requerido" style="width: 100%"></select>
                        </div>
                        <a id="btn_AgregarDireccion" class="btn btn-cuadrado chico waves-effect"><i class="material-icons">add</i></a>

                    </div>
                </div>
            </div>
        </form>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IInformacionOrganicaNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

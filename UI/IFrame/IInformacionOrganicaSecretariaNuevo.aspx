<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IInformacionOrganicaSecretariaNuevo.aspx.cs" Inherits="UI.IFrame.IInformacionOrganicaSecretariaNuevo" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IInformacionOrganicaSecretariaNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="padding">

        <form id="form">

            <div class="row">

                <div class="col s12">
                    <div class="input-field">
                        <input id="input_Nombre" type="text" name="nombre" required="" aria-required="true" maxlength="500" />
                        <label for="input_Nombre" class="no-select">Nombre</label>
                    </div>
                </div>
            </div>
        </form>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IInformacionOrganicaSecretariaNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

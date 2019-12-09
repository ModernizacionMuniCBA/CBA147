<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IVersionSistema.aspx.cs" Inherits="UI.IFrame.IVersionSistema" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IVersionSistema.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="padding">

        <form id="form">

            <div class="row">

                <!-- Correo-E-->

                <div class="col s12">
                    <div class="input-field">
                        <i class="material-icons prefix">info</i>
                        <input id="input_Version" type="text" name="version" required="" aria-required="true" />
                        <label for="input_Version" class="no-select">Versión</label>
                    </div>
                </div>

            </div>
            
            <div class="row margin-top">
                <!-- Descripción  -->
                <div class="col s12">
                    <div class="input-field">
                        <textarea id="input_Descripcion" name="mensaje" class="materialize-textarea" required="" aria-required="true" lines="3" maxlength="2000"></textarea>
                        <label for="input_Descripcion" class=" no-select">Cambios realizados...</label>
                    </div>
                </div>
            </div>
        </form>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IVersionSistema.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

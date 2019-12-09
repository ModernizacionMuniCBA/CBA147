<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IUsuarioCerrojoCambiarPassword.aspx.cs" Inherits="UI.IFrame.IUsuarioCerrojoCambiarPassword" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IUsuarioCerrojoCambiarPassword.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="display: block">

        <form id="form">

            <div class="row margin-top">
                <div class="col s12 m6 l4">
                    <div class="input-field">
                        <input id="input_PasswordAnterior" type="password" name="passwordAnterior" autocomplete="new-password" required="" aria-required="true" minlength="8" maxlength="50" />
                        <label for="input_PasswordAnterior" class="no-select">Contraseña actual</label>
                    </div>
                </div>
            </div>

            <div class="form-separador" />

            <div class="row margin-top">
                <div class="col s12 m6 l4">
                    <div class="input-field">
                        <input id="input_Password" type="password" name="password" autocomplete="new-password" required="" aria-required="true" minlength="8" maxlength="50" />
                        <label for="input_Password" class="no-select">Contraseña nueva</label>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col s12 m6 l4">
                    <div class="input-field">
                        <input id="input_PasswordRepeat" type="password" name="repeatPassword" autocomplete="new-password" required="" aria-required="true" minlength="8" maxlength="50" />
                        <label for="input_PasswordRepeat" class="no-select">Reingrese la nueva contraseña</label>
                    </div>
                </div>
            </div>
        </form>

        <!-- Mi JS -->
        <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IUsuarioCerrojoCambiarPassword.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    </div>
</asp:Content>

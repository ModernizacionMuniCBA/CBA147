<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IContacto.aspx.cs" Inherits="UI.IFrame.IContacto" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IContacto.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="padding">

        <form id="formContacto">

            <div class="card" id="contenedor_Info">
                <i class="material-icons">help</i>
                <label>Desde esta pantalla usted puede contactarse con el soporte de #CBA147 para informar un problema o realizar una solicitud.<br />Por favor sea lo más descriptivo posible</label>
            </div>

            <div class="row">

                <!-- Correo-E-->

                <div class="col s12">
                    <div class="input-field">
                        <i class="material-icons prefix">mail</i>
                        <input id="input_MailContacto" type="email" name="mail" required="" aria-required="true" />
                        <label for="input_MailContacto" class="no-select">Mail de contacto</label>
                    </div>
                </div>

            </div>
            <div class="row">
                <!-- Telefono -->
                <div class="col s12">
                    <div class="input-field">
                        <i class="material-icons prefix">phone</i>
                        <input id="input_TelefonoContacto" type="text" name="telefono" />
                        <label for="input_TelefonoContacto" class="no-select">Teléfono de contacto</label>
                    </div>
                </div>
            </div>

            <div class="row margin-top">
                <!-- Descripción  -->
                <div class="col s12">
                    <div class="input-field">
                        <textarea id="input_Descripcion" name="mensaje" class="materialize-textarea" required="" aria-required="true" lines="3" maxlength="2000" minlength="50"></textarea>
                        <label for="input_Descripcion" class=" no-select">Descripcion del problema, solicitud o requerimiento...</label>
                    </div>
                </div>
            </div>
        </form>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IContacto.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

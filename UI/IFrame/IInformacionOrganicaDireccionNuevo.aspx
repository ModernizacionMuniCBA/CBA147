<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IInformacionOrganicaDireccionNuevo.aspx.cs" Inherits="UI.IFrame.IInformacionOrganicaDireccionNuevo" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IInformacionOrganicaDireccionNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="padding">

        <form id="form">

            <div class="row">

                <div class="col s12 m6 l4" id="contenedor_Secretaria">
                    <div class="mi-input-field margin-bottom">
                        <label class="no-select">Secretaria</label>
                        <select id="input_Secretaria" class="select-requerido" style="width: 100%"></select>
                    </div>
                </div>

                <div class="col s12">
                    <div class="input-field">
                        <input id="input_Nombre" type="text" name="nombreDireccion" required="" aria-required="true" maxlength="255 " />
                        <label for="input_Nombre" class="no-select">Nombre</label>
                    </div>
                </div>

                <div class="col s12">
                    <div class="input-field">
                        <i class="material-icons prefix">account_circle</i>
                        <input id="input_Responsable" type="text" name="nombre" required="" aria-required="true" maxlength="3000" />
                        <label for="input_Responsable" class="no-select">Responsable</label>
                    </div>
                </div>

                <div class="col s12">
                    <div class="input-field">
                        <i class="material-icons prefix">map</i>
                        <input id="input_Domicilio" type="text" name="domicilio" required="" aria-required="true" maxlength="3000" />
                        <label for="input_Domicilio" class="no-select">Domicilio</label>
                    </div>
                </div>

                <div class="col s12">
                    <div class="input-field">
                        <i class="material-icons prefix">phone</i>
                        <input id="input_Telefono" type="text" name="telefono" required="" aria-required="true" maxlength="255" />
                        <label for="input_Telefono" class="no-select">Teléfono</label>
                    </div>
                </div>

                <div class="col s12">
                    <div class="input-field">
                        <i class="material-icons prefix">email</i>
                        <input id="input_Email" type="text" name="email" required="" aria-required="true" maxlength="255" />
                        <label for="input_Email" class="no-select">E-Mail</label>
                    </div>
                </div>



            </div>
        </form>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IInformacionOrganicaDireccionNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IUsuarioCerrojoNuevo.aspx.cs" Inherits="UI.IFrame.IUsuarioCerrojoNuevo" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlSexoSelector.ascx" TagName="SexoSelector" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IUsuarioCerrojoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IUsuarioCerrojoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <div style="display: block; overflow: auto">

        <input type="file" id="pickerFoto" accept="image/*" style="display: none" />

        <div class="row padding" id="contenedor_DatosPersonales">
            <form id="formDatosPersonales">

                <div class="col s12 m12 l3">
                    <label class="titulo">Datos personales</label>
                </div>
                <div class="col s12 m12 l9 no-padding">
                    <div class="row">
                        <div class="col s12 m6 l4">
                            <div class="input-field">
                                <input id="input_Nombre" name="nombre" type="text" maxlength="200" required="" aria-required="true" />
                                <label for="input_Nombre" class="no-select">Nombre</label>
                            </div>
                        </div>
                        <div class="col s12 m6 l4">
                            <div class="input-field">
                                <input id="input_Apellido" name="apellido" type="text" maxlength="200" required="" aria-required="true" />
                                <label for="input_Apellido" class="no-select">Apellido</label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col s12 m6 l4">
                            <div class="input-field">
                                <input id="input_Dni" name="dni" type="text" maxlength="8" required="" aria-required="true" />
                                <label for="input_Dni" class="no-select">N° Documento</label>
                            </div>
                        </div>
                        <div class="col s12 m6 l4">
                            <div class="input-field">
                                <input id="input_FechaNacimiento" name="date" class="date fechaMenorQueHoy" type="text" maxlength="10" autocomplete="off" required="" aria-required="true" />
                                <label for="input_FechaNacimiento" class="no-select">Fecha de nacimiento</label>
                                <a id="btnFechaNacimiento" class="btn-flat waves-effect btn-redondo chico boton-input"><i class="material-icons">today</i></a>
                            </div>

                            <input id="picker_FechaNacimiento" type="date" class="datepicker" style="display: none;" />

                        </div>
                        <%--  <div class="col s12 m6 l4">
                            <div class="input-field">
                                <input id="input_Cuil" class="cuil" type="text" maxlength="100" />
                                <label for="input_Cuil" class="no-select">N° Cuil</label>
                            </div>
                        </div>--%>
                    </div>

                    <div class="row">
                        <div class="col s12 m6 l4 no-padding">
                            <Controles:SexoSelector runat="server" />
                        </div>
                    </div>
                </div>
            </form>

        </div>

        <div class="row" id="contenedor_InfoDatosPersonalesNoEditable" style="display: none">
            <div id="contenedor_DatosUsuario">
                <div class="imagen"></div>
                <div class="datos">
                    <label id="texto_Nombre"></label>
                    <%--<label id="texto_Dni"></label>--%>
                </div>
            </div>


            <div class="col s12" id="contenedor_HintInfoNoEditable">
                <i class="material-icons">info_outline</i>
                <label>El usuario seleccionado se encuentra validado por el Registro Nacional de Personas. Por lo que sus datos no se pueden editar.</label>
            </div>
        </div>



        <div class="row padding">

            <form id="formDatosContacto">

                <div id="contenedor_Empleado" style="display:none">
                    <div class="col s12 m12 l3">
                        <label class="titulo">Empleado</label>
                    </div>

                    <div class="col s12 m12 l9 no-padding">
                        <div class="col s12 m6">
                            <div class="input-field">
                                <input id="input_Cargo" name="cargo" type="text" maxlength="200" />
                                <label for="input_Cargo" class="no-select">Cargo</label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col s12 m12 l3">
                    <label class="titulo">Contacto</label>
                </div>
                <div class="col s12 m12 l9 no-padding">
                    <div class="row">
                        <div class="col s12 m6 l4">
                            <div class="input-field">
                                <input id="input_Email" name="email" type="email" maxlength="200" required="" aria-required="true" />
                                <label for="input_Email" class="no-select">E-Mail</label>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col s12">
                            <label>Telefono Celular</label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col s12 m2 l1">
                            <div class="input-field">
                                <input id="input_TelefonoCelularCaracteristica" name="telefonoCelularCaracteristica" type="text" maxlength="200" />
                                <label for="input_TelefonoCelularCaracteristica" class="no-select">Area</label>
                            </div>
                        </div>
                        <div class="col s1 m1 l1" id="contenedorHintNumeroCelular">
                            <label>15</label>
                        </div>
                        <div class="col s11 m3 l2">
                            <div class="input-field">
                                <input id="input_TelefonoCelularNumero" name="telefonoCelularNumero" type="text" maxlength="200" />
                                <label for="input_TelefonoCelularNumero" class="no-select">Numero</label>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col s12">
                            <label>Telefono Fijo</label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col s12 m2 l1">
                            <div class="input-field">
                                <input id="input_TelefonoFijoCaracteristica" name="telefonoFijoCaracteristica" type="text" maxlength="200" />
                                <label for="input_TelefonoFijoCaracteristica" class="no-select">Area</label>
                            </div>
                        </div>
                        <div class="col s12 m4 l3">
                            <div class="input-field">
                                <input id="input_TelefonoFijoNumero" name="telefonoFijoNumero" type="text" maxlength="200" />
                                <label for="input_TelefonoFijoNumero" class="no-select">Numero</label>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoEditarReferenteProvisorio.aspx.cs" Inherits="UI.IFrame.IRequerimientoEditarReferenteProvisorio" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IMovilNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <form id="formNuevo" style="height: 100%; overflow: auto;">
        <div style="display: block">
            <div id="contenedor" class="scroll row padding full-height">

                <div class="row">
                    <!-- Telefono -->
                    <div class="col s12 m4">
                        <div class="input-field ">
                            <input id="input_referenteTelefono" type="text" name="telefono" required="" aria-required="true" />
                            <label for="input_referenteTelefono" class="no-select">Teléfono</label>
                            <!-- <a id="errorFormulario_Modelo" class="control-observacion colorTextoError no-select"></a> -->
                        </div>
                    </div>


                    <!-- DNI -->
                    <div class="col s12 m4">
                        <div class="input-field ">
                            <input id="input_referenteDni" name="DNI" type="number" required="" min="1" max="99999999" length="8" aria-required="true" />
                            <label for="input_referenteDni" class="no-select">DNI</label>
                            <a id="errorFormulario_Dni" class="control-observacion colorTextoError no-select"></a>
                        </div>
                    </div>
                </div>

                <div class="row" id="contenedor_DatosAdicionales">
                    <!-- Nombre -->
                    <div class="col s12 m4">
                        <div class="input-field ">
                            <input id="input_referenteNombre" type="text" name="nombre" required="" aria-required="true" />
                            <label for="input_referenteNombre" class="no-select">Nombre</label>
                        </div>
                    </div>

                    <!-- Apellido -->
                    <div class="col s12 m4">
                        <div class="input-field ">
                            <input id="input_referenteApellido" type="text" name="apellido" required="" aria-required="true" />
                            <label for="input_referenteApellido" class="no-select">Apellido</label>
                            <!-- <a id="errorFormulario_Modelo" class="control-observacion colorTextoError no-select"></a> -->
                        </div>
                    </div>

                    <!--Género-->
                    <div class="col s12 m4">
                        <div class="mi-input-field">
                            <label class="no-select">Género</label>
                            <select id="select_referenteGenero" style="width: 100%"></select>
                            <a id="errorFormulario_Genero" class="control-observacion colorTextoError no-select"></a>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col s12">
                        <div class="input-field">
                            <input id="input_referenteObservaciones" type="text" name="observaciones" required=""/>
                            <label for="input_referenteObservaciones" class="no-select">Observaciones</label>
                        </div>
                    </div>
                </div>

                <!-- Mi JS -->
                <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoEditarReferenteProvisorio.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
            </div>
        </div>
    </form>
</asp:Content>
